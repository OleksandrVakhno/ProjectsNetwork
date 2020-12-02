using ProjectsNetwork.Services;
using ProjectsNetwork.Models;
using ProjectsNetwork.DataAccess.Repositories.MockRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ProjectsNetwork.Tests.ServicesTests
{
    public class  ProjectsServiceTest
    {

        private readonly ProjectsService _projectsService;
        private readonly MockProjectRepository _projectsRepository;
        private readonly MockApplicationUserRepository _applicationUserRepository;
        private readonly MockInterestedInProjectRepository _interestedInProjectRepository;
        private readonly MockSkillRepository _skillRepository;
        private readonly MockUserSkillRepository _userSkillRepository;
  



        public ProjectsServiceTest()
        {


            this._projectsRepository = new MockProjectRepository();
            this._applicationUserRepository = new MockApplicationUserRepository();
            this._interestedInProjectRepository = new MockInterestedInProjectRepository();
            this._skillRepository = new MockSkillRepository();
            this._userSkillRepository = new MockUserSkillRepository();
            

            this.Seed();

            this._projectsService = new ProjectsService(_projectsRepository, _interestedInProjectRepository, _applicationUserRepository, _userSkillRepository, _skillRepository);
        }

        private void Seed()
        {

            this._projectsRepository.Insert(new Project { UserId = "1" });
            this._interestedInProjectRepository.Insert(new InterestedInProject { UserId = "1", ProjectId = 5, Confirmed = true });


            var projects1 = new Project { Id = 1, Name = "Project1", Description = "New Project1", CreationDate = DateTime.Now};
            var projects2 = new Project { Id = 2, Name = "Project2", Description = "New Project2", CreationDate = DateTime.Now};
            var project3 = new Project { Id = 3, Name = "Project3", UserId = "1" };
            var project4 = new Project { Id = 4 };
            var project6 = new Project { Id = 6 };
            var project7 = new Project { Id = 7 };
            this._projectsRepository.Insert(projects1);
            this._projectsRepository.Insert(projects2);
            this._projectsRepository.Insert(project3);
            this._projectsRepository.Insert(project4);
            this._projectsRepository.Insert(project6);
            this._projectsRepository.Insert(project7);

            var user1 = new ApplicationUser { Id = "1" };
            this._applicationUserRepository.Insert(user1);


            var interested1 = new InterestedInProject
            {
                UserId = "1",
                ProjectId = 1,

            };

            var interested2 = new InterestedInProject
            {
                UserId = "1",
                ProjectId = 2,
             
            };
            var interested3 = new InterestedInProject
            {
                UserId = "5",
                ProjectId = 4
            };
            this._interestedInProjectRepository.Insert(interested1);
            this._interestedInProjectRepository.Insert(interested2);
            this._interestedInProjectRepository.Insert(interested3);

            var applicationuser1 = new ApplicationUser { InterestedInProjects = new List<InterestedInProject> { interested1, interested2 } };
            this._applicationUserRepository.Insert(applicationuser1);


        }

        [Fact]
        public void GetAllTest()
        {
            var projects = this._projectsService.GetAll();
            Assert.NotEmpty(projects);

        }

        [Fact]
        public void GetInterestedTest()
        {
            
            var projects = this._projectsService.GetInterested(2);
            Assert.NotNull(projects);
            Assert.NotEmpty(projects);

            projects = this._projectsService.GetInterested(3);
            Assert.NotNull(projects);
            Assert.Empty(projects);

            Assert.Throws<Exception>(() => this._projectsService.GetInterested(4));

        }

        [Fact]
        public void GetProjectTest()
        {
            var project = this._projectsService.GetProject(2);
            Assert.NotNull(project);

            project = this._projectsService.GetProject(5);
            Assert.Null(project);

        }

        [Fact]
        public void GetAcceptedProjectTest()
        {
            var project = this._projectsService.GetAcceptedProjects("1");
            Assert.NotNull(project);

           
        }

        [Fact]
        public void GetMatchesTest()
        {
            var result = this._projectsService.GetMatches("1");
            Assert.NotEmpty(result);

            result = this._projectsService.GetMatches("5");
            Assert.Empty(result);


        }

        [Fact]
        public void GetUserProjectsTest()
        {
            
            var project = this._projectsService.GetUserProjects("1");
            Assert.NotEmpty(project);

            project = this._projectsService.GetUserProjects("7");
            Assert.Empty(project);

        }

        

        [Fact]
        public void PostProjectTest()
        {
            var project1 = new Project
            {
                Id = 0,
                UserId = "1",
                Name = "project1",
                Description = "new project 1",
                CreationDate = DateTime.Now,
            };

            _projectsRepository.setInsertFailure(true);
            var result = _projectsService.PostProject(project1, new int[] { 0, 1, 2 });
            Assert.False(result);
            _projectsRepository.setThrowsException(true);
            Assert.Throws<Exception>(() => _projectsService.PostProject(project1, new int[] { 0, 1, 2 }));
            _projectsRepository.setInsertFailure(false);
            _projectsRepository.setThrowsException(false);

            _projectsRepository.setSaveFailure(true);
            result = _projectsService.PostProject(project1, new int[] { 0, 1, 2 });
            Assert.False(result);
            _projectsRepository.setThrowsException(true);
            Assert.Throws<Exception>(() => _projectsService.PostProject(project1, new int[] { 0, 1, 2 }));
            _projectsRepository.setSaveFailure(false);
            _projectsRepository.setThrowsException(false);

            result = _projectsService.PostProject(project1, new int[] { 0, 1, 2 });
            Assert.True(result);
        }

        [Fact]
        public void SubmitInterestProjectTest()
        {

            var result = this._projectsService.SubmitInterest("1", 4);
            Assert.True(result);

            //already interested
            result = this._projectsService.SubmitInterest("1", 4);
            Assert.False(result);

            //user cannot submit interest to his project
            result = this._projectsService.SubmitInterest("1", 3);
            Assert.False(result);

            _interestedInProjectRepository.setInsertFailure(true);
            result = _projectsService.SubmitInterest("1", 6);
            Assert.False(result);
            _interestedInProjectRepository.setThrowsException(true);
            Assert.Throws<Exception>(() => _projectsService.SubmitInterest("1", 6));
            _interestedInProjectRepository.setInsertFailure(false);
            _interestedInProjectRepository.setThrowsException(false);
            

            _interestedInProjectRepository.setSaveFailure(true);
            result = _projectsService.SubmitInterest("1", 6);
            Assert.False(result);
            _interestedInProjectRepository.setThrowsException(true);
            Assert.Throws<Exception>(() => _projectsService.SubmitInterest("1", 7));
            _interestedInProjectRepository.setSaveFailure(false);
            _interestedInProjectRepository.setThrowsException(false);

        }
        

        [Fact]
        public void CancelInterest()
        {
            
            _interestedInProjectRepository.setRemoveFailure(true);
            var result = _projectsService.CancelInterest("1", 1);
            Assert.False(result);
            _interestedInProjectRepository.setThrowsException(true);
            Assert.Throws<Exception>(() => _projectsService.CancelInterest("1", 1));
            _interestedInProjectRepository.setRemoveFailure(false);
            _interestedInProjectRepository.setThrowsException(false);

            _interestedInProjectRepository.setSaveFailure(true);
            result = _projectsService.CancelInterest("1", 4);
            Assert.False(result);
            _interestedInProjectRepository.setThrowsException(true);
            Assert.Throws<Exception>(() => _projectsService.CancelInterest("1", 1));
            _interestedInProjectRepository.setSaveFailure(false);
            _interestedInProjectRepository.setThrowsException(false);


            result = this._projectsService.CancelInterest("1", 2);
            Assert.True(result);

        }

        [Fact]
        public void AcceptInterestTest()
        {
            var result = this._projectsService.AcceptInterest("1", 1);
            Assert.True(result);
            _interestedInProjectRepository.setSaveFailure(true);
            result = _projectsService.AcceptInterest("1", 1);
            Assert.False(result);
            _interestedInProjectRepository.setUpdateFailure(true);
            Assert.Throws<Exception>(() => _projectsService.AcceptInterest("1", 1));
            _interestedInProjectRepository.setSaveFailure(false);
            _interestedInProjectRepository.setUpdateFailure(false);


        }

        





    }
}
