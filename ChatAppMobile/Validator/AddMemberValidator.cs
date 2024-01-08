using ChatAppMobile.Pages;
using FluentValidation;

namespace ChatAppMobile.Validator
{
    internal class AddMemberValidator : AbstractValidator<AddGroupMemberViewModel>
    {
        public AddMemberValidator()
        {
            RuleFor((x) => x.Email).NotEmpty();
        }
    }
}