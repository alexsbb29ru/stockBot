using BaseTypes;
using Exceptions;
using Init.Interfaces;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using SecuritiesEvaluation;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace StockBot
{
    /// <summary>
    /// This controller is main place. 
    /// </summary>
    public class IndexController : BaseController
    {
        private ISettingsService _settingsService;
        private IExchangeService _exchangeService;
        private ILocalizeService _localizeService;

        private ITelegramBotClient _botClient;
        private User _me;
        public IndexController(ISettingsService settingsService,
            IExchangeService exchangeService, 
            ILocalizeService localizeService)
        {
            _settingsService = settingsService;
            _exchangeService = exchangeService;
            _localizeService = localizeService;
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
            try
            {
                //var proxyConfig = _settingsService.GetProxyConfig(nameof(BotConfig));
                //var proxy = new WebProxy(proxyConfig.Host, proxyConfig.Port) { UseDefaultCredentials = true };
                var botToken = _settingsService.GetTelegramToken(nameof(BotConfig));
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
            catch (Exception ex)
            {
                throw new InitBotException(ex.Message, ex.InnerException, nameof(BotConfig));
            }
            
        }

        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            try
            {
                if (e.Message.Text != null)
                {
                    var chat = e.Message.Chat;
                    var msg = e.Message.Text;

                    var lang = e.Message.From.LanguageCode;
                    var answer = "";
                    
                    var evaluationList = _exchangeService.GetEvaluation(msg);

                    //Проверяем, что список содержит данные и цикл не пройдет зря
                    if (evaluationList.Any())
                    {
                        var maxEarnings = evaluationList.Max(x => x.Earnings);

                        var exceptionList = _exchangeService.GetExceptionList(evaluationList);
                        evaluationList = evaluationList.Except(exceptionList).ToList();
                        answer += $"{GetOptimalStocks(maxEarnings, evaluationList, lang)}";

                        if (exceptionList.Any())
                        {
                            answer += $"\n\n\r{_localizeService[MessagesLangEnum.SecLowerYields.GetDescription(), lang]}";

                            foreach (var except in exceptionList)
                            {
                                answer += $"\n\r {except.Tiker}";
                            }
                        } 
                    }

                    _logger.Information($"В чат @{_me.Username} пользователем {chat.Username} " +
                        $"было отправлено сообщение: {msg}. Ответ: {answer}");


                    if (!answer.ToLower().Contains("error"))
                    {
                        await _botClient.SendTextMessageAsync(
                        chatId: chat,
                        text: answer);
                    }
                    else
                    {
                        await _botClient.SendTextMessageAsync(
                        chatId: chat,
                        text: answer);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BotOnMessageException(ex.Message, ex.InnerException, nameof(Bot_OnMessage));
            }

        }

        private async void Bot_OnCallbackQuery(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            try
            {
                var callbackQuery = callbackQueryEventArgs.CallbackQuery;

                _logger.Information($"В чате @{_me.Username} от пользователем {callbackQuery.Message.Chat.Username} " +
                       $"сработал callback {callbackQuery.Id}. Ответ: {callbackQuery.Data}");

                await _botClient.AnswerCallbackQueryAsync(
                    callbackQueryId: callbackQuery.Id,
                    text: callbackQuery.Data);

                await _botClient.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: callbackQuery.Data);
            }
            catch (Exception ex)
            {
                throw new BotOnCallBackException(ex.Message, ex.InnerException, nameof(Bot_OnCallbackQuery));
            }
        }
        /// <summary>
        /// Get optimal stocks from tikers list
        /// </summary>
        /// <param name="earningsRange">Required earnings</param>
        /// <param name="evalList">Evaluation list</param>
        /// <returns></returns>
        private string GetOptimalStocks(double earningsRange, List<EvaluationCriteria> evalList, string lang = "en")
        {
            try
            {
                var optimalList = _exchangeService.GetOptimalSecurities(earningsRange, evalList);
                var cultureInfo = CultureInfo.GetCultureInfo(lang);
                string resultMessage;
                if (optimalList.Any())
                {
                    double risk = 0;
                    double earnings = 0;

                    resultMessage = $"\n\r{_localizeService[MessagesLangEnum.OptimalList.GetDescription(), lang]}:";

                    for (int i = 0; i < optimalList.Count; i++)
                    {
                        risk += optimalList[i].Risk * optimalList[i].Weight / 100;
                        earnings += optimalList[i].Earnings * optimalList[i].Weight / 100;

                        resultMessage += $"\n\r{optimalList[i].Tiker} | {optimalList[i].Weight.ToString("F2", cultureInfo)}% ";
                    }

                    resultMessage += $"\n\r{_localizeService[MessagesLangEnum.PortfolioRisk.GetDescription(), lang]}: " +
                        $"{risk.ToString("F2", cultureInfo)}";
                    resultMessage += $"\n\r{_localizeService[MessagesLangEnum.PortfolioEarnings.GetDescription(), lang]}: {earnings.ToString("F2", cultureInfo)}";
                }
                else
                    resultMessage = $"{_localizeService[MessagesLangEnum.NotOptimalStocks.GetDescription(), lang]}.";

                return resultMessage;
            }
            catch (Exception ex)
            {
                throw new GetOptimalListException(ex.Message, ex.InnerException, nameof(GetOptimalStocks));
            }
        }
    }
}
