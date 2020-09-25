using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;
using System.Text;

namespace ProjectsNetwork.Models
{
    public class ProjectSkill
    {

        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        public int SkillId { get; set; }
        [ForeignKey("SkillId")]
        public Skill Skill { get; set; }
    }
}
