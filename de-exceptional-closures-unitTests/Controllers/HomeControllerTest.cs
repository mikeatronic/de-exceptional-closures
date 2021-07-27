using de_exceptional_closures.Controllers;
using Microsoft.AspNetCore.Mvc;
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