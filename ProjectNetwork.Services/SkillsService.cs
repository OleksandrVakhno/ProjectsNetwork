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

        public SkillsService(ISkillRepository skillRepository) {
            this._skillRepository = skillRepository;
        }
        public IEnumerable<Skill> GetAll()
        {
            return this._skillRepository.GetAll();
        }
    }
}
