using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Application.IServices
{
    public interface IExportService
    {
        Task<string> GenerateExcelFilePath(string fileName);
    }
}
