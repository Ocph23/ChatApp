using ChatAppMobile.Pages;
using FluentValidation;

namespace ChatAppMobile.Validator
{
    public class ProfileValidator : AbstractValidator<ProfileViewModel>
    {
        public ProfileValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Telepon).NotEmpty();
        }
    }
}
