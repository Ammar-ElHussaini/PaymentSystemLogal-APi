using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FakeItEasy;
using PaymentSystem.Data_Acess_Layer.Models;
using Data_Access_Layer.ProjectRoot.Core.Interfaces;
using PaymentSystem.Services.Imp.TransferMangent;

namespace PaymentSystem.Tests
{
    public class PaymentServiceTests
    {
        [Fact]
        public async Task GetAllPaymentMethodsAsync_ShouldReturnAllPaymentMethods()
        {
            // Arrange
            var fakeUnitOfWork = A.Fake<IUnitOfWork>();
            var fakePaymentRepo = A.Fake<IGenericRepository<PaymentMethod>>();

            A.CallTo(() => fakeUnitOfWork.Repository<PaymentMethod>()).Returns(fakePaymentRepo);

            var paymentMethods = new List<PaymentMethod>
            {
                new PaymentMethod { PaymentMethodId = 1, MethodName = "Credit Card", Description = "Credit Card Payment" },
                new PaymentMethod { PaymentMethodId = 2, MethodName = "PayPal", Description = "PayPal Payment" }
            };

            A.CallTo(() => fakePaymentRepo.GetAllAsync()).Returns(Task.FromResult((IEnumerable<PaymentMethod>)paymentMethods));

            var service = new PaymentService(fakeUnitOfWork);

            // Act
            var result = await service.GetAllPaymentMethodsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, pm => pm.MethodName == "Credit Card");
            Assert.Contains(result, pm => pm.MethodName == "PayPal");
        }

        [Fact]
        public async Task GetPaymentMethodByIdAsync_ShouldReturnPaymentMethod_WhenExists()
        {
            // Arrange
            var fakeUnitOfWork = A.Fake<IUnitOfWork>();
            var fakePaymentRepo = A.Fake<IGenericRepository<PaymentMethod>>();

            A.CallTo(() => fakeUnitOfWork.Repository<PaymentMethod>()).Returns(fakePaymentRepo);

            var paymentMethod = new PaymentMethod { PaymentMethodId = 1, MethodName = "Credit Card", Description = "Credit Card Payment" };
            A.CallTo(() => fakePaymentRepo.GetByIdAsync(1)).Returns(Task.FromResult(paymentMethod));

            var service = new PaymentService(fakeUnitOfWork);

            // Act
            var result = await service.GetPaymentMethodByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Credit Card", result.MethodName);
        }

        [Fact]
        public async Task AddPaymentMethodAsync_ShouldAddPaymentMethod()
        {
            // Arrange
            var fakeUnitOfWork = A.Fake<IUnitOfWork>();
            var fakePaymentRepo = A.Fake<IGenericRepository<PaymentMethod>>();

            A.CallTo(() => fakeUnitOfWork.Repository<PaymentMethod>()).Returns(fakePaymentRepo);
            A.CallTo(() => fakePaymentRepo.AddAsync(A<PaymentMethod>._)).Returns(Task.CompletedTask);
            A.CallTo(() => fakeUnitOfWork.CompleteAsync());

            var service = new PaymentService(fakeUnitOfWork);
            var paymentMethod = new PaymentMethod { PaymentMethodId = 3, MethodName = "Bitcoin", Description = "Cryptocurrency Payment" };

            // Act
            await service.AddPaymentMethodAsync(paymentMethod);

            // Assert
            A.CallTo(() => fakePaymentRepo.AddAsync(paymentMethod)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeUnitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task UpdatePaymentMethodAsync_ShouldUpdatePaymentMethod()
        {
            // Arrange
            var fakeUnitOfWork = A.Fake<IUnitOfWork>();
            var fakePaymentRepo = A.Fake<IGenericRepository<PaymentMethod>>();

            A.CallTo(() => fakeUnitOfWork.Repository<PaymentMethod>()).Returns(fakePaymentRepo);
            A.CallTo(() => fakePaymentRepo.Update(A<PaymentMethod>._)).DoesNothing();
            A.CallTo(() => fakeUnitOfWork.CompleteAsync());

            var service = new PaymentService(fakeUnitOfWork);
            var paymentMethod = new PaymentMethod { PaymentMethodId = 1, MethodName = "Credit Card", Description = "Updated Description" };

            // Act
            await service.UpdatePaymentMethodAsync(paymentMethod);

            // Assert
            A.CallTo(() => fakePaymentRepo.Update(paymentMethod)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeUnitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task DeletePaymentMethodAsync_ShouldDeletePaymentMethod_WhenExists()
        {
            // Arrange
            var fakeUnitOfWork = A.Fake<IUnitOfWork>();
            var fakePaymentRepo = A.Fake<IGenericRepository<PaymentMethod>>();

            A.CallTo(() => fakeUnitOfWork.Repository<PaymentMethod>()).Returns(fakePaymentRepo);
            A.CallTo(() => fakePaymentRepo.GetByIdAsync(1)).Returns(Task.FromResult(new PaymentMethod { PaymentMethodId = 1, MethodName = "Credit Card" }));
            A.CallTo(() => fakePaymentRepo.Delete(A<PaymentMethod>._)).DoesNothing();
            A.CallTo(() => fakeUnitOfWork.CompleteAsync());

            var service = new PaymentService(fakeUnitOfWork);

            // Act
            await service.DeletePaymentMethodAsync(1);

            // Assert
            A.CallTo(() => fakePaymentRepo.Delete(A<PaymentMethod>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => fakeUnitOfWork.CompleteAsync()).MustHaveHappenedOnceExactly();
        }
    }
}
