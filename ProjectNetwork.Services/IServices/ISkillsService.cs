using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsNetwork.Services.IServices
{
    public interface ISkillsService
    {
        IEnumerable<Skill> GetAll();
        bool PostSkills(string UserId, int[] skills);
        public List<Skill> GetMySkills(String userId);
        public Skill GetASkill(int id);
        public bool AddSkill(Skill skill);
    }

    
}
