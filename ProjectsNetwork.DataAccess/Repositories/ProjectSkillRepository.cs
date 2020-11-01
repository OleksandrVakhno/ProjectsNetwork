using ProjectsNetwork.Data;
using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsNetwork.DataAccess.Repositories
{
    public class ProjectSkillRepository : Repository<ProjectSkill>, IProjectSkillRepository
    {
        private readonly ApplicationDbContext _db;


        public ProjectSkillRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }
        public void Update(ProjectSkill projectSkill)
        {
            throw new NotImplementedException();
        }
    }
}