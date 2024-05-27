using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserSkillRepository : GenericRepository<UserSkill>, IUserSkillRepository
    {
        private readonly ApplicationDbContext _context;

        public UserSkillRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> AddUserSkill(List<Skill> skills, int uid)
        {
            var listUserSkills = new List<UserSkill>();
            foreach (var skill in skills)
            {
                var userSkill = new UserSkill()
                {
                    SkillId = skill.Id,
                    UserId = uid
                };
                listUserSkills.Add(userSkill);  
            }
            await _context.UserSkills.AddRangeAsync(listUserSkills);
            return await _context.SaveChangesAsync();
        }
    }
}
