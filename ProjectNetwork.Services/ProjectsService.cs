using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;
using ProjectsNetwork.Services.IServices;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ProjectsNetwork.Services
{
    public class ProjectsService : IProjectsService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectsService(IProjectRepository projectRepository) {

            this._projectRepository = projectRepository;

        }

        public IEnumerable<Project> GetAll()
        {
            return this._projectRepository.GetAll();
        }

        public Project GetProject(int Id)
        {
            return this._projectRepository.Get(Id);
        }

        public bool PostProject(string UserId, Project project, int[] skills)
        {

            try
            {
                project.CreationDate = DateTime.Now;


                project.UserId = UserId;


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
    }
}
