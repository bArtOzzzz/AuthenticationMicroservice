using AuthenticationMicroservice.Models.Request;
using FluentValidation;

namespace AuthenticationMicroservice.Validation
{
    public class UserEmailModelValidator : AbstractValidator<UserEmailModel>
    {
        public UserEmailModelValidator()
        {
            RuleFor(u => u.EmailAddress).EmailAddress()
                                        .WithMessage("Incorrect email or type");
        }
    }
}
