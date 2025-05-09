using System.Collections.Generic;
using System.Threading.Tasks;
using Data_Access_Layer.ProjectRoot.Core.Interfaces;
using MyPaymentSystem.DTOs;
using PaymentSystem.Data_Acess_Layer.Models;
using PaymentSystem.DTOs.Helper;
using PaymentSystem.Services.Interfaces.ITransferMangent;



    public class TransferService : ITransferService
{
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITelegramBotService TelegramBotService;

        public TransferService(IUnitOfWork unitOfWork, ITelegramBotService telegramBotService)
        {
            _unitOfWork = unitOfWork;
            TelegramBotService = telegramBotService;
        }

        public async Task<IEnumerable<Transfer>> GetAllTransfersAsync()
        {
            var transferRepository = _unitOfWork.Repository<Transfer>();
            return await transferRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Transfer>> GetUserTransfersAsync(int userId)
        {
            var transferRepository = _unitOfWork.Repository<Transfer>();
        var s = await transferRepository.FindAllAsync(t => t.UserId == userId);
        return s;
        }

        public async Task<Transfer> GetTransferByIdAsync(int id)
        {
            var transferRepository = _unitOfWork.Repository<Transfer>();
            return await transferRepository.GetByIdAsync(id);
        }

        public async Task<bool> AddTransferAsync(TransferDTO dto, IFormFile transferImage, int userId)
        {
            var paymentMethod = await _unitOfWork.Repository<PaymentMethod>()
                .FindAsync(p => p.MethodName == dto.PaymentMethod);

            if (paymentMethod == null)
                return false;


            var transfer = MappingHelper.MapDtoToTransfer(dto, userId, paymentMethod.PaymentMethodId, transferImage);

            await TelegramBotService.SendTransferNotificationAsync(transfer, transferImage);

            var reviewStatus = await _unitOfWork.Repository<TransferStatus>()
                .FindAsync(s => s.StatusName == "Pinding");

            transfer.TransferStatusId = reviewStatus.TransferStatusId;

            var transferRepository = _unitOfWork.Repository<Transfer>();
            await transferRepository.AddAsync(transfer);
            await _unitOfWork.CompleteAsync();


            return true;

        }

        public async Task<bool> UpdateTransferStatusAsync(int transferId, string statusName)
        {
            var transferRepository = _unitOfWork.Repository<Transfer>();

            var transfer = await transferRepository.GetByIdAsync(transferId);
            if (transfer == null)
            {

                return false;
            }

            var status = await _unitOfWork.Repository<TransferStatus>()
                .FindAsync(s => s.StatusName.Equals(statusName, StringComparison.OrdinalIgnoreCase));

            if (status == null)
            {
                return false;
            }

            transfer.TransferStatus = status;
            transfer.TransferStatusId = status.TransferStatusId;

            transferRepository.Update(transfer);
            await _unitOfWork.CompleteAsync();

            return true;
        }

    }

