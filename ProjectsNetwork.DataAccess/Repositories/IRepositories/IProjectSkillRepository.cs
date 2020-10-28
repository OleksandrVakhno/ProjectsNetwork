using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsNetwork.DataAccess.Repositories.IRepositories
{
    public interface IProjectSkillRepository : IRepository<ProjectSkill>
    {
        public void Update(ProjectSkill projectSkill);

    }
}
