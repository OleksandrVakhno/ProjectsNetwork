using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectsNetwork.Areas.User.Controllers;
using ProjectsNetwork.Controllers;
using ProjectsNetwork.Data;
using ProjectsNetwork.DataAccess.Repositories;
using ProjectsNetwork.Models;
using ProjectsNetwork.Services.IServices;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Xunit;
namespace ProjectsNetwork.Tests.ControllersTests
{
    public class MatchesControllerTest
    {
        private readonly ControllerContext controllerContext;
        private readonly Mock<IProjectsService> mockProjectService;
        private Project project;
        private readonly InterestedInProjectRepository _interestedInProject;
        private readonly ApplicationDbContext _db;

        public MatchesControllerTest()
        {
                    this.mockProjectService = new Mock<IProjectsService>();

                var user = new ApplicationUser() { UserName = "AppUser", Id = "1" };

                    var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim("name", user.UserName),
                };

                    var identity = new ClaimsIdentity(claims, "Test");
                    var claimsPrincipal = new ClaimsPrincipal(identity);

                    this.controllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext
                        {
                            User = claimsPrincipal
                        }
                    };

                    this.Seed();
                }

                [Fact]
                public void Seed()
                {
                   var project = new Project
                    {
                        Id = 0,
                        UserId = "1",
                        Name = "project1",
                        Description = "new project 1",
                        CreationDate = DateTime.Now,
                    };

                _interestedInProject.Insert(new InterestedInProject()
                {
                    UserId = "1", ProjectId = 1,
                    User= new ApplicationUser() { UserName = "AppUser", Id = "1" },
                    Project=this.project,
                    Confirmed=true
                });

                }

            [Fact]
            public void AcceptedTest()
            {
                var interesteduser = this._interestedInProject.GetAll();
                mockProjectService.Setup(service => service.GetAcceptedProjects(It.IsAny<string>())).Returns(interesteduser);
                var controller = new MatchesController(mockProjectService.Object);
                controller.ControllerContext = this.controllerContext;
                var result = controller.Accepted();
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsAssignableFrom<Project>(viewResult.ViewData.Model);

                mockProjectService.Reset();

            }

            [Fact]
            public void MatchesTest()
            {
                var interesteduser = _interestedInProject.GetAll();
                mockProjectService.Setup(service => service.GetMatches(It.IsAny<string>())).Returns(interesteduser);
                var controller = new MatchesController(mockProjectService.Object);
                controller.ControllerContext = this.controllerContext;
                var result = controller.Matches();
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsAssignableFrom<Project>(viewResult.ViewData.Model);

                mockProjectService.Reset();
            }

            [Fact]
            public void InterestedTest()
            {
                var AppUserRepo = new ApplicationUserRepository(_db);
                var appUser = AppUserRepo.GetAll();
                mockProjectService.Setup(service => service.GetInterested(It.IsAny<int>())).Returns(appUser);
                var controller = new MatchesController(mockProjectService.Object);
                controller.ControllerContext = this.controllerContext;
                var result = controller.Interested(0);
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsAssignableFrom<Project>(viewResult.ViewData.Model);

                mockProjectService.Reset();
            }

            [Fact]
            public void AcceptInterestTest()
            {
                mockProjectService.Setup(service => service.AcceptInterest(It.IsAny<string>(), It.IsAny<int>())).Returns(true);
                var controller = new MatchesController(mockProjectService.Object);
                controller.ControllerContext = this.controllerContext;
                var result = controller.AcceptInterest(0,"1");
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Null(viewResult.ControllerName);
                Assert.Equal("Accepted", viewResult.ActionName);

                mockProjectService.Reset();

                mockProjectService.Setup(service => service.AcceptInterest(It.IsAny<string>(), It.IsAny<int>())).Returns(true);
                controller = new MatchesController(mockProjectService.Object);
                controller.ControllerContext = this.controllerContext;
                result = controller.AcceptInterest(0, "1");
                Assert.Equal("Failed to accept interest", result.ToString());

                mockProjectService.Reset();
        }
        }
        
    }

