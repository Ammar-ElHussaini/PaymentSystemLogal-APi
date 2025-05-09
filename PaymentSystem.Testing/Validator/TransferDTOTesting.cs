using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;
using MyPaymentSystem.DTOs;

public class TransferDTOTesting

{
    [Fact]
    public void TransferDTO_ValidData_ShouldHaveNoValidationErrors()
    {
        // Arrange
        var transferDto = new TransferDTO
        {
            TransferId = 1,
            SenderPhoneNumber = "01234567890",
            ReceiverPhoneNumber = "09876543210",
            Amount = 100.50m,
            TransferDate = "2025-05-09",
            Status = "Success",
            PaymentMethod = "Vodafone Cash"
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(transferDto, null, null);
        var isValid = Validator.TryValidateObject(transferDto, validationContext, validationResults, true);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void TransferDTO_InvalidAmount_ShouldHaveValidationError()
    {
        // Arrange
        var transferDto = new TransferDTO
        {
            TransferId = 1,
            SenderPhoneNumber = "01234567890",
            ReceiverPhoneNumber = "09876543210",
            Amount = 0m, // Invalid, should be greater than zero
            TransferDate = "2025-05-09",
            Status = "Pending",
            PaymentMethod = "Vodafone Cash"
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(transferDto, null, null);
        var isValid = Validator.TryValidateObject(transferDto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.ErrorMessage == "Amount must be greater than zero.");
    }

    [Fact]
    public void TransferDTO_MissingSenderPhoneNumber_ShouldHaveValidationError()
    {
        // Arrange
        var transferDto = new TransferDTO
        {
            TransferId = 1,
            SenderPhoneNumber = "", // Invalid, required field
            ReceiverPhoneNumber = "09876543210",
            Amount = 100.50m,
            TransferDate = "2025-05-09",
            Status = "Pending",
            PaymentMethod = "Vodafone Cash"
        };

        // Act
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(transferDto, null, null);
        var isValid = Validator.TryValidateObject(transferDto, validationContext, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.ErrorMessage == "Sender phone number is required.");
    }
}
