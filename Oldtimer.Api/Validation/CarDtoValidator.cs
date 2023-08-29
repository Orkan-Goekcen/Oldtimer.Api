using FluentValidation;
using Oldtimer.Api.Data;

namespace Oldtimer.Api.Validation
{
    public class CarDtoValidator : AbstractValidator<CarDto>
    {
        public CarDtoValidator()
        {
            RuleFor(car => car.Brand)
                .NotEmpty().WithMessage("Brand is required.")
                .MaximumLength(25).WithMessage("Brand cannot exceed 25 characters.");

            RuleFor(car => car.Model)
                .NotEmpty().WithMessage("Model is required.")
                .MaximumLength(50).WithMessage("Model cannot exceed 50 characters.");

            RuleFor(car => car.LicensePlate)
                .NotEmpty().WithMessage("License plate is required.")
                .MaximumLength(15).WithMessage("License plate cannot exceed 15 characters.");

            RuleFor(car => car.YearOfConstruction)
                           .NotEmpty().WithMessage("Year of construction is required.")
                           .MaximumLength(4).WithMessage("Year of construction cannot exceed 4 characters.")
                           .Matches("^[0-9]{4}$").WithMessage("Year of construction must be a valid year in the format 'YYYY'.")
                           .Must(BeAtLeast30YearsOld).WithMessage("The car must be at least 30 years old to count as an Old Timer.");

            RuleFor(car => car.Colors)
                .IsInEnum().WithMessage("Invalid color value.");
        }

        private bool BeAtLeast30YearsOld(string yearOfConstruction)
        {
            if (int.TryParse(yearOfConstruction, out int year))
            {
                int currentYear = DateTime.Now.Year;
                return currentYear - year >= 30;
            }
            return false;
        }
    }
}
