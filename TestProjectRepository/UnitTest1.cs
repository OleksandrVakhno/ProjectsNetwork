using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ProjectsNetwork.Data;
using ProjectsNetwork.DataAccess.Repositories;
using ProjectsNetwork.Models;
using System;
using System.Data.Common;
using System.Linq;
using Xunit;

namespace ProjectsNetwork.Tests
{
    public class ProjectRepositoryTest : IDisposable
    {

        protected DbContextOptions<ApplicationDbContext> ContextOptions { get; }
        private readonly DbConnection _connection;
        private readonly ApplicationDbContext _context;
        private readonly ProjectRepository _projectRepository;


        public ProjectRepositoryTest()
        {

            ContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options;
            _connection = RelationalOptionsExtension.Extract(ContextOptions).Connection;
            _context = new ApplicationDbContext(ContextOptions);
            Seed();


            _projectRepository = new ProjectRepository(_context);

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


            var project1 = new Project
            {
                Id = 0,
                UserId = "1",
                Name = "project1",
                Description = "new project 1",
                CreationDate = DateTime.Now,
            };

            var project2 = new Project
            {
                Id = 2,
                UserId = "1",
                Name = "project2",
                Description = "new project 2",
                CreationDate = DateTime.Now,
            };

            _context.AddRange(project1, project2);
            _context.SaveChanges();

        }

        [Fact]
        public void InsertTest()
        {
            var inserted = this._projectRepository.Insert(new Project());
            Assert.NotNull(inserted);

        }

        [Fact]
        public void GetAllTest()
        {
            var projects = _projectRepository.GetAll();
            Assert.NotNull(projects);
            Assert.NotEmpty(projects.ToList());
        }





    }



}