using FluentValidation;
using static ChatAppMobile.Pages.LoginPage;

namespace ChatAppMobile.Validator
{
    public class LoginValidator :AbstractValidator<LoginViewModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserName).EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
