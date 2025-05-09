using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;
using PaymentSystem.Data_Acess_Layer.Models;
using Data_Access_Layer.ProjectRoot.Core.Interfaces;
using Microsoft.Extensions.Options;

public class TelegramBotService : ITelegramBotService
{
    private readonly TelegramBotClient _botClient;
    private readonly TransferService _transferService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly TelegramSettings _telegramSettings;

   
    public TelegramBotService(
        TelegramBotClient botClient,
        IOptions<TelegramSettings> telegramSettings)
    {
        _botClient = botClient;
        _telegramSettings = telegramSettings.Value;
    }



    public async Task<string> SendTransferNotificationAsync( Transfer transfer, IFormFile transferImage)
    {
        // Get chat ID from the database
        var chat = await _unitOfWork.Repository<TelegramChat>()
                                     .GetByIdAsync(1); // or based on user, etc.

        if (chat == null || string.IsNullOrWhiteSpace(chat.ChatId))
            throw new Exception("Chat ID not found in the database.");

        var message = $"🧾 New Transfer:\n" +
                      $"👤 Name: {transfer.User}\n" +
                      $"📞 Phone: {transfer.Phone}\n" +
                      $"🔢 Transfer ID: {transfer.TransferId}";

        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData("✅ Approve", $"completed_{transfer.TransferId}") },
            new[] { InlineKeyboardButton.WithCallbackData("❌ Reject", $"rejected_{transfer.TransferId}") }
        });

        using var stream = transferImage.OpenReadStream();
        var inputFile = new InputFileStream(stream, transferImage.FileName);

        await _botClient.SendPhoto(
            chatId: _telegramSettings.ChatId,
            photo: inputFile,
            caption: message,
            replyMarkup: inlineKeyboard
        );

        return message;
    }
    public async Task<bool> HandleCallbackQueryAsync([FromBody] Update update)
    {
        if (update.CallbackQuery != null)
        {
            var callbackData = update.CallbackQuery.Data;

            var parts = callbackData.Split('_');
            if (parts.Length == 2)
            {
                var action = parts[0]; // "completed" or "rejected"
                var transferIdStr = parts[1];

                if (int.TryParse(transferIdStr, out int transferId))
                {
                    string statusName = action == "completed" ? "Success" : "Failed";
                    var result = await _transferService.UpdateTransferStatusAsync(transferId, statusName);

                    string responseMessage = result
                        ? $"✅ Transfer #{transferId} marked as {statusName}."
                        : $"🚫 Transfer not found or update failed.";

                    await _botClient.AnswerCallbackQuery(update.CallbackQuery.Id);
                    await _botClient.SendMessage(
                        chatId: update.CallbackQuery.Message.Chat.Id,
                        text: responseMessage
                    );
                }
            }
        }

        return true;
    }
}