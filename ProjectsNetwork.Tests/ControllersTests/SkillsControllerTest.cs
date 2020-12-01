using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectsNetwork.Controllers;
using ProjectsNetwork.Models;
using ProjectsNetwork.Services.IServices;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace ProjectsNetwork.Tests.ControllersTests
{
    public class SkillsControllerTest
    {
        private readonly ControllerContext controllerContext;
        private readonly Mock<ISkillsService> mockSkillService;
        private List<Skill> skills;
        private List<UserSkill> userSkills;

        public SkillsControllerTest()
        {
            this.mockSkillService = new Mock<ISkillsService>();

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


        public void Seed() {

            var skill1 = new Skill { Id = 1, SkillName = "Java" };
            var skill2 = new Skill { Id = 2, SkillName = "C#" };
            this.skills = new List<Skill>() { skill1, skill2 };


            var userSkill1 = new UserSkill { UserId = "1", Skill = skill1 };
            var userSkill2 = new UserSkill { UserId = "1", Skill = skill2 };
            this.userSkills = new List<UserSkill>() { userSkill1, userSkill2 };
        
        }


        [Fact]
        public void IndexTest()
        {

            //testing only positive case as the UserNotFound case is nearly impossible to mock and it is unlikely to happen because of the way .Net MVC is set up
            mockSkillService.Setup(service => service.GetAll()).Returns(this.skills);
            mockSkillService.Setup(service => service.GetMySkills(It.IsAny<string>())).Returns(this.userSkills);
            var controller = new SkillsController(mockSkillService.Object);
            controller.ControllerContext = this.controllerContext;
            var result = controller.Index();
            var viewResult =  Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<Tuple<List<Skill>, List<Skill>, Skill>>(viewResult.ViewData.Model);


            mockSkillService.Reset(); //clearing up mock
            
        }


        [Fact]
        public void PostUserSkillsTest()
        {
            //testing successfull run
            mockSkillService.Setup(service => service.PostUserSkills(It.IsAny<string>(), It.IsAny<int[]>())).Returns(true);
            var controller = new SkillsController(mockSkillService.Object);
            controller.ControllerContext = this.controllerContext;
            var result = controller.PostUserSkills(new int[] { 0, 1 });
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(viewResult.ControllerName);
            Assert.Equal("Index", viewResult.ActionName);
            mockSkillService.Reset();


            //testing exception
            mockSkillService.Setup(service => service.PostUserSkills(It.IsAny<string>(), It.IsAny<int[]>())).Returns(false);
            controller = new SkillsController(mockSkillService.Object);
            controller.ControllerContext = this.controllerContext;
            var ex = Assert.Throws<Exception>(() => controller.PostUserSkills(new int[] { 0, 1 }));
            Assert.Equal("Could not add skills.", ex.Message);
            mockSkillService.Reset();

        }


        [Fact]
        public void AddNewSkillTest() {

            //testing successfull run
            mockSkillService.Setup(service => service.AddSkill(It.IsAny<Skill>())).Returns(true);
            var controller = new SkillsController(mockSkillService.Object);
            controller.ControllerContext = this.controllerContext;
            var result = controller.AddNewSkill(new Skill());
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(viewResult.ControllerName);
            Assert.Equal("Index", viewResult.ActionName);
            mockSkillService.Reset();


            //testing exception
            mockSkillService.Setup(service => service.AddSkill(It.IsAny<Skill>())).Returns(false);
            controller = new SkillsController(mockSkillService.Object);
            controller.ControllerContext = this.controllerContext;
            var ex = Assert.Throws<Exception>(() => controller.AddNewSkill(new Skill()));
            Assert.Equal("Could not add skill.", ex.Message);
            mockSkillService.Reset();

        }

    }
}
