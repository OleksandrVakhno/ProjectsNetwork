using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ProjectsNetwork.DataAccess.Repositories.MockRepositories
{
    public class MockProjectRepository : MockRepository<Project>, IProjectRepository
    {
        public EntityEntry<Project> Insert(Project item)
        {
            if (this.insertFailure)
            {
                if (throwsException)
                {
                    throw this.e;
                }

                return null;
            }
            string key =  item.Id.ToString() ;
            this.db[key] = item;
            return (EntityEntry<Project>)FormatterServices.GetUninitializedObject(typeof(EntityEntry<Project>));
        }

        public void Update(Project project)
        {
            throw new NotImplementedException();
        }
    }
}
