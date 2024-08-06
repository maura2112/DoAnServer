using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class AddressDTO
    {
        public int? UserId { get; set; }
        public string? Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        
    }
    public class AddressDTOValidator : AbstractValidator<AddressDTO>
    {
        public AddressDTOValidator()
        {
            RuleFor(address => address.City)
            .NotEmpty().WithMessage("Vui lòng chọn tên thành phố");           
            RuleFor(address => address.State)
            .NotEmpty().WithMessage("Vui lòng chọn tên quận/huyện");
        }
    }
}
