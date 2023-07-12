using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using Utilities;
using Utilities.Interfaces;

namespace JsonInputOutputStream.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProcessJsonController : ControllerBase
    {
        private const string ContentType = "application/json";
        private const string Extension = ".json";
        private readonly IJsonHelper _jsonHelper;

        public ProcessJsonController(IJsonHelper jsonHelper)
        {
            _jsonHelper = jsonHelper;
        }

        [HttpPost]
        public FileStreamResult Post(IFormFile file)
        {
            JObject jsonData = _jsonHelper.FileToJObject(file);
            JObject reorderedObject = _jsonHelper.OrderJsonProperties(jsonData);
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
