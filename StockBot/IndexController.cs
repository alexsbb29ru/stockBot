using BaseTypes;
using Init.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
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
        public IndexController(IInitSettings initSetting, IExchangeService exchangeService)
        {
            _initSetting = initSetting;
            _exchangeService = exchangeService;
        }

        public void Index()
        {
            BotConfig();
        }
        /// <summary>
        /// Bot configuration
        /// </summary>
        private void BotConfig()
        {
            var botToken = _initSetting.GetToken(nameof(BotConfig));
            _botClient = new TelegramBotClient(botToken);

            var bot = _botClient.GetMeAsync().Result;
            _logger.Information($"Запуск бота");

            _botClient.OnMessage += Bot_OnMessage;
            _botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if(e.Message.Text != null)
            {
                var chat = e.Message.Chat;
                var msg = e.Message.Text;
                var answer = _exchangeService.GetEvaluation(msg);

                _logger.Information($"В чат ID:{chat.Id} пользователем {chat.FirstName} {chat.LastName} " +
                    $"было отправлено сообщение: {msg}. Ответ: {answer}");

                await _botClient.SendTextMessageAsync(
                    chatId: chat,
                    text: answer);
            }
        }
    }
}
