using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsNetwork.Services.IServices
{
    public interface IProjectsService
    {
        IEnumerable<Project> GetAll();
        Project GetProject(int Id);
        bool PostProject(Project project, int[] skills);
        bool SubmitInterest(string userId, int projectId);
    }
}
