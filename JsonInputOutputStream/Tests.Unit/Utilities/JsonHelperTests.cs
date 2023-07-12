using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json.Linq;
using System.IO;
using Utilities;
using Utilities.Interfaces;
using Xunit;

namespace Tests.Unit.Utilities
{
    public class JsonHelperTests
    {
        [Fact(DisplayName = "WHEN input is a json file " +
                            "THEN a JObject is returned")]
        public void FileToJObject_Success()
        {
            // Arrange.
            IJsonHelper jsonHelper;
            IFormFile file;
            string path = @".\SampleFiles\Articles.json";
            GetIFormFile(out jsonHelper, path, out file);

            //Act
            var data = jsonHelper.FileToJObject(file);

            //Assert
            Assert.IsType(typeof(JProperty), data.First);

            Assert.True(((JContainer)((JContainer)data.First).First).HasValues);
        }

        [Fact(DisplayName = "WHEN input is a JObject " +
                            "THEN primitive properties are shown first")]
        public void OrderJsonProperties_Success()
        {
            // Arrange.
            IJsonHelper jsonHelper;
            IFormFile file;
            string path = @".\SampleFiles\Products.json";
            GetIFormFile(out jsonHelper, path, out file);

            var jsonData = jsonHelper.FileToJObject(file);

            //Act
            JObject reorderedObject = jsonHelper.OrderJsonProperties(jsonData);

            //Assert
            Assert.IsType(typeof(JProperty), reorderedObject.First);

            Assert.True(((JProperty)reorderedObject.Last).Name.Equals("carts"));
        }

        #region private methods

        private static void GetIFormFile(out IJsonHelper jsonHelper, string path, out IFormFile file)
        {
            jsonHelper = new JsonHelper();
            byte[] byteArray = System.IO.File.ReadAllBytes(path);
            var stream = new MemoryStream(byteArray);
            file = new FormFile(stream, 0, stream.Length, "Articles", "json");
        }

        #endregion private methods
    }
}
