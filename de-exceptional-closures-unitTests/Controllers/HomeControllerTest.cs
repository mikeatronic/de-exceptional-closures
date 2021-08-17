using de_exceptional_closures.Controllers;
using de_exceptional_closures_infraStructure.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
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
            var mockMediator = new Mock<IMediator>();

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            controller = new HomeController(mockMediator.Object, userManagerMock.Object);

            // Setup Context
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Add fake Ip address for audit
            IPAddress ip = new IPAddress(16885952);
            controller.HttpContext.Connection.RemoteIpAddress = ip;

        }

        //[Fact]
        //public async Task Index()
        //{
        //    //HttpContext.Session.Set("CreateClosureObj", model)
        //    IndexViewModel model = new IndexViewModel();

        //    controller.HttpContext.Session.Set("CreateClosureObj", model);

        //   //Act
        //   ViewResult result = await controller.Index() as ViewResult;

        //    // Assert
        //    Assert.NotNull(result);
        //}


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