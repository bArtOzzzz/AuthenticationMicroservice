using AuthenticationMicroservice.Models.Request;
using FluentValidation;

namespace AuthenticationMicroservice.Validation
{
    public class UserLoginModelValidator : AbstractValidator<UserLoginModel>
    {
        public UserLoginModelValidator()
        {
            RuleFor(u => u.Username).NotNull().Length(3, 16);
            RuleFor(u => u.Password).NotNull().Length(8, 26);
        }
    }
}
