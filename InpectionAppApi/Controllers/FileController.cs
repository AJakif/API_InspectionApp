using InspectionAppApi.BusinessLayer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace InpectionAppApi.Controllers
{
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
        public async Task<IActionResult> Upload([FromForm] FileUploadBO objFile)
        {
            try
            {
                if (objFile.Files.Length > 0)
                {
                    string length = objFile.Files.Length.ToString();
                    string file = Path.GetFileNameWithoutExtension(objFile.Files.FileName);
                    string Filepath = _webHostEnv.WebRootPath + "\\" + file + "\\";
                    if (!Directory.Exists(Filepath))
                    {
                        //creating File path
                        Directory.CreateDirectory(Filepath);
                    }
                    string Zippath = _webHostEnv.WebRootPath + "\\zip\\";
                    if (!Directory.Exists(Zippath))
                    {
                        //creating Zip path
                        Directory.CreateDirectory(Zippath);
                    }
                    using (var stream = new FileStream(Zippath + objFile.Files.FileName, FileMode.Create))
                    {
                        //Saving zip file
                        await objFile.Files.CopyToAsync(stream);
                    }

                    //extract Zip file
                    ZipFile.ExtractToDirectory(Zippath + objFile.Files.FileName, Filepath);
                    
                    //Delete Zip Folder
                    DirectoryInfo di = new DirectoryInfo(Zippath);

                    foreach (FileInfo files in di.GetFiles())
                    {
                        files.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }

                    return Ok(new { Length = length, Name = file});
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }
    }
}
