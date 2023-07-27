using FluentValidation;
using Oldtimer.Api.Data;

namespace Oldtimer.Api.Validation
{
    public class SammlerUpdateDataValidator : AbstractValidator<SammlerUpdateData>
    {
        public SammlerUpdateDataValidator()
        {
            RuleFor(sammler => sammler.Surname)
                .NotEmpty().WithMessage("Surname is required.")
                .MaximumLength(25).WithMessage("Surname cannot exceed 25 characters.");

            RuleFor(sammler => sammler.Firstname)
                .NotEmpty().WithMessage("Firstname is required.")
                .MaximumLength(25).WithMessage("Firstname cannot exceed 25 characters.");
 
            RuleFor(sammler => sammler.Nickname)
                .MaximumLength(25).WithMessage("Nickname cannot exceed 25 characters.");

            RuleFor(sammler => sammler.Birthdate)
                .NotEmpty().WithMessage("Birthdate is required.")
                .Must(BeValidBirthdate).WithMessage("Invalid birthdate.");

            RuleFor(sammler => sammler.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(35).WithMessage("Email cannot exceed 35 characters.");

            RuleFor(sammler => sammler.Telephone)
                .NotEmpty().WithMessage("Telephone is required.")
                .MaximumLength(35).WithMessage("Telephone cannot exceed 35 characters.");
        }

        private bool BeValidBirthdate(DateTime birthdate)
        {
            // Geburtstag in Vergangenheit
            return birthdate < DateTime.Now;
        }
    }
}
