using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentSystem.Data_Acess_Layer.Models;

namespace PaymentSystem.Services.Interfaces.ITransferMangent
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsAsync();
        Task<PaymentMethod> GetPaymentMethodByIdAsync(int id);
        Task AddPaymentMethodAsync(PaymentMethod paymentMethod);
        Task UpdatePaymentMethodAsync(PaymentMethod paymentMethod);
        Task DeletePaymentMethodAsync(int id);
    }
}
