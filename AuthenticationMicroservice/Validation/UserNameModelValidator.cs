using AuthenticationMicroservice.Models.Request;
using FluentValidation;

namespace AuthenticationMicroservice.Validation
{
    public class UserNameModelValidator : AbstractValidator<UserNameModel>
    {
        public UserNameModelValidator()
        {
            RuleFor(u => u.Username).Length(3, 22)
                                    .WithMessage("Username should be 3 to 22 characters");
        }
    }
}
