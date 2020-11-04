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
            var oldInterest = this._db.InterestedInProjects.Find(interestedInProject.UserId, interestedInProject.ProjectId);
            if (oldInterest == null)
            {
                throw new Exception("Interested in project is not found");
            }

            oldInterest.Confirmed = interestedInProject.Confirmed;
        }
    }
}
