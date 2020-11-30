using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ProjectsNetwork.DataAccess.Repositories.MockRepositories
{
    public class MockSkillRepository : MockRepository<Skill>, ISkillRepository
    {
        public EntityEntry<Skill> Insert(Skill item)
        {
            if (this.insertFailure)
            {
                if (throwsException)
                {
                    throw this.e;
                }

                return null;
            }
            string key =  item.Id.ToString();
            this.db[key] = item;
            return (EntityEntry<Skill>)FormatterServices.GetUninitializedObject(typeof(EntityEntry<Skill>));
        }

        public void Update(Skill skill)
        {
            throw new NotImplementedException();
        }
    }
}
