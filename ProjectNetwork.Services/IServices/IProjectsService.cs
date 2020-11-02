using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ProjectsNetwork.Services.IServices
{
    public interface IProjectsService
    {
        IEnumerable<Project> GetAll(Expression<Func<Project, bool>> filter = null);
        IEnumerable<Project> GetUserProjects(string userId);
        IEnumerable<ApplicationUser> GetInterested(int projectId); //users interested in my project
        IEnumerable<Project> GetAcceptedProjects(string userId); //my projects that I accepted interest to
        IEnumerable<Project> GetMatches(string userId); // other people's projects that confirmed my interest
        Project GetProject(int Id);
        bool PostProject(Project project, int[] skills);
        bool SubmitInterest(string userId, int projectId);
        bool AcceptInterest(string userId, int projectId);

    }
}
