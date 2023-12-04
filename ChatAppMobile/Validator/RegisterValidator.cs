using ChatAppMobile.Pages;
using FluentValidation;

namespace ChatAppMobile.Validator
{
    public class RegisterValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.UserName).EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.ConfirmPassword).NotEmpty()
                .Equal(x => x.Password)
                .When(x => !string.IsNullOrEmpty(x.Password));
        }
    }
}
