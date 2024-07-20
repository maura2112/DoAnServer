using Domain.Common;
using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BidDTO 
    {
        public long Id { get; set; }
        public int ProjectId { get; set; }
        public int? UserId { get; set; }
        public string Proposal { get; set; }

        public int Duration { get; set; }

        public int Budget { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? AcceptedDate { get; set; }

        //public int? TotalOfStage { get; set; }
        //public virtual ICollection<BidStage>? BidStages { get; set; }

        public virtual ProjectDTO? Project { get; set; } = null!;
        public virtual AppUserDTO? AppUser { get; set; } = null!;

        public virtual AppUserDTO2? AppUser2 { get; set; } = null!;
    }

    public class BidAccepted
    {
        public long Id { get; set; }
    }

    public class BiddingDTO
    {
        public int ProjectId { get; set; }
        public string Proposal { get; set; }
        public int Duration { get; set; }
        public int Budget { get; set; }
    }

    public class ErrorResponseDTO
    {
        public string message { get; set; }
    }
    public class BidResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public BidDTO Data { get; set; }
    }

    public class UpdateBidDTO
    {
        public long Id { get; set; }
        public string Proposal { get; set; }
        public int Duration { get; set; }
        public int Budget { get; set; }
    }

    public class BidDTOValidator : AbstractValidator<BidDTO>
    {
        public BidDTOValidator()
        {
            RuleFor(v => v.ProjectId)
                .NotEmpty().WithMessage("Phải chọn 1 dự án để đấu thầu");
            RuleFor(v => v.Budget)
                .NotEmpty().WithMessage("Ngân sách không được để trống")
                .GreaterThan(0).WithMessage("Ngân sách phải lớn hơn 0");
            RuleFor(v => v.Duration)
                .NotEmpty().WithMessage("Ngân sách không được để trống")
                .GreaterThan(0).WithMessage("Thời lượng lớn hơn 0");
            RuleFor(v => v.Proposal).NotEmpty().WithMessage("Không được để trống");
        }
    }

    public class BiddingDTOValidator : AbstractValidator<BiddingDTO>
    {
        public BiddingDTOValidator()
        {
            RuleFor(v => v.ProjectId)
                .NotEmpty().WithMessage("Phải chọn 1 dự án để đấu thầu");
            RuleFor(v => v.Budget)
                .NotEmpty().WithMessage("Ngân sách không được để trống")
                .GreaterThan(0).WithMessage("Ngân sách phải lớn hơn 0").LessThan(2000000000)
                .WithMessage("Ngân sách phải nhỏ hơn 2B");
                
            RuleFor(v => v.Duration)
                .NotEmpty().WithMessage("Thời lượng không được để trống")
                .GreaterThan(0).WithMessage("Thời lượng lớn hơn 0")
                .LessThan(100).WithMessage("Thời lượng phải nhỏ hơn 100");
            RuleFor(v => v.Proposal)
                .NotEmpty().WithMessage("Đề xuất không được để trống");
            RuleFor(v => v.Proposal.Length)
                .LessThan(1000).WithMessage("Đề xuất không được lớn hơn 1000 kí tự");

        }
    }
    public class UpdateBidDTOValidator : AbstractValidator<UpdateBidDTO>
    {
        public UpdateBidDTOValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Không được để trống");
            RuleFor(v => v.Budget)
                .NotEmpty().WithMessage("Ngân sách không được để trống")
                .GreaterThan(0).WithMessage("Ngân sách phải lớn hơn 0").LessThan(2000000000)
                .WithMessage("Ngân sách phải nhỏ hơn 2B");

            RuleFor(v => v.Duration)
                .NotEmpty().WithMessage("Thời lượng không được để trống")
                .GreaterThan(0).WithMessage("Thời lượng lớn hơn 0")
                .LessThan(100).WithMessage("Thời lượng phải nhỏ hơn 100");
            RuleFor(v => v.Proposal)
                .NotEmpty().WithMessage("Đề xuất không được để trống");
            RuleFor(v => v.Proposal.Length)
                .LessThan(1000).WithMessage("Đề xuất không được lớn hơn 1000 kí tự");
        }
    }

}
    

