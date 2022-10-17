using AuthenticationMicroservice.Models.Request;
using FluentValidation;

namespace AuthenticationMicroservice.Validation
{
    public class UserPasswordModelValidator : AbstractValidator<UserPasswordModel>
    {
        public UserPasswordModelValidator()
        {
            RuleFor(u => u.Password).Length(8, 26)
                                    .WithMessage("Password should be 8 to 26 characters");
        }
    }
}
