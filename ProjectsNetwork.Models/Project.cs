using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectsNetwork.Models
{
    public class Project
    {


        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; } 

        [Required]
        public DateTime CreationDate { get; set; }


        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public List<Skill> PrefferedSkills { get; set; }
        public List<InterestedInProject> UsersInterested { get; set; }

    }
}
