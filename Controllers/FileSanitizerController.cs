using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace FileSanitizer.Controllers
{
 
    [Route("api/")]
    [ApiController]
    public class FileSanitizerController : ControllerBase
    {
        [HttpPost("sanitize")]
        public async Task<IActionResult> Post()
        {
           
            var file = Request.Form.Files[0];
            var ext = file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            ActionResult result=null;
            switch (ext)
            {
                case "abc":
                     result= await SanitizeABC(file);
                    break;
                default:
                    break;
            }

            return result;
        }

        private async Task<ActionResult> SanitizeABC(IFormFile file)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var streamcontent = new StreamReader(file.OpenReadStream());
            string lineStr = string.Empty;
            string newStr = string.Empty;
            string CurrntDirectory=Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string[] fileParts=file.FileName.Split('.');
            string newFile = fileParts[0] + "Sanitized" + "." + fileParts[1];
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(CurrntDirectory , newFile)))
            {
                while ((lineStr = streamcontent.ReadLine()) != null)
                {
                    if (lineStr.Length > 0)
                    {
                        if (!lineStr.Equals("123") && !lineStr.Equals("789"))
                        {

                            newStr = await SanitizeLine(lineStr);

                        }
                        else
                        {
                            newStr = lineStr;
                        }
                        outputFile.WriteLine(newStr);
                    }
                }
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(CurrntDirectory, newFile));
            return File(fileBytes, "application/force-download", newFile);
        }

        private async Task<string> SanitizeLine(string lineStr)
        {
            string newLine = string.Empty;
            List<string> resulcnkGroupst = new List<string>(Regex.Split(lineStr, @"(?<=\G.{3})", RegexOptions.Singleline));
            for (int i=0; i< resulcnkGroupst.Count-1;++i)
            {
                if(resulcnkGroupst[i].Length>0)
                    newLine +=  ValidateChunk(resulcnkGroupst[i]);
            }
            return newLine;
        }

        private string ValidateChunk(string cnk)
        {
            string newCnk = cnk;
            if (!Char.IsDigit(cnk[1]))
            {
                newCnk = "A225C";
            }

            return newCnk;
        }
    }
}
