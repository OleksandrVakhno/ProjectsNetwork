using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ProjectsNetwork.Services.IApplicationService
{
    public interface IApplicationUserService{
        public ApplicationUser GetApplicationUser(string userId);
    }
}
