using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace ProjectsNetwork.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<UserSkill> Skills { get; set; }
        public List<Project> Projects { get; set; }
        public List<InterestedInProject> InterestedInProjects {get; set;}

    }
}
