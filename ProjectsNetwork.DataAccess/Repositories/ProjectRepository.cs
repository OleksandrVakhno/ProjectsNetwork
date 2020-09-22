﻿using ProjectsNetwork.Data;
using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsNetwork.DataAccess.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {

        public readonly ApplicationDbContext _db;


        public ProjectRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }
        public void Update(Project project)
        {
            var oldProject = this._db.Projects.Find(project.Id);
            if (oldProject == null)
            {
                throw new KeyNotFoundException("Project with this ID was not found");
            }

            oldProject.Name = project.Name;
            oldProject.Description = project.Description;
            oldProject.Skills = project.Skills;
        }
    }
}