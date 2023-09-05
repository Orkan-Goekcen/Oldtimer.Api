using Xunit;
using FluentValidation.TestHelper;
using Oldtimer.Api.Data;
using Oldtimer.Api.Validation;
using Microsoft.AspNetCore.Routing;

namespace Oldtimer.Api.Tests
{
    public class CarValidatorTests
    {
        [Fact]
        public void Test_CarValidator_BrandMaxLength_ReturnsNoValidationErrors()
        {
            // Arrange
            var carValidator = new CarValidator();
            var car = new Car
            {
                Brand = "T",
                Model = "Corolla",
                LicensePlate = "ABC123",
                YearOfConstruction = "1990",
                Colors = Car.Color.Red,
                Sammler = new Sammler { Id = 1, Firstname = "John", Surname = "Doe", Birthdate = DateTime.Today, Email = "dasdsadsa@sadad.com", Telephone = "2132123131321" }
            };

            // Act
            var result = carValidator.TestValidate(car);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.Brand);
        }

        [Fact]
        public void Test_CarValidator_BrandMaxLength_ReturnsValidationErrors()
        {
            // Arrange
            var carValidator = new CarValidator();
            var car = new Car
            {
                Brand = "ThisBrandNameIsVeryVeryLongAndExceedsTheMaximumAllowedLength",
                Model = "Corolla",
                LicensePlate = "ABC123",
                YearOfConstruction = "1990",
                Colors = Car.Color.Red,
                Sammler = new Sammler { Id = 1, Firstname = "John", Surname = "Doe", Birthdate = DateTime.Today, Email = "dasdsadsa@sadad.com", Telephone = "2132123131321" }
            };

            // Act
            var result = carValidator.TestValidate(car);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Brand);
        }

        [Fact]
        public void Test_CarValidator_ValidCar_ReturnsNoValidationErrors()
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
                Sammler = new Sammler { Id = 1, Firstname = "John", Surname = "Doe", Birthdate = DateTime.Today, Email = "dasdsadsa@sadad.com", Telephone = "2132123131321" }
            };

            // Act
            var result = carValidator.TestValidate(car);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Test_CarValidator_AllInvalidProperties_ReturnsValidationErrors()
        {
            // Arrange
            var carValidator = new CarValidator();
            var car = new Car
            {
                Brand = "",
                Model = "",
                LicensePlate = "",
                YearOfConstruction = "",
                Colors = (Car.Color)10,
                Sammler = null
            };

            // Act
            var result = carValidator.TestValidate(car);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Brand);
            result.ShouldHaveValidationErrorFor(c => c.Model);
            result.ShouldHaveValidationErrorFor(c => c.LicensePlate);
            result.ShouldHaveValidationErrorFor(c => c.YearOfConstruction);
            result.ShouldHaveValidationErrorFor(c => c.Colors);
            result.ShouldHaveValidationErrorFor(c => c.Sammler);
        }
    }

    public class CarDtoValidatorTests
    {
        [Fact]
        public void Test_CarDtoValidator_BrandMaxLength_ReturnsNoValidationErrors()
        {
            // Arrange
            var carDtoValidator = new CarDtoValidator();
            var carDto = new CarDto
            {
                Brand = "Toyota", // Brand is within the allowed MaxLength
                Model = "Corolla",
                LicensePlate = "ABC123",
                YearOfConstruction = "1990",
                Colors = Car.Color.Red
            };

            // Act
            var result = carDtoValidator.TestValidate(carDto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.Brand);
        }

        [Fact]
        public void Test_CarDtoValidator_BrandMaxLength_ReturnsValidationErrors()
        {
            // Arrange
            var carDtoValidator = new CarDtoValidator();
            var carDto = new CarDto
            {
                Brand = "ThisBrandNameIsVeryVeryLongAndExceedsTheMaximumAllowedLength", // Brand is longer than MaxLength
                Model = "Corolla",
                LicensePlate = "ABC123",
                YearOfConstruction = "1990",
                Colors = Car.Color.Red
            };

            // Act
            var result = carDtoValidator.TestValidate(carDto);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.Brand);
        }


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

        [Fact]
        public void Test_CarDtoValidator_InvalidBrand_ReturnsValidationError()
        {
            // Arrange
            var carDtoValidator = new CarDtoValidator();
            var carDto = new CarDto { Brand = "" };

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
        public void Test_SammlerValidator_Email_ValidFormat_ReturnsNoValidationErrors()
        {
            // Arrange
            var sammlerValidator = new SammlerValidator();
            var sammler = new Sammler
            {
                Surname = "Doe",
                Firstname = "John",
                Nickname = "JD",
                Birthdate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com", // Valid email format
                Telephone = "123456"
            };

            // Act
            var result = sammlerValidator.TestValidate(sammler);

            // Assert
            result.ShouldNotHaveValidationErrorFor(s => s.Email);
        }

        [Fact]
        public void Test_SammlerValidator_Email_InvalidFormat_ReturnsValidationErrors()
        {
            // Arrange
            var sammlerValidator = new SammlerValidator();
            var sammler = new Sammler
            {
                Surname = "Doe",
                Firstname = "John",
                Nickname = "JD",
                Birthdate = new DateTime(1990, 1, 1),
                Email = "invalid_email_format", // Invalid email format
                Telephone = "123456"
            };

            // Act
            var result = sammlerValidator.TestValidate(sammler);

            // Assert
            result.ShouldHaveValidationErrorFor(s => s.Email);
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

        [Fact]
        public void Test_InvalidSammler_FailsValidation()
        {
            // Arrange
            var sammler = new Sammler
            {
                Surname = null,
                Firstname = "",
                Nickname = "Nickname_With_More_Than_25_Characters_Here",
                Birthdate = DateTime.Now.AddDays(1), // Set to a future date
                Email = "invalid_email_format",
                Telephone = "1234567890123456789012345678901234567890"
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
    }

    public class SammlerUpdateDataValidatorTests
    {
        private readonly SammlerUpdateDataValidator _sammlerUpdateDataValidator;

        public SammlerUpdateDataValidatorTests()
        {
            _sammlerUpdateDataValidator = new SammlerUpdateDataValidator();
        }

        [Fact]
        public void Test_SammlerUpdateDataValidator_Email_ValidFormat_ReturnsNoValidationErrors()
        {
            // Arrange
            var sammlerUpdateDataValidator = new SammlerUpdateDataValidator();
            var sammlerUpdateData = new SammlerUpdateData
            {
                Surname = "Doe",
                Firstname = "John",
                Nickname = "JD",
                Birthdate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com", // Valid email format
                Telephone = "123456"
            };

            // Act
            var result = sammlerUpdateDataValidator.TestValidate(sammlerUpdateData);

            // Assert
            result.ShouldNotHaveValidationErrorFor(s => s.Email);
        }

        [Fact]
        public void Test_SammlerUpdateDataValidator_Email_InvalidFormat_ReturnsValidationErrors()
        {
            // Arrange
            var sammlerUpdateDataValidator = new SammlerUpdateDataValidator();
            var sammlerUpdateData = new SammlerUpdateData
            {
                Surname = "Doe",
                Firstname = "John",
                Nickname = "JD",
                Birthdate = new DateTime(1990, 1, 1),
                Email = "invalid_email_format", // Invalid email format
                Telephone = "123456"
            };

            // Act
            var result = sammlerUpdateDataValidator.TestValidate(sammlerUpdateData);

            // Assert
            result.ShouldHaveValidationErrorFor(s => s.Email);
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

        [Fact]
        public void Test_InvalidSammlerUpdateData_FailsValidation()
        {
            // Arrange
            var sammlerUpdateData = new SammlerUpdateData
            {
                Surname = null,
                Firstname = "",
                Nickname = "Nickname_With_More_Than_25_Characters_Here",
                Birthdate = DateTime.Now.AddDays(1), // Set to a future date
                Email = "invalid_email_format",
                Telephone = "1234567890123456789012345678901234567890"
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

        public class SammlerDtoValidatorTests
        {
            private readonly SammlerDtoValidator _sammlerDtoValidator;

            public SammlerDtoValidatorTests()
            {
                _sammlerDtoValidator = new SammlerDtoValidator();
            }

            [Fact]
            public void Should_Not_Have_Error_When_Surname_Is_Valid()
            {
                var sammlerDto = new SammlerDto { Surname = "ValidSurname" };
                var result = _sammlerDtoValidator.TestValidate(sammlerDto);
                result.ShouldNotHaveValidationErrorFor(s => s.Surname);
            }

            [Fact]
            public void Should_Have_Error_When_Surname_Is_Empty()
            {
                var sammlerDto = new SammlerDto { Surname = "" };
                var result = _sammlerDtoValidator.TestValidate(sammlerDto);
                result.ShouldHaveValidationErrorFor(s => s.Surname)
                    .WithErrorMessage("Surname is required.");
            }

            [Fact]
            public void Should_Have_Error_When_Surname_Exceeds_Maximum_Length()
            {
                var sammlerDto = new SammlerDto { Surname = new string('A', 26) };
                var result = _sammlerDtoValidator.TestValidate(sammlerDto);
                result.ShouldHaveValidationErrorFor(s => s.Surname)
                    .WithErrorMessage("Surname cannot exceed 25 characters.");
            }

            [Fact]
            public void Should_Not_Have_Error_When_Firstname_Is_Valid()
            {
                var sammlerDto = new SammlerDto { Firstname = "ValidFirstname" };
                var result = _sammlerDtoValidator.TestValidate(sammlerDto);
                result.ShouldNotHaveValidationErrorFor(s => s.Firstname);
            }

            [Fact]
            public void Should_Have_Error_When_Firstname_Is_Empty()
            {
                var sammlerDto = new SammlerDto { Firstname = "" };
                var result = _sammlerDtoValidator.TestValidate(sammlerDto);
                result.ShouldHaveValidationErrorFor(s => s.Firstname)
                    .WithErrorMessage("Firstname is required.");
            }

            [Fact]
            public void Should_Have_Error_When_Firstname_Exceeds_Maximum_Length()
            {
                var sammlerDto = new SammlerDto { Firstname = new string('A', 26) };
                var result = _sammlerDtoValidator.TestValidate(sammlerDto);
                result.ShouldHaveValidationErrorFor(s => s.Firstname)
                    .WithErrorMessage("Firstname cannot exceed 25 characters.");
            }

            [Fact]
            public void Should_Not_Have_Error_When_Nickname_Is_Valid()
            {
                var sammlerDto = new SammlerDto { Nickname = "ValidNickname" };
                var result = _sammlerDtoValidator.TestValidate(sammlerDto);
                result.ShouldNotHaveValidationErrorFor(s => s.Nickname);
            }

            [Fact]
            public void Should_Have_Error_When_Nickname_Exceeds_Maximum_Length()
            {
                var sammlerDto = new SammlerDto { Nickname = new string('A', 26) };
                var result = _sammlerDtoValidator.TestValidate(sammlerDto);
                result.ShouldHaveValidationErrorFor(s => s.Nickname)
                    .WithErrorMessage("Nickname cannot exceed 25 characters.");
            }

            [Fact]
            public void Should_Not_Have_Error_When_Birthdate_Is_Valid()
            {
                var sammlerDto = new SammlerDto { Birthdate = DateTime.Now.AddYears(-30) };
                var result = _sammlerDtoValidator.TestValidate(sammlerDto);
                result.ShouldNotHaveValidationErrorFor(s => s.Birthdate);
            }

            [Fact]
            public void Should_Have_Error_When_Birthdate_Is_Empty()
            {
                var sammlerDto = new SammlerDto { Birthdate = default(DateTime) };
                var result = _sammlerDtoValidator.TestValidate(sammlerDto);
                result.ShouldHaveValidationErrorFor(s => s.Birthdate)
                    .WithErrorMessage("Birthdate is required.");
            }

            [Fact]
            public void Should_Have_Error_When_Birthdate_Is_In_The_Future()
            {
                var sammlerDto = new SammlerDto { Birthdate = DateTime.Now.AddYears(1) };
                var result = _sammlerDtoValidator.TestValidate(sammlerDto);
                result.ShouldHaveValidationErrorFor(s => s.Birthdate)
                    .WithErrorMessage("Invalid birthdate.");
            }

            [Fact]
            public void Should_Not_Have_Error_When_Email_Is_Valid()
            {
                var sammlerDto = new SammlerDto { Email = "valid.email@example.com" };
                var result = _sammlerDtoValidator.TestValidate(sammlerDto);
                result.ShouldNotHaveValidationErrorFor(s => s.Email);
            }

            [Fact]
            public void Should_Have_Error_When_Email_Is_Empty()
            {
                var sammlerDto = new SammlerDto { Email = "" };
                var result = _sammlerDtoValidator.TestValidate(sammlerDto);
                result.ShouldHaveValidationErrorFor(s => s.Email)
                    .WithErrorMessage("Email is required.");
            }

            [Fact]
            public void Should_Have_Error_When_Email_Has_Invalid_Format()
            {
                var sammlerDto = new SammlerDto { Email = "invalid-email" };
                var result = _sammlerDtoValidator.TestValidate(sammlerDto);
                result.ShouldHaveValidationErrorFor(s => s.Email)
                    .WithErrorMessage("Invalid email format.");
            }

            [Fact]
            public void Should_Not_Have_Error_When_Telephone_Is_Valid()
            {
                var sammlerDto = new SammlerDto { Telephone = "1234567890" };
                var result = _sammlerDtoValidator.TestValidate(sammlerDto);
                result.ShouldNotHaveValidationErrorFor(s => s.Telephone);
            }

            [Fact]
            public void Should_Have_Error_When_Telephone_Is_Empty()
            {
                var sammlerDto = new SammlerDto { Telephone = "" };
                var result = _sammlerDtoValidator.TestValidate(sammlerDto);
                result.ShouldHaveValidationErrorFor(s => s.Telephone)
                    .WithErrorMessage("Telephone is required.");
            }

            [Fact]
            public void Should_Have_Error_When_Telephone_Exceeds_Maximum_Length()
            {
                var sammlerDto = new SammlerDto { Telephone = new string('A', 36) };
                var result = _sammlerDtoValidator.TestValidate(sammlerDto);
                result.ShouldHaveValidationErrorFor(s => s.Telephone)
                    .WithErrorMessage("Telephone cannot exceed 35 characters.");
            }
        }
    }
}
