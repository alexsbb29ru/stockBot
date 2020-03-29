using BaseTypes;
using Init.Interfaces;
using Newtonsoft.Json;
using SecuritiesEvaluation;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
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

        private ITelegramBotClient _botClient;
        private User _me;
        public IndexController(ISettingsService settingsService,
            IExchangeService exchangeService)
        {
            _settingsService = settingsService;
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

        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            try
            {
                if (e.Message.Text != null)
                {
                    var chat = e.Message.Chat;
                    var msg = e.Message.Text;

                    var cultureInfo = CultureInfo.CurrentCulture;
                    var answer = "Company: Risk | Earnings | CV | Period";

                    var evaluationList = _exchangeService.GetEvaluation(msg);
                    var exceptionList = _exchangeService.GetExceptionList(evaluationList);
                    

                    //Проверяем, что список содержит данные и цикл не пройдет зря
                    if (evaluationList.Any())
                    {
                        foreach (var item in evaluationList)
                        {
                            if (!item.Tiker.ToLower(cultureInfo).Contains("error"))
                            {
                                answer += $"\n\r{item.Tiker}: {item.Risk.ToString("F2", cultureInfo)} | {item.Earnings.ToString("F2", cultureInfo)}" +
                                    $" | {(item.Risk / item.Earnings).ToString("F2", cultureInfo)} | {DateTime.Now.Year - 5} - {DateTime.Now.Year}\n\r";
                            }

                            else
                                answer += $"\n\r{item.Tiker.Replace("error", "")} is wrong tiker";
                        }
                    }

                    if (exceptionList.Any())
                    {
                        answer += $"\n\rThese stocks have a worse return on the indicator:\n\r";

                        foreach (var except in exceptionList)
                        {
                            answer += $"\n\r {except.Tiker}";
                        }
                    }

                    _logger.Information($"В чат @{_me.Username} пользователем {chat.Username} " +
                        $"было отправлено сообщение: {msg}. Ответ: {answer}");


                    if (!answer.ToLower().Contains("error"))
                    {
                        //answer += "\n\rWhat level of profitability?";

                        var firstRow = new List<InlineKeyboardButton>()
                        {
                            InlineKeyboardButton.WithCallbackData("0-10%", GetOptimalStocks("0-10", evaluationList)),
                            InlineKeyboardButton.WithCallbackData("10-20%", GetOptimalStocks("10-20", evaluationList)),
                            InlineKeyboardButton.WithCallbackData("20-30%", GetOptimalStocks("20-30", evaluationList))
                        };

                        var secondRow = new List<InlineKeyboardButton>()
                        {
                            InlineKeyboardButton.WithCallbackData("30-40%", GetOptimalStocks("30-40", evaluationList)),
                            InlineKeyboardButton.WithCallbackData("40-50%", GetOptimalStocks("40-50", evaluationList)),
                            InlineKeyboardButton.WithCallbackData("50-60%", GetOptimalStocks("50-60", evaluationList)),
                        };

                        var thirdRow = new List<InlineKeyboardButton>()
                        {
                            InlineKeyboardButton.WithCallbackData("60-70%", GetOptimalStocks("60-70", evaluationList)),
                            InlineKeyboardButton.WithCallbackData("70-80%", GetOptimalStocks("70-80", evaluationList)),
                            InlineKeyboardButton.WithCallbackData("80-90%", GetOptimalStocks("80-90", evaluationList))
                        };

                        var fourthRow = new List<InlineKeyboardButton>()
                        {
                            InlineKeyboardButton.WithCallbackData("90-100%", GetOptimalStocks("90-100", evaluationList))
                        };

                        

                        var earningsKeyBoard = new List<List<InlineKeyboardButton>>()
                        {
                            firstRow,
                            secondRow,
                            thirdRow,
                            fourthRow
                        };
                        await _botClient.SendTextMessageAsync(
                        chatId: chat,
                        text: answer);
                        //await _botClient.SendTextMessageAsync(
                        //chatId: chat,
                        //text: answer,
                        //replyMarkup: new InlineKeyboardMarkup(earningsKeyBoard));
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

                throw;
            }

        }

        private async void Bot_OnCallbackQuery(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            try
            {
                var callbackQuery = callbackQueryEventArgs.CallbackQuery;

                await _botClient.AnswerCallbackQueryAsync(
                    callbackQueryId: callbackQuery.Id,
                    text: callbackQuery.Data);

                await _botClient.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: callbackQuery.Data);

                _logger.Information($"В чате @{_me.Username} от пользователем {callbackQuery.Message.Chat.Username} " +
                        $"сработал callback {callbackQuery.Id}. Ответ: {callbackQuery.Data}");
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private string GetOptimalStocks(string text) => text;
        private string GetOptimalStocks(string earningsRange, List<EvaluationCriteria> evalList)
        {
            var range = earningsRange.Split("-").Select(x => x.Trim()).ToList();
            var lowRange = double.Parse(range[0]);
            var upRange = double.Parse(range[1]);

            var optimalList = _exchangeService.GetOptimalSecurities(upRange, evalList);
            var resultMessage = "";

            if (optimalList.Any())
            {
                double risk = 0;
                double earnings = 0;

                for (int i = 0; i < optimalList.Count; i++)
                {
                    risk += optimalList[i].Risk * optimalList[i].Weight / 100;
                    earnings += optimalList[i].Earnings * optimalList[i].Weight / 100;

                    resultMessage += $"\n\r{optimalList[i].Tiker} | {optimalList[i].Weight.ToString("F2", CultureInfo.CurrentCulture)}";

                    resultMessage += $"\n\rPortfolio risk: {risk.ToString("F2", CultureInfo.CurrentCulture)}";
                    resultMessage += $"\n\rPortfolio earnings: {earnings.ToString("F2", CultureInfo.CurrentCulture)}";
                }
            }
            else
                resultMessage = "These stocks do not constitute an optimal portfolio.";

            return "";
        }
    }
}
