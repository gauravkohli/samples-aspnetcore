using System;
using Xunit;
using okta_aspnetcore_mvc_example.Controllers;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using okta_aspnetcore_mvc_example.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;

namespace okta_aspnetcore_mvc_example.Tests.Controller
{
    public class HomeControllerUnitTest
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly HomeController _controller;

        public HomeControllerUnitTest(){
            _mockLogger = new Mock<ILogger<HomeController>>();
            _controller =  new HomeController(_mockLogger.Object);
        }

        [Fact]
        public void Index_ActionExecutes_ReturnsView()
        {
            var result = _controller.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Privacy_ActionExecutes_ReturnsView()
        {
            var result = _controller.Privacy();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ActionExecutes_ReturnsView()
        {
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = _controller.Error();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ErrorViewModel>(viewResult.Model);
        }

        [Fact]
        public void Profile_ActionExecutes_ReturnsView()
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

            var result = _controller.Profile();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<System.Security.Claims.Claim>>(viewResult.ViewData.Model);

            var moList  = model.ToList();
            Assert.Single(moList);
            Assert.Equal(claims[0].Value, moList[0].Value);
        }
    }
}
