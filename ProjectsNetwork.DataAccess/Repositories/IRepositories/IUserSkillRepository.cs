using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsNetwork.DataAccess.Repositories.IRepositories
{
    public interface IUserSkillRepository : IRepository<UserSkill>
    {
        public void Update(UserSkill userSkill);

    }
}

