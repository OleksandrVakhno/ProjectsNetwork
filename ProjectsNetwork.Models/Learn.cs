using System;
using System.Collections.Generic;

namespace ProjectsNetwork.Models
{
    public class Learn
    {
        public Learn(Project project, List<Skill> skills, bool accepted, bool request)
        {
            Project = project;
            Skills = skills;
            Accepted = accepted;
            Request = request;
        }

        public Project Project { get; set; }
        public List<Skill> Skills { get; set; }
        public bool Request { get; set; }
        public bool Accepted { get; set; }
    }
}
