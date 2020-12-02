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
            if (this.insertFailure)
            {
                if (throwsException)
                {
                    throw this.e;
                }

                return null;
            }
            string key =  item.UserId.ToString() + item.ProjectId.ToString() ;
            this.db[key] = item;
            return (EntityEntry<InterestedInProject>)FormatterServices.GetUninitializedObject(typeof(EntityEntry<InterestedInProject>));
        }

        public void Update(InterestedInProject item)
        {
            var key = (item.UserId, item.ProjectId).ToString();
            this.db[key].Confirmed = item.Confirmed;
            




            
            
            
                

            
            
        }
    }
}
