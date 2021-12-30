using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace InspectionAppApi.BusinessLayer
{
    public class FileUploadBO
    {
        public IFormFile Files { get; set; }
    }
}
