using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;
using PaymentSystem.DTOs;

public class LoginDtoTesting
{
    [Fact]
    public void LoginDto_ValidData_ShouldHaveNoValidationErrors()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Username = "validUsername",
            Password = "validPassword123"
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(loginDto, null, null);
        var isValid = Validator.TryValidateObject(loginDto, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void LoginDto_InvalidUsername_ShouldHaveValidationError()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Username = "", // Invalid
            Password = "validPassword123"
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(loginDto, null, null);
        var isValid = Validator.TryValidateObject(loginDto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.ErrorMessage == "Username is required.");
    }

    [Fact]
    public void LoginDto_InvalidPassword_ShouldHaveValidationError()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Username = "validUsername",
            Password = "short" // Invalid, less than 6 characters
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(loginDto, null, null);
        var isValid = Validator.TryValidateObject(loginDto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.ErrorMessage == "Password must be at least 6 characters long.");
    }
}
