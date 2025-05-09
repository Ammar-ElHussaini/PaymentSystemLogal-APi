using FluentAssertions;
using Microsoft.Extensions.Options;
using PaymentSystem.Data_Acess_Layer.Models;

public class JwtServiceTests
{
    [Fact]
    public void GenerateJwtToken_ShouldReturnToken_WhenUserIsValid()
    {
        // Arrange
        var appSettings = Options.Create(new AppSettings
        {
            Jwt = new Jwt
            {
                Key = "hFuh893u893noi44gi4jgio489g4gkl4mg4ig4",
                Issuer = "TestIssuer",
                Audience = "TestAudience"
            }
        });

        var service = new JwtService(appSettings);

        var user = new Users { UserName = "testuser" };

        // Act
        var token = service.GenerateJwtToken(user);

        // Assert
        token.Should().NotBeNullOrEmpty();
    }
}
