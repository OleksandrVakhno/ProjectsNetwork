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
        private readonly MockUserSkillRepository _userSkillRepository;
        private readonly MockSkillRepository _skillRepository;



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
            var userSkill1 = new UserSkill { UserId = "1", SkillId = 1, Skill = new Skill { Id = 4, SkillName = "C++"} };
            var userSkill2 = new UserSkill { UserId = "2", SkillId = 2, Skill= new Skill { Id = 3, SkillName = "Python" } };
            this._userSkillRepository.Insert(userSkill1);
            this._userSkillRepository.Insert(userSkill2);

            var skill1 = new Skill { Id = 1, SkillName = "Java", Users = new List<UserSkill> { userSkill1 } };
            var skill2 = new Skill { Id = 2, SkillName = "C++", Users = new List<UserSkill> { userSkill2 } };
            this._skillRepository.Insert(skill1);
            this._skillRepository.Insert(skill2);


            var projects1 = new Project { Id = 1, Name = "Project1", Description = "New Project1", CreationDate = DateTime.Now, UserId ="1",
                User = new ApplicationUser()
                {
                    Skills = new List<UserSkill> { new UserSkill()
                    {
                        UserId = "1",
                        SkillId = 3
                    }
                    }

                },
                PrefferedSkills = new List<ProjectSkill> { new ProjectSkill { ProjectId = 3, Skill = skill1} }
                };
            var projects2 = new Project { Id = 2, Name = "Project2", Description = "New Project2", CreationDate = DateTime.Now, UserId = "1" };
            this._projectsRepository.Insert(projects1);
            this._projectsRepository.Insert(projects2);

            

            

            
            

            

            var interested1 = new InterestedInProject
            {
                UserId = "1",
                ProjectId = 4,
                User = new ApplicationUser
                {
                    Skills = new List<UserSkill> { userSkill1}

                },
                Project = projects1,
                Confirmed = true,
                
            };

            var interested2 = new InterestedInProject
            {
                UserId = "1",
                ProjectId = 5,
                User = new ApplicationUser
                {
                    Skills = new List<UserSkill> { userSkill2},
                    Projects = new List<Project> { new Project { Id = 3, Name = "proj", UserId ="1"} },
                    InterestedInProjects = new List<InterestedInProject> { new InterestedInProject { UserId="2", Confirmed = false} }

                },
                Project = projects2,
                Confirmed = false,
            };
            this._interestedInProjectRepository.Insert(interested1);
            this._interestedInProjectRepository.Insert(interested2);

            var applicationuser1 = new ApplicationUser { Skills = new List<UserSkill> { userSkill1 }, Projects = new List<Project> { projects1 }, InterestedInProjects = new List<InterestedInProject> { interested1 } };
            var applicationuser2 = new ApplicationUser { Skills = new List<UserSkill> { userSkill2 }, Projects = new List<Project> { projects2 }, InterestedInProjects = new List<InterestedInProject> { interested2 } };
            this._applicationUserRepository.Insert(applicationuser1);
            this._applicationUserRepository.Insert(applicationuser2);


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
        public void SubmitInterestProject()
        {

            var result = this._projectsService.SubmitInterest("2", 1);
            Assert.True(result);
            

            _interestedInProjectRepository.setInsertFailure(true);
            result = _projectsService.SubmitInterest("2", 1);
            Assert.False(result);
            _interestedInProjectRepository.setThrowsException(true);
            Assert.Throws<Exception>(() => _projectsService.SubmitInterest("2", 1));
            _interestedInProjectRepository.setInsertFailure(false);
            _interestedInProjectRepository.setThrowsException(false);
            

            _interestedInProjectRepository.setSaveFailure(true);
            result = _projectsService.SubmitInterest("2", 1);
            Assert.False(result);
            _interestedInProjectRepository.setThrowsException(true);
            Assert.Throws<Exception>(() => _projectsService.SubmitInterest("2", 1));
            _interestedInProjectRepository.setSaveFailure(false);
            _interestedInProjectRepository.setThrowsException(false);







        }
        

        [Fact]
        public void CancelInterest()
        {
            var result = this._projectsService.CancelInterest("1", 4);
            Assert.True(result);
            

            _interestedInProjectRepository.setRemoveFailure(true);
            result = _projectsService.CancelInterest("1", 4);
            Assert.False(result);
            _interestedInProjectRepository.setThrowsException(true);
            Assert.Throws<Exception>(() => _projectsService.SubmitInterest("1", 4));
            _interestedInProjectRepository.setRemoveFailure(false);
            _interestedInProjectRepository.setThrowsException(false);

            _interestedInProjectRepository.setSaveFailure(true);
            result = _projectsService.CancelInterest("1", 4);
            Assert.False(result);
            _interestedInProjectRepository.setThrowsException(true);
            Assert.Throws<Exception>(() => _projectsService.SubmitInterest("1", 4));
            _interestedInProjectRepository.setSaveFailure(false);
            _interestedInProjectRepository.setThrowsException(false);




        }

        [Fact]
        public void AcceptInterestTest()
        {
            var result = this._projectsService.AcceptInterest("1", 1);
            Assert.True(result);
            _interestedInProjectRepository.setSaveFailure(true);
            result = _projectsService.AcceptInterest("1", 1);
            Assert.False(result);
            _interestedInProjectRepository.setThrowsException(true);
            Assert.Throws<Exception>(() => _projectsService.AcceptInterest("1", 1));
            _interestedInProjectRepository.setSaveFailure(false);
            _interestedInProjectRepository.setThrowsException(false);








        }

        





    }
}
