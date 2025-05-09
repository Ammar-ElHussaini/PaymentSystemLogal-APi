
using PaymentSystem.Data_Acess_Layer.Models;

public interface ITelegramBotService
{
    Task<string> SendTransferNotificationAsync(Transfer transfer, IFormFile transferImage);
}