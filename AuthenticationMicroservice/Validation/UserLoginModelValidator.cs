using AuthenticationMicroservice.Models.Request;
using FluentValidation;

namespace AuthenticationMicroservice.Validation
{
    public class UserLoginModelValidator : AbstractValidator<UserLoginModel>
    {
        public UserLoginModelValidator()
        {
            RuleFor(u => u.Username).Length(3, 22)
                                    .WithMessage("Username should be 3 to 22 characters");
            
            RuleFor(u => u.Password).Length(8, 26)
                                    .WithMessage("Password should be 8 to 26 characters");
        }
    }
}
