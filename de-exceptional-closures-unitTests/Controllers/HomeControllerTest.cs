using de_exceptional_closures.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace de_exceptional_closures_unitTests.Controllers
{
    public class HomeControllerTest
    {
        private readonly HomeController controller;

        public HomeControllerTest()
        {
            // Arrange
            controller = new HomeController();

            // Setup Context
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Add fake Ip address for audit
            IPAddress ip = new IPAddress(16885952);
            controller.HttpContext.Connection.RemoteIpAddress = ip;
        }

        [Fact]
        public void Accessibility()
        {
            //Act
            ViewResult result = controller.Accessibility() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Cookies()
        {
            //Act
            ViewResult result = controller.Cookies() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Privacy()
        {
            //Act
            ViewResult result = controller.Privacy() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }
    }
}