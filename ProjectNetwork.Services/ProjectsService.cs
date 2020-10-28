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
        private readonly IProjectSkillRepository _projectSkillRepository;
        private readonly ISkillRepository _skillRepository;

        public ProjectsService(IProjectRepository projectRepository, IProjectSkillRepository projectSkillRepository, ISkillRepository skillRepository) {

            this._projectRepository = projectRepository;
            this._projectSkillRepository = projectSkillRepository;
            this._skillRepository = skillRepository;

        }

        public IEnumerable<Project> GetAll()
        {
            return this._projectRepository.GetAll();
        }

        public Project GetProject(int Id)
        {
            return this._projectRepository.Get(Id);
        }

        public List<Skill> GetMySkills(int projectId)
        {
            var tempProject = this._projectSkillRepository.GetAll(proj => proj.ProjectId == projectId);
            List<Skill> mySkills = new List<Skill>();
            foreach (var projectSkill in tempProject)
            {
                mySkills.Add(this._skillRepository.Get(projectSkill.SkillId));
            }
            return mySkills;
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
