using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;
using ProjectsNetwork.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ProjectsNetwork.Services
{
    public class ProjectsService : IProjectsService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IInterestedInProjectRepository _interestedInProjectRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;

        public ProjectsService(IProjectRepository projectRepository, IInterestedInProjectRepository interestedInProjectRepository, IApplicationUserRepository applicationUserRepository) {

            this._projectRepository = projectRepository;
            this._interestedInProjectRepository = interestedInProjectRepository;
            this._applicationUserRepository = applicationUserRepository;

        }

        public IEnumerable<Project> GetAll()
        {
            return this._projectRepository.GetAll();
        }

        public Project GetProject(int Id)
        {
            var project = this._projectRepository.Get(Id);
            if (project!=null)
            {
                project.UsersInterested = this._interestedInProjectRepository.GetAll(interestedIn => interestedIn.ProjectId == Id).ToList();
            }

            return project;
        }

        public IEnumerable<Project> GetUserProjects(string userId)
        {
            return this._projectRepository.GetAll(proj => proj.UserId == userId);
        }

        public bool PostProject(Project project, int[] skills)
        {

            try
            {
                
                if (skills != null)
                {
                    List<ProjectSkill> prefferedSkills = new List<ProjectSkill>();
                    foreach (int x in skills)
                    {
                        var cur = new ProjectSkill();
                        cur.ProjectId = project.Id;
                        cur.SkillId = x;
                        prefferedSkills.Add(cur);
                    }
                    project.PrefferedSkills = prefferedSkills;
                }

                if (this._projectRepository.Insert(project) == null)
                {
                    return false;
                }

                if (this._projectRepository.Save() == 0)
                {
                    return false;
                }

                return true;

            }catch(Exception e)
            {
                throw new Exception("Failed to create a new project: " + e.Message);
            }
           
        }


        public bool SubmitInterest(string userId, int projectId)
        {
            try
            {
                var project = this._projectRepository.Get(projectId);
                var interestedIn = this._interestedInProjectRepository.Get(userId, projectId);
                //user should not be interested in own project
                if (project.UserId == userId || interestedIn!=null)
                {
                    return false;
                }

                
                
                var interestedInProject = new InterestedInProject
                {
                    ProjectId = projectId,
                    UserId = userId,
                    Confirmed = false
                };

                var inserted = this._interestedInProjectRepository.Insert(interestedInProject);
                if ( inserted == null)
                {
                    return false;
                }

                if (this._interestedInProjectRepository.Save() == 0)
                {
                    return false;
                }

                return true;
            }
            catch(Exception e)
            {
                throw new Exception("Failed to submit the interest: " + e.Message);
            }
        }
    }
}
