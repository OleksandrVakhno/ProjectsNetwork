using System;
using System.Collections.Generic;

namespace ProjectsNetwork.Models
{
    public class Learn
    {
        public Learn(Project project, List<Skill> skills, bool request)
        {
            Project = project;
            Skills = skills;
            Request = request;
        }

        public Project Project { get; set; }
        public List<Skill> Skills { get; set; }
        public bool Request { get; set; }
    }
}
