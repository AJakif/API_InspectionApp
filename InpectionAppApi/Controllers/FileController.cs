using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InpectionAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        public static IWebHostEnvironment _webHostEnv;

        public FileController(IWebHostEnvironment webHostEnv)
        {
            _webHostEnv = webHostEnv;
        }


        [HttpPost]
        [Route("api/upload")]
        public string Upload()
        {

            var objFile = HttpContext.Request.Form.Files[0];
            try
            {
                if (objFile.Length > 0)
                {
                    string path = _webHostEnv.WebRootPath + "\\uploads\\";
                    if (!Directory.Exists(path))
                    {
                        //creating path
                        Directory.CreateDirectory(path);
                    }
                    //unzip here
                    using FileStream fileStream = System.IO.File.Create(path + objFile.FileName);
                    objFile.CopyTo(fileStream);
                    fileStream.Flush();
                    return "Uploaded.";
                }
                else
                {
                    return "Not Uploaded.";
                }
            }
            catch (Exception e)
            {
                return e.Message;

            }
        }
    }
}
