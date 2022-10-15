using AuthenticationMicroservice.Models.Request;
using FluentValidation;

namespace AuthenticationMicroservice.Validation
{
    public class UserNameModelValidator : AbstractValidator<UserNameModel>
    {
        public UserNameModelValidator()
        {
            RuleFor(u => u.Username).NotNull().Length(3, 16);
        }
    }
}
