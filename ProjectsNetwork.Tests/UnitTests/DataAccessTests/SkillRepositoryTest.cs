﻿using Microsoft.Data.Sqlite;
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
    public class SkillRepositoryTest : IDisposable
    {

        protected DbContextOptions<ApplicationDbContext> ContextOptions { get; }
        private readonly DbConnection _connection;
        private readonly ApplicationDbContext _context;
        private readonly SkillRepository _skillRepository;


        public SkillRepositoryTest()
        {

            ContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options;
            _connection = RelationalOptionsExtension.Extract(ContextOptions).Connection;
            _context = new ApplicationDbContext(ContextOptions);
            Seed();


            _skillRepository = new SkillRepository(_context);

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



            var skill1 = new Skill
            {
                Id = 3,
                SkillName = "Machine Learning",
                Users = new List<UserSkill> { },
                Projects = new List<ProjectSkill> { }

            };

            var skill2 = new Skill
            {

                Id = 4,
                SkillName = "Data Analytics",
                Users = new List<UserSkill> { },
                Projects = new List<ProjectSkill> { }

            };

            _context.AddRange(skill1, skill2);
            _context.SaveChanges();
        }

        [Fact]
        public void InsertTest()
        {
            var skills = this._skillRepository.Insert(new Skill() { Id = 5, SkillName = "CyberSecurity" });
            Assert.NotNull(skills);

            skills = this._skillRepository.Insert(new Skill());
            Assert.NotNull(skills);


        }

        [Fact]
        public void GetAllTest()
        {
            var skills = this._skillRepository.GetAll();
            Assert.NotNull(skills);
            Assert.NotEmpty(skills.ToList());

        }

        [Fact]
        public void RemoveTest()
        {
            var skills = this._skillRepository.Remove(4);
            Assert.NotNull(skills);
        }

        [Fact]
        public void save()
        {
            var skills = this._skillRepository.Remove(4);
            var command = this._skillRepository.Save();
            Assert.NotEqual(0, command);



        }

        [Fact]
        public void saveAsyncTest()
        {
            var skills = this._skillRepository.Remove(3);
            var command = this._skillRepository.SaveAsync();
            Assert.NotNull(command);

        }
    }
}
