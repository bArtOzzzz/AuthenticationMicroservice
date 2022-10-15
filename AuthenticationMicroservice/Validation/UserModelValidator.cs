using AuthenticationMicroservice.Models.Request;
using FluentValidation;

namespace AuthenticationMicroservice.Validation
{
    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
            RuleFor(u => u.Username).NotNull()
                                    .Length(3, 22);

            RuleFor(u => u.EmailAddress).NotEmpty()
                                        .EmailAddress();

            RuleFor(u => u.Password).NotNull()
                                    .Length(8, 26);

            RuleFor(u => u.Password).Equal(u => u.PasswordRepeated);
        } 
    }
}
