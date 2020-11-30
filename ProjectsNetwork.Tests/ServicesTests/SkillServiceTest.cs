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
            var skill = this._skillService.GetASkill(1);
            Assert.NotNull(skill);

            skill = this._skillService.GetASkill(3);
            Assert.Null(skill);
        }

        [Fact]
        public void GetMySkillsTest()
        {
            var skills = this._skillService.GetMySkills("1");
            Assert.NotEmpty(skills);

            skills = this._skillService.GetMySkills("3");
            Assert.Empty(skills);
        }


        [Fact]
        public void PostUserSkillsTest()
        {

        }




    }
}
