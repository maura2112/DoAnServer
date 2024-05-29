using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IProjectSkillRepository : IGenericRepository<ProjectSkill>
    {
        public Task<int> AddProjectSkill(List<Skill> skills, int pId);
    }
}
