using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface ISkillRepository : IGenericRepository<Skill>
    {
        public  Task<Skill> GetByNameAsync(string skillName);

        public Task<List<Skill>> GetSkillByUser(int UserId);

        public Task<List<Skill>> SaveSkillsToOtherCategory (List<Skill> skills);
        
    }
}
