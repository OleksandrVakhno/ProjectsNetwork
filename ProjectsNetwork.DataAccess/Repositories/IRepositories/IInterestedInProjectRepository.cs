using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsNetwork.DataAccess.Repositories.IRepositories
{
    public interface IInterestedInProjectRepository: IRepository<InterestedInProject>
    {
        public void Update(InterestedInProject interestedInProject);
    }
}
