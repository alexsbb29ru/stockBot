using BaseTypes;
using Init.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using YahooFinanceApi;

namespace StockBot
{
    /// <summary>
    /// This controller is main place. 
    /// </summary>
    public class IndexController : BaseController
    {
        private IInitSettings _initSetting;
        private IExchangeService _exchangeService;

        private ITelegramBotClient _botClient;
        private User _me;
        public IndexController(IInitSettings initSetting, 
            IExchangeService exchangeService)
        {
            _initSetting = initSetting;
            _exchangeService = exchangeService;
        }

        public async Task Index()
        {
            await BotConfig();
        }
        /// <summary>
        /// Bot configuration
        /// </summary>
        private async Task BotConfig()
        {
            var botToken = _initSetting.GetToken(nameof(BotConfig));
            _botClient = new TelegramBotClient(botToken);

            _me = await _botClient.GetMeAsync();
            _logger.Information($"Запуск бота");

            _botClient.OnMessage += Bot_OnMessage;
            _botClient.OnCallbackQuery += Bot_OnCallbackQuery;

            _botClient.StartReceiving(Array.Empty<UpdateType>());

            _logger.Information($"Start listening for @{_me.Username}");
            Console.ReadLine();

            _botClient.StopReceiving();
        }

        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if(e.Message.Text != null)
            {
                var chat = e.Message.Chat;
                var msg = e.Message.Text;
                var answer = _exchangeService.GetEvaluation(msg);

                _logger.Information($"В чат @{_me.Username} пользователем {chat.Username} " +
                    $"было отправлено сообщение: {msg}. Ответ: {answer}");

                await _botClient.SendTextMessageAsync(
                    chatId: chat,
                    text: answer,
                    replyMarkup: new InlineKeyboardMarkup(new[] { InlineKeyboardButton.WithCallbackData("Get more information", GetNewData()) }));
            }
        }

        private async void Bot_OnCallbackQuery(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;

            //await _botClient.AnswerCallbackQueryAsync(
            //    callbackQueryId: callbackQuery.Id,
            //    text:callbackQuery.Data);

            await _botClient.SendTextMessageAsync(
                chatId:callbackQuery.Message.Chat.Id,
                text: callbackQuery.Data);

            _logger.Information($"В чате @{_me.Username} от пользователем {callbackQuery.Message.Chat.Username} " +
                    $"сработал callback {callbackQuery.Id}. Ответ: {callbackQuery.Data}");
        }

        private string GetNewData()
        {
            return "Oops";
        }
    }
}
