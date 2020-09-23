﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectsNetwork.Models
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SkillName { get; set; }
    }
}
