﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectsNetwork.Areas.User.Controllers;
using ProjectsNetwork.Controllers;
using ProjectsNetwork.Data;
using ProjectsNetwork.DataAccess.Repositories;
using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;
using ProjectsNetwork.Services.IApplicationService;
using ProjectsNetwork.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace ProjectsNetwork.Tests.ControllersTests
{
    public class CommunicationControllerTest
    {
        private readonly ControllerContext controllerContext;
        private readonly Mock<IProjectsService> mockProjectService;
        private readonly Mock<ISkillsService> mockSkillsService;
        private readonly Mock<IApplicationUserService> mockApplicationUserService;

        private List<Project> projects;
        private List<UserSkill> skills;


        public CommunicationControllerTest()
        {
            this.mockProjectService = new Mock<IProjectsService>();
            this.mockApplicationUserService = new Mock<IApplicationUserService>();
            this.mockSkillsService = new Mock<ISkillsService>();

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
            var project1 = new Project
            {
                Id = 0,
                UserId = "1",
                Name = "project1",
                Description = "new project 1",
                CreationDate = DateTime.Now,
            };

            var project2 = new Project
            {
                Id = 1,
                UserId = "2",
                Name = "project2",
                Description = "new project 2",
                CreationDate = DateTime.Now,
            };

            this.projects = new List<Project>() { project1, project2 };

            var userskill1 = new UserSkill { SkillId = 1, UserId = "1" };
            var userskill2 = new UserSkill { SkillId = 2, UserId = "2" };

            this.skills = new List<UserSkill>() { userskill1, userskill2 };


        }

        [Fact]
        public void UserInfoTest()
        {

            var user = new ApplicationUser() { UserName = "AppUser", Id = "1" };
            mockApplicationUserService.Setup(service => service.GetApplicationUser(It.IsAny<string>())).Returns(user);
            mockSkillsService.Setup(service => service.GetMySkills(It.IsAny<string>())).Returns(this.skills);
            mockProjectService.Setup(service => service.GetUserProjects(It.IsAny<string>())).Returns(projects);

            var controller = new CommunicationController(mockProjectService.Object, mockApplicationUserService.Object, mockSkillsService.Object);
            controller.ControllerContext = this.controllerContext;
            var result = controller.UserInfo("1");
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<ApplicationUser>(viewResult.ViewData.Model);

            mockProjectService.Reset();
            mockApplicationUserService.Reset();
            mockSkillsService.Reset();

            user = null;
            mockApplicationUserService.Setup(service => service.GetApplicationUser(It.IsAny<string>())).Returns(user);
            
            mockSkillsService.Setup(service => service.GetMySkills(It.IsAny<string>())).Returns(skills);
            mockProjectService.Setup(service => service.GetUserProjects(It.IsAny<string>())).Returns(projects);
            controller = new CommunicationController(mockProjectService.Object, mockApplicationUserService.Object, mockSkillsService.Object);
            controller.ControllerContext = this.controllerContext;
            result = controller.UserInfo("1");
            Assert.IsType<NotFoundObjectResult>(result);

            mockProjectService.Reset();
            mockApplicationUserService.Reset();
            mockSkillsService.Reset();

        }
    }

}