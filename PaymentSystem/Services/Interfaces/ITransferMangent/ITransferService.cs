namespace PaymentSystem.Services.Interfaces.ITransferMangent
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PaymentSystem.Data_Acess_Layer.Models;
    using MyPaymentSystem.DTOs;
    using PaymentSystem.DTOs.Helper;
    using Microsoft.AspNetCore.Http;



    public interface ITransferService
    {
        Task<IEnumerable<Transfer>> GetAllTransfersAsync();
        Task<IEnumerable<Transfer>> GetUserTransfersAsync(int userId);
        Task<Transfer> GetTransferByIdAsync(int id);
        Task<bool> AddTransferAsync(TransferDTO dto, IFormFile transferImage, int userId);
        Task<bool> UpdateTransferStatusAsync(int transferId, string statusName);
    }


}
