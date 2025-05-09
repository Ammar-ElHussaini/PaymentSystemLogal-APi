using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;
using PaymentSystem.DTOs;

public class RegisterDtoTesting
{
    [Fact]
    public void RegisterDto_ValidData_ShouldHaveNoValidationErrors()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Username = "validUsername",
            Email = "test@example.com",
            PhoneNumber = "01234567890",
            Password = "validPassword123"
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(registerDto, null, null);
        var isValid = Validator.TryValidateObject(registerDto, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void RegisterDto_InvalidEmail_ShouldHaveValidationError()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Username = "validUsername",
            Email = "invalidEmail", // Invalid email
            PhoneNumber = "01234567890",
            Password = "validPassword123"
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(registerDto, null, null);
        var isValid = Validator.TryValidateObject(registerDto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.ErrorMessage == "Invalid email format.");
    }

    [Fact]
    public void RegisterDto_InvalidPhoneNumber_ShouldHaveValidationError()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Username = "validUsername",
            Email = "test@example.com",
            PhoneNumber = "12345", // Invalid phone number
            Password = "validPassword123"
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(registerDto, null, null);
        var isValid = Validator.TryValidateObject(registerDto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.ErrorMessage == "Phone number must be exactly 11 digits.");
    }
}
