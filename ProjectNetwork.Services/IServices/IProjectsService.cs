using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsNetwork.Services.IServices
{
    public interface IProjectsService
    {
        IEnumerable<Project> GetAll();
        IEnumerable<Project> GetUserProjects(string userId);
        Project GetProject(int Id);
        bool PostProject(Project project, int[] skills);
        bool SubmitInterest(string userId, int projectId);

    }
}
