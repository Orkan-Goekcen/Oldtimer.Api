using Xunit;
using FluentValidation.TestHelper;
using Oldtimer.Api.Data;
using Oldtimer.Api.Validation;

namespace Oldtimer.Api.Tests
{
    public class CarValidatorTests
    {
        [Fact]
        public void Test_CarValidator_ValidCar_ReturnsNoValidationError()
        {
            // Arrange
            var carValidator = new CarValidator();
            var car = new Car
            {
                Brand = "Toyota",
                Model = "Corolla",
                LicensePlate = "ABC123",
                YearOfConstruction = "1990",
                Colors = Car.Color.Red,
                Sammler = new Sammler { Id = 1, Firstname = "John", Surname = "Doe" }
            };

            // Act
            var result = carValidator.TestValidate(car);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.Brand);
            result.ShouldNotHaveValidationErrorFor(c => c.Model);
            result.ShouldNotHaveValidationErrorFor(c => c.LicensePlate);
            result.ShouldNotHaveValidationErrorFor(c => c.YearOfConstruction);
            result.ShouldNotHaveValidationErrorFor(c => c.Colors);
            result.ShouldNotHaveValidationErrorFor(c => c.Sammler);
        }

        [Theory]
        [InlineData("")] // Empty brand
        public void Test_CarValidator_InvalidBrand_ReturnsValidationError(string brand)
        {
            // Arrange
            var carValidator = new CarValidator();
            var car = new Car { Brand = brand };

            // Act
            var result = carValidator.TestValidate(car);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Brand);
        }

        // Other test methods can be similarly added to cover the remaining rules of CarValidator.
    }

    public class CarDtoValidatorTests
    {
        [Fact]
        public void Test_CarDtoValidator_ValidCarDto_ReturnsNoValidationError()
        {
            // Arrange
            var carDtoValidator = new CarDtoValidator();
            var carDto = new CarDto
            {
                Brand = "Toyota",
                Model = "Corolla",
                LicensePlate = "ABC123",
                YearOfConstruction = "1990",
                Colors = Car.Color.Red
            };

            // Act
            var result = carDtoValidator.TestValidate(carDto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.Brand);
            result.ShouldNotHaveValidationErrorFor(c => c.Model);
            result.ShouldNotHaveValidationErrorFor(c => c.LicensePlate);
            result.ShouldNotHaveValidationErrorFor(c => c.YearOfConstruction);
            result.ShouldNotHaveValidationErrorFor(c => c.Colors);
        }

        [Theory]
        [InlineData("")] // Empty brand
        public void Test_CarDtoValidator_InvalidBrand_ReturnsValidationError(string brand)
        {
            // Arrange
            var carDtoValidator = new CarDtoValidator();
            var carDto = new CarDto { Brand = brand };

            // Act
            var result = carDtoValidator.TestValidate(carDto);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Brand);
        }

        // Other test methods can be similarly added to cover the remaining rules of CarDtoValidator.
    }
}
