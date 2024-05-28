using Application.DTOs;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface ISkillService
    {
        Task<List<Skill>> AddSkillForUser(List<string> skillNames, int uid);

        Task<Pagination<SkillDTO>> Get(int pageIndex, int pageSize);

        Task<Pagination<SkillDTO>> GetWithFilter(Expression<Func<Skill, bool>> filter, int pageIndex, int pageSize);
        Task<int> Add(SkillDTO request);
    }
}
