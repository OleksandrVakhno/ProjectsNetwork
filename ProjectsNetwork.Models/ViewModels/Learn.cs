using System;
using System.Collections.Generic;

namespace ProjectsNetwork.Models
{
    public class Learn
    {
        public Learn(Project project, List<Skill> projectSkills, List<Skill> allSkills, bool accepted, bool request)
        {
            Project = project;
            ProjectSkills = projectSkills;
            AllSkills = allSkills;
            Accepted = accepted;
            Request = request;
        }

        public Project Project { get; set; }
        public List<Skill> ProjectSkills { get; set; }
        public List<Skill> AllSkills { get; set; }
        public bool Request { get; set; }
        public bool Accepted { get; set; }
    }
}
