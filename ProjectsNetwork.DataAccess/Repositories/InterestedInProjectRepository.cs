using ProjectsNetwork.Data;
using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsNetwork.DataAccess.Repositories
{
    public class InterestedInProjectRepository : Repository<InterestedInProject>, IInterestedInProjectRepository
    {
        private readonly ApplicationDbContext _db;

        public InterestedInProjectRepository(ApplicationDbContext db): base(db)
        {
            this._db = db;
        }
        public void Update(InterestedInProject interestedInProject)
        {
            throw new NotImplementedException();
        }
    }
}
