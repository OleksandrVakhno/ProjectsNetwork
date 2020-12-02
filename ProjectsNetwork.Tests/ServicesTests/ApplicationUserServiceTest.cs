using ProjectsNetwork.Services;
using ProjectsNetwork.Models;
using ProjectsNetwork.DataAccess.Repositories.MockRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ProjectsNetwork.Tests.ServicesTests
{
    public class ApplicationUserServiceTest
    {

        private readonly ApplicationUserService _applicationUserService;
        private readonly MockApplicationUserRepository _applicationUserRepository;
        

        public ApplicationUserServiceTest()
        {

            this._applicationUserRepository = new MockApplicationUserRepository();
            this.Seed();

            this._applicationUserService = new ApplicationUserService(_applicationUserRepository);
        }

        private void Seed()
        {
            var User = new ApplicationUser { Id = "1" };
            this._applicationUserRepository.Insert(User);
         }

        [Fact]
        public void GetApplicationUserTest()
        {
            var appUser = this._applicationUserService.GetApplicationUser("1");
            Assert.NotNull(appUser);

            appUser = this._applicationUserService.GetApplicationUser("3");
            Assert.Null(appUser);
        }

        


    }
}
