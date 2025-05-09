using Xunit;
using FakeItEasy;
using PaymentSystem.Services;
using PaymentSystem.Data_Acess_Layer.Models;
using PaymentSystem.DTOs;
using Data_Access_Layer.ProjectRoot.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyPaymentSystem.DTOs;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace PaymentSystem.Tests
{
    public class TransferServiceTests
    {

        [Fact]
        public async Task AddTransferAsync_ShouldReturnTrue_WhenTransferAdded()
        {
            // Arrange
            var fakeUnitOfWork = A.Fake<IUnitOfWork>();
            var fakeTransferRepo = A.Fake<IGenericRepository<Transfer>>();
            var fakeTelegramService = A.Fake<ITelegramBotService>();

            A.CallTo(() => fakeUnitOfWork.Repository<Transfer>()).Returns(fakeTransferRepo);
            A.CallTo(() => fakeTransferRepo.AddAsync(A<Transfer>._)).Returns(Task.CompletedTask);
            A.CallTo(() => fakeUnitOfWork.CompleteAsync()).Returns(1);

            var service = new TransferService(fakeUnitOfWork, fakeTelegramService);

            var formFile = A.Fake<IFormFile>(); 
            var transfer = new Transfer { UserId = 123, Amount = 100, Phone = 66 };
            var transfer2 = new TransferDTO { PaymentMethod = "123", Amount = 100, SenderPhoneNumber = "66" };

            // Act
            var result = await service.AddTransferAsync(transfer2, formFile, 6);

            // Assert
            Assert.True(result);
            A.CallTo(() => fakeTransferRepo.AddAsync(A<Transfer>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdateTransferStatusAsync_ShouldReturnTrue_WhenStatusUpdated()
        {
            // Arrange
            var fakeUnitOfWork = A.Fake<IUnitOfWork>();
            var fakeTransferRepo = A.Fake<IGenericRepository<Transfer>>();
            var fakeStatusRepo = A.Fake<IGenericRepository<TransferStatus>>();
            var fakeTelegramBotService = A.Fake<ITelegramBotService>();

            var transfer = new Transfer { TransferId = 1, TransferStatusId = 1 };
            var status = new TransferStatus { TransferStatusId = 2, StatusName = "Completed" };

            A.CallTo(() => fakeUnitOfWork.Repository<Transfer>()).Returns(fakeTransferRepo);
            A.CallTo(() => fakeUnitOfWork.Repository<TransferStatus>()).Returns(fakeStatusRepo);
            A.CallTo(() => fakeTransferRepo.GetByIdAsync(1)).Returns(transfer);
            A.CallTo(() => fakeStatusRepo.FindAsync(A<System.Linq.Expressions.Expression<System.Func<TransferStatus, bool>>>._)).Returns(status);
            A.CallTo(() => fakeUnitOfWork.CompleteAsync()).Returns(1);

            var service = new TransferService(fakeUnitOfWork, fakeTelegramBotService);

            // Act
            var result = await service.UpdateTransferStatusAsync(1, "Completed");

            // Assert
            Assert.True(result);
            A.CallTo(() => fakeTransferRepo.Update(A<Transfer>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeUnitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdateTransferStatusAsync_ShouldReturnFalse_WhenTransferNotFound()
        {
            // Arrange
            var fakeUnitOfWork = A.Fake<IUnitOfWork>();
            var fakeTransferRepo = A.Fake<IGenericRepository<Transfer>>();
            var fakeTelegramBotService = A.Fake<ITelegramBotService>();

            A.CallTo(() => fakeUnitOfWork.Repository<Transfer>()).Returns(fakeTransferRepo);
            A.CallTo(() => fakeTransferRepo.GetByIdAsync(1)).Returns(Task.FromResult<Transfer>(null));

            var service = new TransferService(fakeUnitOfWork, fakeTelegramBotService);

            // Act
            var result = await service.UpdateTransferStatusAsync(1, "Completed");

            // Assert
            Assert.False(result);
            A.CallTo(() => fakeUnitOfWork.CompleteAsync()).MustNotHaveHappened();
        }

    }
}
