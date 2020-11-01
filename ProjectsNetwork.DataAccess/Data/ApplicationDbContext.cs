using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectsNetwork.DataAccess.Data.EntityConfigurations;
using ProjectsNetwork.Models;

namespace ProjectsNetwork.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration<InterestedInProject>(new InterestedInProjectConfiguration());
            modelBuilder.ApplyConfiguration<UserSkill>(new UserSkillConfiguration());
            modelBuilder.ApplyConfiguration<ProjectSkill>(new ProjectSkillConfiguration());
            modelBuilder.Entity<ApplicationUser>().Property(e => e.Id).ValueGeneratedOnAdd();
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<InterestedInProject> InterestedInProjects { get; set; }
        public DbSet<Skill> Skills { get; set; }
        

    }
}
