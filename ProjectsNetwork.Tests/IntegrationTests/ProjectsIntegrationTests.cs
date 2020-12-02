using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using ProjectsNetwork.Controllers;
using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;
using ProjectsNetwork.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace ProjectsNetwork.Tests.IntegrationTests
{
    public class ProjectsIntegrationTests
    {


        private readonly ControllerContext controllerContext;
        private readonly Mock<IProjectRepository> _projectRepository;
        private readonly Mock<IInterestedInProjectRepository> _interestedInProjectRepository;
        private readonly Mock<IApplicationUserRepository> _applicationUserRepository;
        private readonly Mock<IUserSkillRepository> _userSkillRepository;
        private readonly Mock<ISkillRepository> _skillRepository;
        private readonly Mock<IProjectSkillRepository> _projectSkillRepository;
        private List<Skill> skills;
        private List<Project> projects;

        public ProjectsIntegrationTests()
        {

            this._projectRepository = new Mock<IProjectRepository>();
            this._interestedInProjectRepository = new Mock<IInterestedInProjectRepository>();
            this._applicationUserRepository = new Mock<IApplicationUserRepository>();
            this._userSkillRepository = new Mock<IUserSkillRepository>();
            this._skillRepository = new Mock<ISkillRepository>();
            this._projectSkillRepository = new Mock<IProjectSkillRepository>();


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

        public void Seed()
        {

            var skill1 = new Skill { Id = 1, SkillName = "Java" };
            var skill2 = new Skill { Id = 2, SkillName = "C#" };
            this.skills = new List<Skill>() { skill1, skill2 };

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
                PrefferedSkills = new List<ProjectSkill>() { new ProjectSkill { SkillId = 1} }
            };

            this.projects = new List<Project>() { project1, project2 };

        }


        [Fact]
        public void IndexTest()
        {
            _projectRepository.Setup(service => service.GetAll(It.IsAny<Expression<Func<Project, bool>>>(), null, null)).Returns(this.projects);
            var projectService = new ProjectsService(_projectRepository.Object, _interestedInProjectRepository.Object, _applicationUserRepository.Object, _userSkillRepository.Object, _skillRepository.Object);
            var skillsService = new SkillsService(_skillRepository.Object, _projectSkillRepository.Object, _userSkillRepository.Object);

            var postsController = new PostsController(projectService, skillsService);
            postsController.ControllerContext = this.controllerContext;

            var result = postsController.Index("");
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<List<Project>>(viewResult.ViewData.Model);
            _projectRepository.Reset();


            _projectRepository.Setup(service => service.GetAll(It.IsAny<Expression<Func<Project, bool>>>(), null, It.IsAny<String>())).Returns(this.projects);
            _skillRepository.Setup(service => service.Get(1)).Returns(this.skills[0]);
            projectService = new ProjectsService(_projectRepository.Object, _interestedInProjectRepository.Object, _applicationUserRepository.Object, _userSkillRepository.Object, _skillRepository.Object);
            skillsService = new SkillsService(_skillRepository.Object, _projectSkillRepository.Object, _userSkillRepository.Object);

            postsController = new PostsController(projectService, skillsService);
            postsController.ControllerContext = this.controllerContext;

            result = postsController.Index("Java");
            viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<List<Project>>(viewResult.ViewData.Model);
            _projectRepository.Reset();
            _skillRepository.Reset();

        }


        [Fact]
        public void PostTest()
        {
            _projectRepository.Setup(service => service.Insert(It.IsAny<Project>())).Returns((EntityEntry<Project>)FormatterServices.GetUninitializedObject(typeof(EntityEntry<Project>)));
            _projectRepository.Setup(service => service.Save()).Returns(1);

            var projectService = new ProjectsService(_projectRepository.Object, _interestedInProjectRepository.Object, _applicationUserRepository.Object, _userSkillRepository.Object, _skillRepository.Object);
            var skillsService = new SkillsService(_skillRepository.Object, _projectSkillRepository.Object, _userSkillRepository.Object);

            var postsController = new PostsController(projectService, skillsService);
            postsController.ControllerContext = this.controllerContext;

            var result = postsController.Post(new Project(), new int[] { 1, 2 });
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(viewResult.ControllerName);
            Assert.Equal("MyProjects", viewResult.ActionName);
            _projectRepository.Reset();


            //testing failure scenario
            _projectRepository.Setup(repo => repo.Insert(It.IsAny<Project>())).Returns((EntityEntry<Project>)FormatterServices.GetUninitializedObject(typeof(EntityEntry<Project>)));
            _projectRepository.Setup(repo => repo.Save()).Returns(0);

            projectService = new ProjectsService(_projectRepository.Object, _interestedInProjectRepository.Object, _applicationUserRepository.Object, _userSkillRepository.Object, _skillRepository.Object);
            skillsService = new SkillsService(_skillRepository.Object, _projectSkillRepository.Object, _userSkillRepository.Object);

            postsController = new PostsController(projectService, skillsService);
            postsController.ControllerContext = this.controllerContext;

            Assert.Throws<Exception>(() => postsController.Post(new Project(), new int[] { 1, 2 }));
            _projectRepository.Reset();

        }


        [Fact]
        public void AddNewSkillTest()
        {
            _skillRepository.Setup(repo => repo.Insert(It.IsAny<Skill>())).Returns((EntityEntry<Skill>)FormatterServices.GetUninitializedObject(typeof(EntityEntry<Skill>)));
            _skillRepository.Setup(repo => repo.Save()).Returns(1);

            var projectService = new ProjectsService(_projectRepository.Object, _interestedInProjectRepository.Object, _applicationUserRepository.Object, _userSkillRepository.Object, _skillRepository.Object);
            var skillsService = new SkillsService(_skillRepository.Object, _projectSkillRepository.Object, _userSkillRepository.Object);

            var postsController = new PostsController(projectService, skillsService);
            postsController.ControllerContext = this.controllerContext;

            var result = postsController.AddNewSkill(new Skill());
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(viewResult.ControllerName);
            Assert.Equal("Post", viewResult.ActionName);
            _skillRepository.Reset();

            _skillRepository.Setup(repo => repo.Insert(It.IsAny<Skill>())).Returns((EntityEntry<Skill>)FormatterServices.GetUninitializedObject(typeof(EntityEntry<Skill>)));
            _skillRepository.Setup(repo => repo.Save()).Returns(0);

            projectService = new ProjectsService(_projectRepository.Object, _interestedInProjectRepository.Object, _applicationUserRepository.Object, _userSkillRepository.Object, _skillRepository.Object);
            skillsService = new SkillsService(_skillRepository.Object, _projectSkillRepository.Object, _userSkillRepository.Object);

            postsController = new PostsController(projectService, skillsService);
            postsController.ControllerContext = this.controllerContext;

            Assert.Throws<Exception>(() => postsController.AddNewSkill(new Skill()));
            _skillRepository.Reset();



        }

    }
}
