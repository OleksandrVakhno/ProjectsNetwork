using ProjectsNetwork.Data;
using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsNetwork.DataAccess.Repositories
{
    public class UserSkillRepository : Repository<UserSkill>, IUserSkillRepository
    {
        private readonly ApplicationDbContext _db;


        public UserSkillRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }
        public void Update(UserSkill userSkill)
        {
            throw new NotImplementedException();
        }
    }
}

