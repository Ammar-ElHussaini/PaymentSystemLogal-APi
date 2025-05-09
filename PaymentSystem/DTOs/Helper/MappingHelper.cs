using PaymentSystem.Data_Acess_Layer.Models;
using PaymentSystem.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using MyPaymentSystem.DTOs;

namespace PaymentSystem.DTOs.Helper
{
    public static class MappingHelper
    {
        public static Transfer MapDtoToTransfer(TransferDTO dto, int userId, int paymentMethodId, IFormFile transferImage)
        {
            return new Transfer
            {
                UserId = userId,
                PaymentMethodId = paymentMethodId,
                Phone = int.Parse(dto.SenderPhoneNumber),
                Amount = dto.Amount,
                TransferDate = DateTime.UtcNow
            };
        }

        public static TransferDTO MapTransferToDto(Transfer transfer)
        {
            return new TransferDTO
            {
                TransferId = transfer.TransferId,
                SenderPhoneNumber = transfer.Phone.ToString(),
                ReceiverPhoneNumber = "",
                Amount = transfer.Amount,
                TransferDate = transfer.TransferDate.ToString("yyyy-MM-dd HH:mm"),
                Status = transfer.TransferStatus?.StatusName,
                PaymentMethod = transfer.PaymentMethod?.MethodName
            };
        }

        public static Users MapRegisterDtoToUser(RegisterDto dto)
        {
            return new Users
            {
                UserName = dto.Username,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = dto.Password,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Roles = "User"
            };
        }
    }
}
