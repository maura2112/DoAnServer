using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProjectSkillRepository : GenericRepository<ProjectSkill>, IProjectSkillRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectSkillRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> AddProjectSkill(List<Skill> skills, int pId)
        {
            var listProjectSkills = new List<ProjectSkill>();
            foreach (var skill in skills)
            {
                var projectSkill = new ProjectSkill()
                {
                    SkillId = skill.Id,
                    ProjectId = pId,
                };
                listProjectSkills.Add(projectSkill);
            }
            await _context.ProjectSkills.AddRangeAsync(listProjectSkills);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Skill>> GetListProjectSkillByProjectId(int projectId)
        {
            var listProjectSkills = await _context.ProjectSkills.Where(x=>x.ProjectId == projectId).ToListAsync();
            var listSkills = new List<Skill>();
            foreach (var skill in listProjectSkills)
            {
                var skillEnt = _context.Skills.Where(x => x.Id == skill.SkillId);
                listSkills.Add((Skill)skillEnt);
            }
            return listSkills;
        }
    }
}
