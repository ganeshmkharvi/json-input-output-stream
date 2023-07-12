using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using Utilities.Interfaces;

namespace Utilities
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Utilities.Interfaces.IJsonHelper" />
    public class JsonHelper: IJsonHelper
    {
        /// <summary>
        /// Orders the json properties.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        /// <returns></returns>
        public JObject OrderJsonProperties(JObject jsonObject)
        {
            // Create a new JObject to store the reordered properties
            JObject reorderedObject = new JObject();
            HandlePrimitiveTypes(jsonObject, reorderedObject);
            HandleObject(jsonObject, reorderedObject);

            foreach (JProperty property in jsonObject.Properties())
            {
                if (property.Value.Type == JTokenType.Array)
                {
                    var arrdata = AddArrayData(property);
                    reorderedObject.Add(property.Name, arrdata.First.FirstOrDefault());
                }
            }

            return reorderedObject;
        }

        /// <summary>
        /// Handles the object.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        /// <param name="reorderedObject">The reordered object.</param>
        private void HandleObject(JObject jsonObject, JObject reorderedObject)
        {
            // Move the complex object properties to the new JObject
            foreach (JProperty property in jsonObject.Properties())
            {
                if (property.Value.Type == JTokenType.Object)
                {
                    reorderedObject.Add(property.Name, OrderJsonProperties((JObject)property.Value));

                }

            }
        }

        /// <summary>
        /// Handles the primitive types.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        /// <param name="reorderedObject">The reordered object.</param>
        private void HandlePrimitiveTypes(JObject jsonObject, JObject reorderedObject)
        {
            // Move the primitive properties to the new JObject
            foreach (JProperty property in jsonObject.Properties())
            {
                if (property.Value.Type != JTokenType.Object && property.Value.Type != JTokenType.Array)
                    reorderedObject.Add(property.Name, property.Value);
            }
        }

        /// <summary>
        /// Adds the array data.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        private JObject AddArrayData(JProperty property)
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

        /// <summary>
        /// Handles the nested array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns></returns>
        private JArray HandleNestedArray(JArray array)
        {
            JArray arr = new JArray();
            foreach (var arItem in array)
            {
                arr.Add(HandleArray(arItem));
            }
            return arr;
        }

        /// <summary>
        /// Handles the array.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private JObject HandleArray(JToken item)
        {
            JObject obj = new JObject();
            var prop = (JObject)item;
        
            HandlePrimitiveTypes(prop, obj);
            HandleObject(prop, obj);

            foreach (var pItem in prop.Properties())
            {
                if (pItem.Value.Type == JTokenType.Array)
                {

                    if (pItem.First.Type == JTokenType.Array)
                    {
                        if (pItem?.First?.FirstOrDefault()?.Type != JTokenType.Object &&
                            pItem?.First?.FirstOrDefault()?.Type != JTokenType.Array)
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

        /// <summary>
        /// Files to j object.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public JObject FileToJObject(IFormFile file)
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
