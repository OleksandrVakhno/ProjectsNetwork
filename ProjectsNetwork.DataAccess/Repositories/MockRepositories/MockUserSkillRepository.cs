using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ProjectsNetwork.DataAccess.Repositories.MockRepositories
{
    public class MockUserSkillRepository : MockRepository<UserSkill>, IUserSkillRepository
    {
        public EntityEntry<UserSkill> Insert(UserSkill item)
        {
            if (this.failure)
            {
                return null;
            }
            string key = item.UserId.ToString() + item.SkillId.ToString() ;
            this.db[key] = item;
            return (EntityEntry<UserSkill>)FormatterServices.GetUninitializedObject(typeof(EntityEntry<UserSkill>));
        }

        public void Update(UserSkill userSkill)
        {
            throw new NotImplementedException();
        }
    }
}
