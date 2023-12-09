using ChatAppMobile.Pages;
using FluentValidation;

namespace ChatAppMobile.Validator
{
    internal class AddContactValidator :AbstractValidator<AddContactViewModel>
    {
        public AddContactValidator()
        {
            RuleFor((x) => x.Email).NotEmpty();
        }
    }
}