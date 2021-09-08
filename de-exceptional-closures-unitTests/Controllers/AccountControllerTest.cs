using de_exceptional_closures.Controllers;
using de_exceptional_closures.Notify;
using de_exceptional_closures_infraStructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net.Http;
using Xunit;

namespace de_exceptional_closures_unitTests.Controllers
{
    public class AccountControllerTest
    {
        private readonly AccountController controller;
        public AccountControllerTest()
        {
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            var mockNotify = new Mock<INotifyService>();

            var mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
              userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
              null, null, null, null);

            var mockClient = new Mock<IHttpClientFactory>();

            controller = new AccountController(userManagerMock.Object, mockNotify.Object, mockSignInManager.Object, mockClient.Object);
        }

        [Fact]
        public void ForgotPassword()
        {
            //Act
            ViewResult result = controller.ForgotPassword() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ForgotPasswordConfirmation()
        {
            //Act
            ViewResult result = controller.ForgotPasswordConfirmation() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Register()
        {
            //Act
            ViewResult result = controller.Register() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void RegisterConfirmation()
        {
            //Act
            ViewResult result = controller.RegisterConfirmation() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }
    }
}