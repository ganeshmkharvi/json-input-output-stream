using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

namespace Utilities
{
    public static class JsonHelper
    {
        public static JObject OrderJsonProperties(JObject jsonObject)
        {
            // Create a new JObject to store the reordered properties
            JObject reorderedObject = new JObject();

            // Move the primitive properties to the new JObject
            foreach (JProperty property in jsonObject.Properties())
            {
                if (property.Value.Type != JTokenType.Object && property.Value.Type != JTokenType.Array)
                    reorderedObject.Add(property.Name, property.Value);
            }

            // Move the complex object properties to the new JObject
            foreach (JProperty property in jsonObject.Properties())
            {
                if (property.Value.Type == JTokenType.Object)
                {
                    var data = OrderJsonProperties((JObject)property.Value);
                    if (property.Value.Type == JTokenType.Array)
                    {
                        reorderedObject = data;
                    }
                    else
                    {
                        reorderedObject.Add(property.Name, data);
                    }
                }

            }

            foreach (JProperty property in jsonObject.Properties())
            {
                if (property.Value.Type == JTokenType.Array)
                {
                    var arrdata = AddArrayData(reorderedObject, property);
                    reorderedObject.Add(property.Name, arrdata.First.FirstOrDefault());
                }
            }

            return reorderedObject;
        }

        private static JObject AddArrayData(JObject reorderedObject, JProperty property)
        {
            JObject obj = new JObject();

            foreach (var item in property.ToList())
            {
                if (item.Type == JTokenType.Array)
                {
                    if (item.First.Type != JTokenType.Object)
                    {
                        obj.Add(property.Name, property.Value);
                    }

                    if (item.First.Type == JTokenType.Object)
                    {
                        if (property.Value.Type == JTokenType.Array)
                        {
                            obj.Add(property.Name, HandleNestedArray((JArray)property.Value));
                        }
                    }
                }
                if (property.Value.Type == JTokenType.Object)
                {
                    obj.Add(property.Name, OrderJsonProperties((JObject)property.Value));
                }

            }

            return obj;
        }

        private static JArray HandleNestedArray(JArray array)
        {
            JArray arr = new JArray();
            foreach (var arItem in array)
            {
                arr.Add(HandleArray(arItem));
            }
            return arr;
        }

        private static JObject HandleArray(JToken item)
        {
            JObject obj = new JObject();
            var prop = (JObject)item;
            foreach (var pItem in prop.Properties())
            {
                if (pItem.Value.Type != JTokenType.Object && pItem.Value.Type != JTokenType.Array)
                {
                    obj.Add(pItem.Name, pItem.Value);
                }
            }
            foreach (var pItem in prop.Properties())
            {
                if (pItem.Value.Type == JTokenType.Object)
                {
                    obj.Add(pItem.Name, OrderJsonProperties((JObject)pItem.Value));
                }
            }

            foreach (var pItem in (prop).Properties())
            {
                if (pItem.Value.Type == JTokenType.Array)
                {

                    if (pItem.First.Type == JTokenType.Array)
                    {
                        if (pItem.First.FirstOrDefault().Type != JTokenType.Object &&
                            pItem.First.FirstOrDefault().Type != JTokenType.Array)
                        {
                            obj.Add(pItem.Name, pItem.Value);
                        }
                        else
                        {
                            obj.Add(pItem.Name, HandleNestedArray((JArray)pItem.Value));
                        }
                    }

                    if (pItem.First.Type == JTokenType.Object)
                    {
                        if (pItem.Value.Type == JTokenType.Array)
                        {
                            obj.Add(pItem.Name, HandleNestedArray((JArray)pItem.Value));
                        }
                    }

                }
            }


            return obj;
        }

        public static JObject FileToJObject(IFormFile file)
        {
            string fileContent = null;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                fileContent = reader.ReadToEnd();
            }

            // Parse the JSON string into a JObject
            return JObject.Parse(fileContent);
        }

    }
}
