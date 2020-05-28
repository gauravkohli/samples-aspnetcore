using System;
using Xunit;
using okta_aspnetcore_mvc_example.Controllers;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;


namespace okta_aspnetcore_mvc_example.Tests.Controller
{
    public class AccountControllerUnitTest
    {
        private readonly AccountController _controller;

        public AccountControllerUnitTest(){
            _controller =  new AccountController();
        }

        [Fact]
        public void SignIn_ActionExecutes_ReturnsView()
        {
            var result = _controller.SignIn();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void SignIn_ActionExecutes_ReturnsChallengeIfNotAuthenticated()
        {
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var token = "sampleSessionToken";
            var result = _controller.SignIn(token);
            var challenge = Assert.IsType<ChallengeResult>(result);

            Assert.Equal("/Home/", challenge.Properties.RedirectUri);
            Assert.Equal(token, challenge.Properties.Items["sessionToken"]);
        }        

        [Fact]
        public void SignIn_ActionExecutes_ReturnsRedirectActionIfAuthenticated()
        {

            var claims = new List<Claim>() 
            {
                new Claim("name", "John Doe")
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.User = claimsPrincipal;

            var token = "some token";
            var result = _controller.SignIn(token);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }    


        [Fact]
        public void SignOut_ActionExecutes_ReturnsView()
        {
            var result = _controller.SignOut();
            var signOutResult =  Assert.IsType<SignOutResult>(result);
            Assert.Equal("/Home/", signOutResult.Properties.RedirectUri);
        }

    }
}
