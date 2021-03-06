﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        public EntityEntry<ApplicationUser> Insert(ApplicationUser item)
        {
            if (this.insertFailure)
            {
                if (throwsException)
                {
                    throw this.e;
                }

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
