using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using Utilities.Interfaces;

namespace JsonInputOutputStream.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProcessJsonController : ControllerBase
    {
        /// <summary>
        /// The content type
        /// </summary>
        private const string ContentType = "application/json";
        /// <summary>
        /// The extension
        /// </summary>
        private const string Extension = ".json";
        /// <summary>
        /// The json helper
        /// </summary>
        private readonly IJsonHelper _jsonHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessJsonController"/> class.
        /// </summary>
        /// <param name="jsonHelper">The json helper.</param>
        public ProcessJsonController(IJsonHelper jsonHelper)
        {
            _jsonHelper = jsonHelper;
        }

        /// <summary>
        /// Posts the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        [HttpPost]
        public FileStreamResult Post(IFormFile file)
        {
            JObject jsonData = _jsonHelper.FileToJObject(file);
            JObject reorderedObject = _jsonHelper.OrderJsonProperties(jsonData);
            return StringToFile(reorderedObject.ToString(), $"{Guid.NewGuid()}{Extension}", ContentType);
        }

        /// <summary>
        /// Strings to file.
        /// </summary>
        /// <param name="reorderedJson">The reordered json.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns></returns>
        private FileStreamResult StringToFile(string reorderedJson, string fileName, string contentType)
        {
            var bytes = Encoding.UTF8.GetBytes(reorderedJson);
            MemoryStream ms = new MemoryStream(bytes);
            return File(fileStream: ms, contentType: contentType, fileDownloadName: fileName);
        }
    }
}
