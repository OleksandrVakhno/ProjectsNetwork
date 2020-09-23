using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsNetwork.DataAccess.Repositories.IRepositories
{
    public interface ISkillRepository: IRepository<Skill>
    {
        public void Update(Skill skill);

    }
}
