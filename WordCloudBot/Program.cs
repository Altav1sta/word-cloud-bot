using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


Console.WriteLine("Hello, World!");

var apiToken = "1311883738:AAFKd1sflKRV4glSHrXj4qGl6DlAoal8Keo";
var botClient = new TelegramBotClient(apiToken);
var me = await botClient.GetMeAsync();

using var cts = new CancellationTokenSource();

botClient.StartReceiving(
    new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync),
    cts.Token);

Console.WriteLine($"Start listening for @{me.Username}...");
Console.ReadLine();

cts.Cancel();


Console.WriteLine("Bye, World!");




async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Type != UpdateType.Message)
        return;
    if (update.Message.Type != MessageType.Text)
        return;

    var chatId = update.Message.Chat.Id;

    await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: update.Message.Text
    );
}

Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",

        _ => $"Exception: {exception.GetType}\n{exception.Message}"
    };

    Console.WriteLine(ErrorMessage);

    return Task.CompletedTask;
}