using ProjectsNetwork.DataAccess.Repositories.IRepositories;
using ProjectsNetwork.Models;
using ProjectsNetwork.Services.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectsNetwork.Services
{
    public class SkillsService : ISkillsService
    {


        private readonly ISkillRepository _skillRepository;
        private readonly IUserSkillRepository _userSkillRepository;

        public SkillsService(ISkillRepository skillRepository, IUserSkillRepository userSkillRepository) {
            this._skillRepository = skillRepository;
            this._userSkillRepository = userSkillRepository;
        }
        public IEnumerable<Skill> GetAll()
        {
            return this._skillRepository.GetAll();
        }

        /*public IEnumerable<UserSkill> GetMySkills(int id)
        {
            return this._userSkillRepository.Get(id);
        }*/

        public bool PostSkills(string UserId, int[] skills)
        {

            try
            {
               
                if (skills != null)
                {
                    List<UserSkill> newSkills = new List<UserSkill>();
                    foreach (int x in skills)
                    {
                        var cur = new UserSkill();
                        cur.UserId = UserId;
                        cur.SkillId = x;
                        newSkills.Add(cur);
                    }
                    foreach(UserSkill temp in newSkills)
                    {
                        if (this._userSkillRepository.Insert(temp) == null)
                        {
                            return false;
                        }

                        

                    }
                    if (this._userSkillRepository.Save() == 0)
                    {
                        return false;
                    }



                }

                

                return true;

            }
            catch (Exception e)
            {
                throw new Exception("Failed to create a new project: " + e.Message);
            }

        }
    }
}
