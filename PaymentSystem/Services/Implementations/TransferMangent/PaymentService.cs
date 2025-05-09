
using System.Collections.Generic;
using System.Threading.Tasks;
using Data_Access_Layer.ProjectRoot.Core.Interfaces;
using PaymentSystem.Data_Acess_Layer.Models;
using PaymentSystem.Services.Interfaces.ITransferMangent;



namespace PaymentSystem.Services.Imp.TransferMangent
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsAsync()
        {
            var paymentRepository = _unitOfWork.Repository<PaymentMethod>();

            return await paymentRepository.GetAllAsync();
        }

        public async Task<PaymentMethod> GetPaymentMethodByIdAsync(int id)
        {
            var paymentRepository = _unitOfWork.Repository<PaymentMethod>();
            return await paymentRepository.GetByIdAsync(id);
        }

        public async Task AddPaymentMethodAsync(PaymentMethod paymentMethod)
        {
            var paymentRepository = _unitOfWork.Repository<PaymentMethod>();
            await paymentRepository.AddAsync(paymentMethod);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdatePaymentMethodAsync(PaymentMethod paymentMethod)
        {
            var paymentRepository = _unitOfWork.Repository<PaymentMethod>();
            paymentRepository.Update(paymentMethod);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeletePaymentMethodAsync(int id)
        {
            var paymentRepository = _unitOfWork.Repository<PaymentMethod>();
            var paymentMethod = await paymentRepository.GetByIdAsync(id);
            if (paymentMethod != null)
            {
                paymentRepository.Delete(paymentMethod);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
