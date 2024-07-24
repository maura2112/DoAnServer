using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Common;
using FluentValidation;
using MimeKit.Encodings;

namespace Application.DTOs
{
    public class ProjectStatusDTO :BaseEntity
    {
        public string StatusName { get; set; }
        public string? StatusColor { get; set; }
    }

    public class ProjectStatusUpdate
    {
        public int StatusId { get; set; }
        public int ProjectId { get; set; }
        public int? BidId { get; set; }
        public string? RejectReason { get; set; }
    }

    public class ProjectStatusUpdateValidator : AbstractValidator<ProjectStatusUpdate>
    {
        public ProjectStatusUpdateValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotNull().WithMessage("Dự án không được để trống");

            RuleFor(x => x.StatusId)
                .NotNull().WithMessage("Trạng thái không được để trống");

            RuleFor(x => x.RejectReason)
                .MaximumLength(200).WithMessage("Không quá 200 kí tự")
                .NotEmpty().WithMessage("Lí do từ chối không được lỗi")
                .When(x => x.ProjectId == 5);
        }
    }
}
