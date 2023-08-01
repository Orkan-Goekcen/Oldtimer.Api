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
    }
    public class SammlerValidatorTests
    {
        private readonly SammlerValidator _sammlerValidator;

        public SammlerValidatorTests()
        {
            _sammlerValidator = new SammlerValidator();
        }

        [Fact]
        public void Test_ValidSammler_PassesValidation()
        {
            // Arrange
            var sammler = new Sammler
            {
                Surname = "Doe",
                Firstname = "John",
                Nickname = "JD",
                Birthdate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Telephone = "123456"
            };

            // Act
            var result = _sammlerValidator.TestValidate(sammler);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)] // Empty Surname
        [InlineData("")] // Empty Firstname
        [InlineData("2100-01-01")] // Future birthdate
        [InlineData("invalid_email_format")] // Invalid email format
        [InlineData("1234567890123456789012345678901234567890")] // Telephone exceeds 35 characters
        public void Test_InvalidSammler_FailsValidation(string invalidValue)
        {
            // Arrange
            var sammler = new Sammler
            {
                Surname = invalidValue,
                Firstname = invalidValue,
                Nickname = invalidValue,
                Birthdate = DateTime.Parse(invalidValue),
                Email = invalidValue,
                Telephone = invalidValue
            };

            // Act
            var result = _sammlerValidator.TestValidate(sammler);

            // Assert
            result.ShouldHaveValidationErrorFor(s => s.Surname);
            result.ShouldHaveValidationErrorFor(s => s.Firstname);
            result.ShouldHaveValidationErrorFor(s => s.Nickname);
            result.ShouldHaveValidationErrorFor(s => s.Birthdate);
            result.ShouldHaveValidationErrorFor(s => s.Email);
            result.ShouldHaveValidationErrorFor(s => s.Telephone);
        }

        // You can add more test cases to cover other validation rules and scenarios.
    }
    public class SammlerUpdateDataValidatorTests
    {
        private readonly SammlerUpdateDataValidator _sammlerUpdateDataValidator;

        public SammlerUpdateDataValidatorTests()
        {
            _sammlerUpdateDataValidator = new SammlerUpdateDataValidator();
        }

        [Fact]
        public void Test_ValidSammlerUpdateData_PassesValidation()
        {
            // Arrange
            var sammlerUpdateData = new SammlerUpdateData
            {
                Surname = "Doe",
                Firstname = "John",
                Nickname = "JD",
                Birthdate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Telephone = "123456"
            };

            // Act
            var result = _sammlerUpdateDataValidator.TestValidate(sammlerUpdateData);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("")] // Empty Surname
        [InlineData("")] // Empty Firstname
        [InlineData("2100-01-01")] // Future birthdate
        [InlineData("invalid_email_format")] // Invalid email format
        [InlineData("1234567890123456789012345678901234567890")] // Telephone exceeds 35 characters
        public void Test_InvalidSammlerUpdateData_FailsValidation(string invalidValue)
        {
            // Arrange
            var sammlerUpdateData = new SammlerUpdateData
            {
                Surname = invalidValue,
                Firstname = invalidValue,
                Nickname = invalidValue,
                Birthdate = DateTime.TryParse("1990-01-01", out var birthdate) ? birthdate : DateTime.MinValue,
                Email = invalidValue,
                Telephone = invalidValue
            };

            // Act
            var result = _sammlerUpdateDataValidator.TestValidate(sammlerUpdateData);

            // Assert
            result.ShouldHaveValidationErrorFor(s => s.Surname);
            result.ShouldHaveValidationErrorFor(s => s.Firstname);
            result.ShouldHaveValidationErrorFor(s => s.Nickname);
            result.ShouldHaveValidationErrorFor(s => s.Birthdate);
            result.ShouldHaveValidationErrorFor(s => s.Email);
            result.ShouldHaveValidationErrorFor(s => s.Telephone);
        }
    }
}