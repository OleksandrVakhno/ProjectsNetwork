using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ProjectsNetwork.DataAccess.Repositories.MockRepositories
{
    public class MockProjectSkillRepository : MockRepository<ProjectSkill>, IProjectSkillRepository
    {
        public EntityEntry<ProjectSkill> Insert(ProjectSkill item)
        {
            if (this.failure)
            {
                return null;
            }
            string key =  item.ProjectId.ToString() + item.SkillId.ToString() ;
            this.db[key] = item;
            return (EntityEntry<ProjectSkill>)FormatterServices.GetUninitializedObject(typeof(EntityEntry<ProjectSkill>));
        }

        public void Update(ProjectSkill projectSkill)
        {
            throw new NotImplementedException();
        }
    }
}
