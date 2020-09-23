using ProjectsNetwork.Data;
using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsNetwork.DataAccess.Repositories
{
    public class SkillRepository : Repository<Skill>, ISkillRepository
    {
        private readonly ApplicationDbContext _db;


        public SkillRepository(ApplicationDbContext db): base(db)
        {
            this._db = db;
        }
        public void Update(Skill skill)
        {
            throw new NotImplementedException();
        }
    }
}
