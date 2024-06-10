using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IUserSkillRepository : IGenericRepository<UserSkill>
    {
        public Task<int> AddUserSkill(List<Skill> skills, int uid);

        public Task<int> RemoveUserSkill(int uid);
    }
}
