using AuthenticationMicroservice.Models.Request;
using FluentValidation;

namespace AuthenticationMicroservice.Validation
{
    public class RoleModelValidator : AbstractValidator<RoleModel>
    {
        public RoleModelValidator()
        {
            RuleFor(r => r.Role).Length(5, 18)
                                .WithMessage("Length should be 5 to 18 characters");
        }
    }
}
