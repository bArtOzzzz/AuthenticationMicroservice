using AuthenticationMicroservice.Models.Request;
using FluentValidation;

namespace AuthenticationMicroservice.Validation
{
    public class UserPasswordModelValidator : AbstractValidator<UserPasswordModel>
    {
        public UserPasswordModelValidator()
        {
            RuleFor(u => u.Password).NotNull().Length(8, 26);
        }
    }
}
