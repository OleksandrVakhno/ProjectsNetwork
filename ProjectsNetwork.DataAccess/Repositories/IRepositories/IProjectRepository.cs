using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsNetwork.DataAccess.Repositories.IRepositories
{
    public interface IProjectRepository
    {
        public void Update(Project project);
    }
}
