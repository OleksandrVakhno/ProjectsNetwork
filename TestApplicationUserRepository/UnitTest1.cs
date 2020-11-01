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
    public class ApplicationUserRepositoryTest : IDisposable
    {

        protected DbContextOptions<ApplicationDbContext> ContextOptions { get; }
        private readonly DbConnection _connection;
        private readonly ApplicationDbContext _context;
        private readonly ApplicationUserRepository _applicationUserRepository;


        public ApplicationUserRepositoryTest()
        {

            ContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options;
            _connection = RelationalOptionsExtension.Extract(ContextOptions).Connection;
            _context = new ApplicationDbContext(ContextOptions);
            Seed();


            _applicationUserRepository = new ApplicationUserRepository(_context);

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


            var applicationUser1 = new ApplicationUser
            {
                Skills = new List<UserSkill> { },
                Projects = new List<Project> { },
                InterestedInProjects = new List<InterestedInProject> { }


            };

            var applicationUser2 = new ApplicationUser
            {
                Skills = new List<UserSkill> { },
                Projects = new List<Project> { },
                InterestedInProjects = new List<InterestedInProject> { }


            };

            _context.AddRange(applicationUser1, applicationUser2);
            _context.SaveChanges();

        }

        [Fact]
        public void InsertTest()
        {
            var inserted = this._applicationUserRepository.Insert(new ApplicationUser());
            Assert.NotNull(inserted);

        }

        [Fact]
        public void GetAllTest()
        {
            var applicationUser = _applicationUserRepository.GetAll();
            Assert.NotNull(applicationUser);
            Assert.NotEmpty(applicationUser.ToList());
        }


    }



}

