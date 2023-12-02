using Client.OcphAuthClient.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcphApiAuth.Client.Validators
{
    public class RegisterValidators :AbstractValidator<RegisterRequest>
    {
        public RegisterValidators()
        {
            RuleFor(x=>x.Email).NotEmpty().WithMessage("{PropertyName} Tidak Boleh Kosong"); 
            RuleFor(x=>x.Password).NotEmpty().WithMessage("{PropertyName} Tidak Boleh Kosong");
            RuleFor(x=>x.ConfirmPassword).Equal(x=>x.Password).WithMessage("Password Tidak Sama !");
        }
    }
}
