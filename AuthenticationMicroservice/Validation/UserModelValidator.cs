using AuthenticationMicroservice.Models.Request;
using FluentValidation;

namespace AuthenticationMicroservice.Validation
{
    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(u => u.Username).Length(3, 22)
                                    .WithMessage("Username should be 3 to 22 characters");

            RuleFor(u => u.EmailAddress).EmailAddress()
                                        .WithMessage("Incorrect email or type");

            RuleFor(u => u.Password).Length(8, 26)
                                    .WithMessage("Password should be 8 to 26 characters");

            RuleFor(u => u.Password).Equal(u => u.PasswordRepeated)
                                    .WithMessage("Passwords must match");
        } 
    }
}
