using ProjectsNetwork.Data;
using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsNetwork.DataAccess.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {

        private readonly ApplicationDbContext _db;


        public ApplicationUserRepository(ApplicationDbContext db): base(db)
        {
            this._db = db;
        }

        public void Update(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}
