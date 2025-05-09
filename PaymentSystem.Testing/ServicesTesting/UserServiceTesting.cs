using Xunit;
using FakeItEasy;
using FluentAssertions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PaymentSystem.DTOs;
using PaymentSystem.Data_Acess_Layer.Models;
using Data_Access_Layer.ProjectRoot.Core.Interfaces;
using PaymentSystem.Services.Interfaces;
using PaymentSystem.Services.Imp;

namespace PaymentSystem.Testing.ServicesTesting
{
    public class UserServiceTests
    {
        [Fact]
        public async Task RegisterAsync_ShouldReturnToken_WhenUserIsNew()
        {
            // Arrange
            var fakeUnitOfWork = A.Fake<IUnitOfWork>();
            var fakeUserRepo = A.Fake<IGenericRepository<Users>>();
            var fakeJwtService = A.Fake<IJwtService>();
            var fakeConfig = A.Fake<IConfiguration>();

            var registerDto = new RegisterDto
            {
                Username = "newuser",
                Email = "new@example.com",
                PhoneNumber = "0123456789",
                Password = "password"
            };

            A.CallTo(() => fakeUnitOfWork.Repository<Users>())
                .Returns(fakeUserRepo);

            A.CallTo(() => fakeUserRepo.FindAsync(A<System.Linq.Expressions.Expression<Func<Users, bool>>>._))
                .Returns(Task.FromResult<Users?>(null));

            A.CallTo(() => fakeJwtService.GenerateJwtToken(A<Users>._))
                .Returns("fake-token");

            var service = new UserService(fakeUnitOfWork, fakeConfig, fakeJwtService);

            // Act
            var result = await service.RegisterAsync(registerDto);

            // Assert
            result.Should().Be("fake-token");
            A.CallTo(() => fakeUserRepo.AddAsync(A<Users>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeUnitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnNull_WhenUserAlreadyExists()
        {
            // Arrange
            var fakeUnitOfWork = A.Fake<IUnitOfWork>();
            var fakeUserRepo = A.Fake<IGenericRepository<Users>>();
            var fakeJwtService = A.Fake<IJwtService>();
            var fakeConfig = A.Fake<IConfiguration>();

            var registerDto = new RegisterDto
            {
                Username = "existinguser",
                Email = "exist@example.com",
                PhoneNumber = "0111111111",
                Password = "pass"
            };

            A.CallTo(() => fakeUnitOfWork.Repository<Users>())
                .Returns(fakeUserRepo);

            A.CallTo(() => fakeUserRepo.FindAsync(A<System.Linq.Expressions.Expression<Func<Users, bool>>>._))
                .Returns(Task.FromResult(new Users()));

            var service = new UserService(fakeUnitOfWork, fakeConfig, fakeJwtService);

            // Act
            var result = await service.RegisterAsync(registerDto);

            // Assert
            result.Should().BeNull();
            A.CallTo(() => fakeUserRepo.AddAsync(A<Users>._)).MustNotHaveHappened();
            A.CallTo(() => fakeUnitOfWork.CompleteAsync()).MustNotHaveHappened();
        }


        [Fact]
        public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var fakeUnitOfWork = A.Fake<IUnitOfWork>();
            var fakeUserRepo = A.Fake<IGenericRepository<Users>>();
            var fakeJwtService = A.Fake<IJwtService>();
            var fakeConfig = A.Fake<IConfiguration>();

            var loginDto = new LoginDto
            {
                Username = "testuser",
                Password = "1234"
            };

            var fakeUser = new Users
            {
                UserName = "testuser",
                PasswordHash = "1234"
            };

            A.CallTo(() => fakeUnitOfWork.Repository<Users>())
                .Returns(fakeUserRepo);

            A.CallTo(() => fakeUserRepo.FindAsync(A<System.Linq.Expressions.Expression<Func<Users, bool>>>._))
                .Returns(Task.FromResult(fakeUser));

            A.CallTo(() => fakeJwtService.GenerateJwtToken(fakeUser))
                .Returns("valid-jwt-token");

            var service = new UserService(fakeUnitOfWork, fakeConfig, fakeJwtService);

            // Act
            var result = await service.LoginAsync(loginDto);

            // Assert
            result.Should().Be("valid-jwt-token");
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            var fakeUnitOfWork = A.Fake<IUnitOfWork>();
            var fakeUserRepo = A.Fake<IGenericRepository<Users>>();
            var fakeJwtService = A.Fake<IJwtService>();
            var fakeConfig = A.Fake<IConfiguration>();

            var loginDto = new LoginDto
            {
                Username = "nouser",
                Password = "whatever"
            };

            A.CallTo(() => fakeUnitOfWork.Repository<Users>())
                .Returns(fakeUserRepo);

            A.CallTo(() => fakeUserRepo.FindAsync(A<System.Linq.Expressions.Expression<Func<Users, bool>>>._))
                .Returns(Task.FromResult<Users?>(null)); 

            var service = new UserService(fakeUnitOfWork, fakeConfig, fakeJwtService);

            // Act
            var result = await service.LoginAsync(loginDto);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenPasswordIsIncorrect()
        {
            // Arrange
            var fakeUnitOfWork = A.Fake<IUnitOfWork>();
            var fakeUserRepo = A.Fake<IGenericRepository<Users>>();
            var fakeJwtService = A.Fake<IJwtService>();
            var fakeConfig = A.Fake<IConfiguration>();

            var loginDto = new LoginDto
            {
                Username = "testuser",
                Password = "wrongpass"
            };

            var fakeUser = new Users
            {
                UserName = "testuser",
                PasswordHash = "correctpass"
            };

            A.CallTo(() => fakeUnitOfWork.Repository<Users>())
                .Returns(fakeUserRepo);

            A.CallTo(() => fakeUserRepo.FindAsync(A<System.Linq.Expressions.Expression<Func<Users, bool>>>._))
                .Returns(Task.FromResult(fakeUser));

            var service = new UserService(fakeUnitOfWork, fakeConfig, fakeJwtService);

            // Act
            var result = await service.LoginAsync(loginDto);

            // Assert
            result.Should().BeNull();
        }



    }
}
