using System;
using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;
using ProjectsNetwork.Services.IApplicationService;

namespace ProjectsNetwork.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        public ApplicationUserService(IApplicationUserRepository applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;
        }

        public ApplicationUser GetApplicationUser(string id)
        {
           var applicationUser = _applicationUserRepository.Get(id);
            return applicationUser;
        }
    }
}
