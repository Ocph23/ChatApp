using Client.OcphAuthClient.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcphApiAuth.Client.Validators
{
    public class LoginValidators :AbstractValidator<LoginRequest>
    {
        public LoginValidators()
        {
            RuleFor(x=>x.UserName).NotEmpty();
            RuleFor(x=>x.Password).NotEmpty();
        }
    }
}
