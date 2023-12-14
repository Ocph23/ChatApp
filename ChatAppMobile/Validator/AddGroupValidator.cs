using ChatAppMobile.Pages;
using FluentValidation;

namespace ChatAppMobile.Validator
{
    internal class AddGroupValidator : AbstractValidator<AddGroupViewModel>
    {
        public AddGroupValidator()
        {
            RuleFor((x) => x.GroupName).NotEmpty();
            RuleFor((x) => x.Description).NotEmpty();
        }
    }
}