using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ProjectsNetwork.DataAccess.Repositories.MockRepositories
{
    public class MockApplicationUserRepository :  MockRepository<ApplicationUser>, IApplicationUserRepository
    {
        EntityEntry<ApplicationUser> IRepository<ApplicationUser>.Insert(ApplicationUser item)
        {
            if (this.failure)
            {
                return null;
            }
            string key = item.Id;
            this.db[key] = item;
            return (EntityEntry<ApplicationUser>)FormatterServices.GetUninitializedObject(typeof(EntityEntry<ApplicationUser>));
        }

        void IApplicationUserRepository.Update(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}
