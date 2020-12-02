using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ProjectsNetwork.Data;
using ProjectsNetwork.DataAccess.Repositories;
using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Xunit;

namespace ProjectsNetwork.Tests
{
    public class InterestedInProjectRepositoryTest : IDisposable
    {

        protected DbContextOptions<ApplicationDbContext> ContextOptions { get; }
        private readonly DbConnection _connection;
        private readonly ApplicationDbContext _context;
        private readonly InterestedInProjectRepository _interestedInProjectRepository;


        public InterestedInProjectRepositoryTest()
        {

            ContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options;
            _connection = RelationalOptionsExtension.Extract(ContextOptions).Connection;
            _context = new ApplicationDbContext(ContextOptions);
            Seed();


            _interestedInProjectRepository = new InterestedInProjectRepository(_context);

        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());

            connection.Open();

            return connection;
        }

        public void Dispose()
        {

            _context.Dispose();
            _connection.Dispose();

        }


        private void Seed()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var user = new ApplicationUser { Id = "1" };
            _context.Add(user);
            _context.SaveChanges();


            var interested1 = new InterestedInProject
            {

                UserId = "1",
                ProjectId = 1,
                User = new ApplicationUser
                {
                    Skills = new List<UserSkill> { },
                    Projects = new List<Project> { },
                    InterestedInProjects = new List<InterestedInProject> { }
                },

                Project = new Project
                {
                    Id = 1,
                    UserId = "1",
                    Name = "project1",
                    Description = "new project 1",
                    CreationDate = DateTime.Now,
                },
                Confirmed = false

            };

            var interested2 = new InterestedInProject
            {
                UserId = "1",
                ProjectId = 2,
                User = new ApplicationUser
                {
                    Skills = new List<UserSkill> { },
                    Projects = new List<Project> { },
                    InterestedInProjects = new List<InterestedInProject> { }
                },

                Project = new Project
                {
                    Id = 2,
                    UserId = "1",
                    Name = "project2",
                    Description = "new project 2",
                    CreationDate = DateTime.Now,
                },
                Confirmed = false

            };

            _context.AddRange(interested1, interested2);
            _context.SaveChanges();
        }

        [Fact]
        public void InsertTest()
        {
            var interested = this._interestedInProjectRepository.Insert(new InterestedInProject { UserId = "1", ProjectId = 8, Confirmed = false });
            Assert.NotNull(interested);



        }

        [Fact]
        public void GetAllTest()
        {
            var interested = _interestedInProjectRepository.GetAll();
            Assert.NotNull(interested);
            Assert.NotEmpty(interested.ToList());

        }

        [Fact]
        public void RemoveTest()
        {
            var interested = this._interestedInProjectRepository.Remove("1", 7);
            Assert.NotNull(interested);

        }

        [Fact]
        public void save()
        {
            var interested = this._interestedInProjectRepository.Insert(new InterestedInProject { UserId = "1", ProjectId = 6, Confirmed = false });
            var command = this._interestedInProjectRepository.Save();
            Assert.NotEqual(0, command);

        }

        [Fact]
        public void saveAsyncTest()
        {
            var interested = this._interestedInProjectRepository.Insert(new InterestedInProject { UserId = "1", ProjectId = 3, Confirmed = false });
            var command = this._interestedInProjectRepository.SaveAsync();
            Assert.NotNull(command);


        }
    }
}
