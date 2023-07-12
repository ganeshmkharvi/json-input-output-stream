using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using Utilities;

namespace JsonInputOutputStream.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProcessJsonController : ControllerBase
    {
        private const string ContentType = "application/json";
        private const string Extension = ".json";

        [HttpPost]
        public FileStreamResult Post(IFormFile file)
        {
            JObject jsonData = JsonHelper.FileToJObject(file);
            JObject reorderedObject = JsonHelper.OrderJsonProperties(jsonData);
            return StringToFile(reorderedObject.ToString(), $"{Guid.NewGuid()}{Extension}", ContentType);
        }

        private  FileStreamResult StringToFile(string reorderedJson, string fileName, string contentType)
        {
            var bytes = Encoding.UTF8.GetBytes(reorderedJson);
            MemoryStream ms = new MemoryStream(bytes);
            return File(fileStream: ms, contentType: contentType, fileDownloadName: fileName);
        }
    }
}
