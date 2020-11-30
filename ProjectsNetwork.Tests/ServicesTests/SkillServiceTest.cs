using ProjectsNetwork.Services;
using ProjectsNetwork.Models;
using ProjectsNetwork.DataAccess.Repositories.MockRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ProjectsNetwork.Tests.ServicesTests
{
    public class SkillServiceTest
    {

        private readonly SkillsService _skillService;
        private readonly MockSkillRepository _skillRepository;
        private readonly MockProjectSkillRepository _projectSkillRepository;
        private readonly MockUserSkillRepository _userSkillRepository;


        public SkillServiceTest()
        {

            this._skillRepository = new MockSkillRepository();
            this._projectSkillRepository = new MockProjectSkillRepository();
            this._userSkillRepository = new MockUserSkillRepository();
            this.Seed();

            this._skillService = new SkillsService(_skillRepository, _projectSkillRepository, _userSkillRepository);
        }

        private void Seed()
        {

            var projectSkill1 = new ProjectSkill { ProjectId = 1, SkillId = 1 };
            var projectSkill2 = new ProjectSkill { ProjectId = 2, SkillId = 2 };
            this._projectSkillRepository.Insert(projectSkill1);
            this._projectSkillRepository.Insert(projectSkill2);


            var userSkill1 = new UserSkill { UserId = "1", SkillId = 1 };
            var userSkill2 = new UserSkill { UserId = "2", SkillId = 2 };
            this._userSkillRepository.Insert(userSkill1);
            this._userSkillRepository.Insert(userSkill2);


            this._skillRepository.Insert(new Skill { Id = 1, SkillName = "Java", Users = new List<UserSkill> { userSkill1 } });
            this._skillRepository.Insert(new Skill { Id = 2, SkillName = "C++", Projects = new List<ProjectSkill> { projectSkill1 } });

        }

        [Fact]
        public void GetAllTest()
        {
            var skills = this._skillService.GetAll();
            Assert.NotEmpty(skills);

        }

        [Fact]
        public void GetASkillTest()
        {
            //skills returned
            var skill = this._skillService.GetASkill(1);
            Assert.NotNull(skill);

            //no skills exist
            skill = this._skillService.GetASkill(3);
            Assert.Null(skill);
        }

        [Fact]
        public void GetMySkillsTest()
        {
            //skills returned
            var skills = this._skillService.GetMySkills("1");
            Assert.NotEmpty(skills);

            //no skills exist
            skills = this._skillService.GetMySkills("3");
            Assert.Empty(skills);
        }


        [Fact]
        public void PostUserSkillsTest()
        {
            //failure to insert
            _userSkillRepository.setInsertFailure(true);
            var result = _skillService.PostUserSkills("1", new int[] { 0, 1 });
            Assert.False(result);
            _userSkillRepository.setThrowsException(true);
            Assert.Throws<Exception>(() => _skillService.PostUserSkills("1", new int[] { 0, 1 })); //testing exception while inserting
            _userSkillRepository.setInsertFailure(false);
            _userSkillRepository.setThrowsException(false);


            //failure to save
            _userSkillRepository.setSaveFailure(true);
            result = _skillService.PostUserSkills("1", new int[] { 0, 1 });
            Assert.False(result);
            _userSkillRepository.setThrowsException(true);
            Assert.Throws<Exception>(() => _skillService.PostUserSkills("1", new int[] { 0, 1 })); //testing exception while saving
            _userSkillRepository.setSaveFailure(false);
            _userSkillRepository.setThrowsException(false);


            //successful execution
            result = _skillService.PostUserSkills("1", new int[] { 0, 1 });
            Assert.True(result);

        }


        [Fact]
        public void AddSkillTest()
        {
            //failure to insert
            _skillRepository.setInsertFailure(true);
            var result = _skillService.AddSkill(new Skill { Id = 1, SkillName = "C#" });
            Assert.False(result);
            _skillRepository.setThrowsException(true);
            Assert.Throws<Exception>(() => _skillService.AddSkill(new Skill { Id = 1, SkillName = "C#" }));
            _skillRepository.setInsertFailure(false);
            _skillRepository.setThrowsException(false);


            //failure to save
            _skillRepository.setSaveFailure(true);
            result = _skillService.AddSkill(new Skill { Id = 1, SkillName = "C#" });
            Assert.False(result);
            _skillRepository.setThrowsException(true);
            Assert.Throws<Exception>(() => _skillService.AddSkill(new Skill { Id = 1, SkillName = "C#" }));
            _skillRepository.setSaveFailure(false);
            _skillRepository.setThrowsException(false);

            //successful execution
            result = _skillService.AddSkill(new Skill { Id = 1, SkillName = "C#" });
            Assert.True(result);

        }


        [Fact]
        public void GetProjectSkillsTest()
        {
            //skills returned
            var skills = this._skillService.GetProjectSkills(1);
            Assert.NotEmpty(skills);

            //no skills exist
            skills = this._skillService.GetProjectSkills(3);
            Assert.Empty(skills);

        }


    }
}
