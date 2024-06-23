using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReportCategoryRepository : GenericRepository<ReportCategory>, IReportCategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public ReportCategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
