using ProjectsNetwork.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsNetwork.Services.IServices
{
    public interface ISkillsService
    {
        IEnumerable<Skill> GetAll();
    }
}
