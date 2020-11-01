using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;
using ProjectsNetwork.Services.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsNetwork.Services
{
    public class SkillsService : ISkillsService
    {


        private readonly ISkillRepository _skillRepository;
        private readonly IUserSkillRepository _userSkillRepository;
        private readonly IProjectSkillRepository _projectSkillRepository;

        public SkillsService(ISkillRepository skillRepository, IProjectSkillRepository projectSkillRepository, IUserSkillRepository userSkillRepository) {
            this._skillRepository = skillRepository;
            this._projectSkillRepository = projectSkillRepository;
            this._userSkillRepository = userSkillRepository;
        }
        public IEnumerable<Skill> GetAll()
        {
            return this._skillRepository.GetAll();
        }
        public Skill GetASkill(int id)
        {
            return this._skillRepository.Get(id);
        }

        public List<Skill> GetMySkills(String userId)
        {
            var tempSkils = this._userSkillRepository.GetAll(skills => skills.UserId == userId);
            List<Skill> mySkills = new List<Skill>();
            foreach (var userSkill in tempSkils)
            {
                mySkills.Add(GetASkill(userSkill.SkillId));
            }
            return mySkills;
        }

        
        public bool PostUserSkills(string UserId, int[] skills)
        {

            try
            {
               
                if (skills != null)
                {
                    List<UserSkill> newSkills = new List<UserSkill>();
                    foreach (int x in skills)
                    {
                        var cur = new UserSkill();
                        cur.UserId = UserId;
                        cur.SkillId = x;
                        newSkills.Add(cur);
                    }
                    foreach(UserSkill temp in newSkills)
                    {
                        if (this._userSkillRepository.Insert(temp) == null)
                        {
                            return false;
                        }

                        

                    }
                    if (this._userSkillRepository.Save() == 0)
                    {
                        return false;
                    }



                }

                

                return true;

            }
            catch (Exception e)
            {
                throw new Exception("Failed to create a new project: " + e.Message);
            }

        }

        public bool AddSkill(Skill skill)
        {
            if (skill != null)
            {
                if (this._skillRepository.Insert(skill) == null)
                {
                    return false;
                }

                if (this._skillRepository.Save() == 0)
                {
                    return false;
                }

                return true;
            }
            return false;
        }

        public List<Skill> GetProjectSkills(int projectId)
        {
            var tempProject = this._projectSkillRepository.GetAll(proj => proj.ProjectId == projectId);
            List<Skill> projectSkills = new List<Skill>();
            foreach (var projectSkill in tempProject)
            {
                projectSkills.Add(this._skillRepository.Get(projectSkill.SkillId));
            }
            return projectSkills;
        }
    }
}
