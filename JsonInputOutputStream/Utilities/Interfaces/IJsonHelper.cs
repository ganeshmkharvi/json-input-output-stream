using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace Utilities.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IJsonHelper
    {
        /// <summary>
        /// Orders the json properties.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        /// <returns></returns>
        public JObject OrderJsonProperties(JObject jsonObject);
        /// <summary>
        /// Files to j object.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public JObject FileToJObject(IFormFile file);
    }
}
