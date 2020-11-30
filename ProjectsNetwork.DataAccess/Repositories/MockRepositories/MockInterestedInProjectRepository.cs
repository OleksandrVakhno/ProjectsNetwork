using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ProjectsNetwork.DataAccess.Repositories.MockRepositories
{
    public class MockInterestedInProjectRepository : MockRepository<InterestedInProject>, IInterestedInProjectRepository
    {
        public EntityEntry<InterestedInProject> Insert(InterestedInProject item)
        {
            if (this.failure)
            {
                return null;
            }
            string key =  item.UserId.ToString() + item.ProjectId.ToString() ;
            this.db[key] = item;
            return (EntityEntry<InterestedInProject>)FormatterServices.GetUninitializedObject(typeof(EntityEntry<InterestedInProject>));
        }

        public void Update(InterestedInProject interestedInProject)
        {
            throw new NotImplementedException();
        }
    }
}
