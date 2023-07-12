using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace Utilities.Interfaces
{
    public interface IJsonHelper
    {
        public JObject OrderJsonProperties(JObject jsonObject);
        public JObject FileToJObject(IFormFile file);
    }
}
