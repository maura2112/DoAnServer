using Application.DTOs;
using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IProjectSkillService 
    {
        Task<int> Add(ProjectSkillDTO request);
        Task<Pagination<ProjectSkillDTO>> Get(int pageIndex, int pageSize);

        Task<Pagination<ProjectSkillDTO>> GetWithFilter(Expression<Func<ProjectSkill, bool>> filter, int pageIndex, int pageSize);
    }
}
