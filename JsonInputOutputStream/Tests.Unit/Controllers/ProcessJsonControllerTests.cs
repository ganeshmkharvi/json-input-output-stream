using JsonInputOutputStream.Controllers;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json.Linq;
using Utilities.Interfaces;
using Xunit;

namespace Tests.Unit.Controllers
{
    public class ProcessJsonControllerTests
    {
        [Fact(DisplayName = "WHEN input is a json file " +
                             "THEN primitive properties are shown first in the new json file")]
        public void Post_Success()
        {
            //Arrange
            var jsonHelperMock = new Mock<IJsonHelper>();
            var jsonObject = new JObject();
            jsonObject.Add("Album", "Me Against The World");
            jsonHelperMock.Setup(x => x.OrderJsonProperties(It.IsAny<JObject>())).Returns(jsonObject);
            var controller = new ProcessJsonController(jsonHelperMock.Object);

            //Act
            var act = controller.Post(Mock.Of<IFormFile>());

            //Assert
            Assert.True(act.ContentType.Equals("application/json"));
        }
    }
}
