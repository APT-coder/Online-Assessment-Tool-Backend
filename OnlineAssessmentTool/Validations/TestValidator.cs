using FluentValidation;
using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool.Validations
{
    public class TestValidator : AbstractValidator<Users>
        {
            public TestValidator()
            {
                RuleFor(user => user.Username)
                    .NotEmpty().WithMessage("Username is required.")
                    .Length(3, 100).WithMessage("Username must be between 3 and 100 characters.");

                RuleFor(user => user.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email format.")
                    .Length(5, 100).WithMessage("Email must be between 5 and 100 characters.");

                RuleFor(user => user.Phone)
                    .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number must be in a valid format."); 

                RuleFor(user => user.IsAdmin)
                    .NotNull().WithMessage("IsAdmin must be specified.");

                RuleFor(user => user.UUID)
                    .NotEqual(Guid.Empty).WithMessage("UUID cannot be empty.");

                RuleFor(user => user.UserType)
                    .IsInEnum().WithMessage("Invalid user type.");
            }
        }
}
