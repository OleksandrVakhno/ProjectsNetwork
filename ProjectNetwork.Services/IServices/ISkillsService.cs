using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsNetwork.Services.IServices
{
    public interface ISkillsService
    {
        IEnumerable<Skill> GetAll();
        bool PostUserSkills(string UserId, int[] skills);
        public List<Skill> GetMySkills(String userId);
        public List<Skill> GetProjectSkills(int projectId);
        public Skill GetASkill(int id);
        public bool AddSkill(Skill skill);
    }

    
}
