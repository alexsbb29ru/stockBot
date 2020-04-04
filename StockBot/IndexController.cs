using BaseTypes;
using Exceptions;
using SecuritiesEvaluation;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace StockBot
{
    /// <summary>
    /// This controller is main place. 
    /// </summary>
    public class IndexController : BaseController
    {
        private readonly ISettingsService _settingsService;
        private readonly IExchangeService _exchangeService;
        private readonly ILocalizeService _localizeService;

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
                throw new InitBotException(ex, nameof(BotConfig));
            }
        }

        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            try
            {
                if (e.Message.Text == null) return;

                var chat = e.Message.Chat;
                var msg = e.Message.Text;
                var lang = e.Message.From.LanguageCode;
                var cultureInfo = CultureInfo.GetCultureInfo(lang);
                var indicatorName = "IMOEX.ME";

                var answer = "";
                //Message for start command
                if (msg.ToLower(cultureInfo) == "/start")
                {
                    answer = _localizeService[MessagesLangEnum.StartText.GetDescription(), lang];

                    _logger.Information($"В чат @{_me.Username} пользователем {chat.Username} " +
                                        $"было отправлено сообщение: {msg}. Ответ: {answer}");

                    await _botClient.SendTextMessageAsync(
                        chatId: chat,
                        text: answer);
                    return;
                }

                //Наиболее слабая акция
                var weak = default(EvaluationCriteria);
                //Список акций с их показателями
                var evaluationList = _exchangeService.GetEvaluation(msg);

                //Проверяем, что список содержит данные и цикл не пройдет зря
                if (evaluationList.Any())
                {
                    if (evaluationList.Any(x => x.Tiker.ToLower(cultureInfo).Contains("error")))
                    {
                        var errorTikers = evaluationList.Where(x => x.Tiker.ToLower(cultureInfo)
                                .Contains("error"))
                            .Select(x => x)
                            .ToList();
                        //Удаляем херовые тикеры из списка, чтобы не учитывать в дальнейших выборках
                        evaluationList = evaluationList.Except(errorTikers).ToList();
                        answer += $"\n\r{_localizeService[MessagesLangEnum.BadTikerName.GetDescription(), lang]}:";

                        foreach (var tiker in errorTikers)
                        {
                            answer += $"\n\r{tiker.Tiker.ToLower(cultureInfo).Replace("error", "")}";
                        }

                        answer += "\n\r";
                    }

                    //Проверяем, что количество оставшихся тикеров больше или равно 3
                    if (evaluationList.Count >= 3)
                    {
                        //Получение маскимальной доходности для получения списка с оптимальным распределением долей
                        var maxEarnings = evaluationList.Max(x => x.Earnings);
                        //Список акций, показатели которых хуже по индикатору
                        var exceptionList = _exchangeService.GetExceptionList(evaluationList, indicatorName);
                        var indicator = _exchangeService.GetIndicator(indicatorName);

                        //Получение самой слабой акции
                        weak = _exchangeService.GetWeakerStock(evaluationList);

                        //Удаляем ее из общего списка, если он содержит 4 и более записей
                        if (evaluationList.Count > 4)
                            evaluationList.Remove(weak);

                        answer += $"{GetOptimalStocks(maxEarnings, evaluationList, lang)}";
                        //Если есть плохие акции, выведем их (с голой жопой на мороз) пользователю (чтобы стыдно им стало)
                        if (exceptionList.Any())
                        {
                            answer +=
                                $"\n\n\r{_localizeService[MessagesLangEnum.SecLowerYields.GetDescription(), lang]}({indicator.Tiker} - {indicator.Earnings.ToString("F2", cultureInfo)}%):";
                            answer = exceptionList.Aggregate(answer,
                                (current, stock) =>
                                    current + $"\n\r{stock.Tiker}: {stock.Earnings.ToString("F2", cultureInfo)}%");
                        }
                    }
                    else if (evaluationList.Any())
                    {
                        answer += $"\n\r{_localizeService[MessagesLangEnum.CompanyTitle.GetDescription(), lang]} | " +
                                  $"{_localizeService[MessagesLangEnum.RiskTitle.GetDescription(), lang]} | " +
                                  $"{_localizeService[MessagesLangEnum.EarningsTile.GetDescription(), lang]} | CV";
                        foreach (var stock in evaluationList)
                        {
                            answer +=
                                $"\n\r{stock.Tiker} | {stock.Risk.ToString("F2", cultureInfo)}% " +
                                $"| {stock.Earnings.ToString("F2", cultureInfo)}% " +
                                $"| {(stock.Risk / stock.Earnings).ToString("F2", cultureInfo)}";
                        }

                        answer +=
                            $"\n\n\r{_localizeService[MessagesLangEnum.AddMoreStocksGetOptimal.GetDescription(), lang]}.";
                        //Получение самой слабой акции
                        weak = _exchangeService.GetWeakerStock(evaluationList);
                    }

                    //Вывод самой плохой (стыдной) акции
                    if (evaluationList.Count > 1 && weak != null)
                    {
                        answer += $"\n\n\r{_localizeService[MessagesLangEnum.VeryBadStock.GetDescription(), lang]}:";
                        answer += $"\n\r{weak.Tiker}";
                    }
                }

                _logger.Information($"В чат @{_me.Username} пользователем {chat.Username} " +
                                    $"было отправлено сообщение: {msg}. Ответ: {answer}");

                //Если нет ошибок, выведем в ответе все, что до этого момента накопили в переменную answer

                await _botClient.SendTextMessageAsync(
                    chatId: chat,
                    text: answer);
            }
            catch (Exception ex)
            {
                throw new BotOnMessageException(ex, nameof(Bot_OnMessage));
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
                throw new BotOnCallBackException(ex, nameof(Bot_OnCallbackQuery));
            }
        }

        /// <summary>
        /// Get optimal stocks from tikers list
        /// </summary>
        /// <param name="earningsRange">Required earnings</param>
        /// <param name="evalList">Evaluation list</param>
        /// <param name="lang">User language</param>
        /// <returns></returns>
        private string GetOptimalStocks(double earningsRange, List<EvaluationCriteria> evalList, string lang = "en")
        {
            
            try
            {
                var optimalList = _exchangeService.GetOptimalSecurities(earningsRange, evalList);
                var cultureInfo = CultureInfo.GetCultureInfo(lang);
                string resultMessage;
                
                if (!optimalList.Any())
                    resultMessage =
                        $"{_localizeService[MessagesLangEnum.NotOptimalStocks.GetDescription(), lang]}.";

                double risk = 0;
                double earnings = 0;

                resultMessage = $"\n\r{_localizeService[MessagesLangEnum.OptimalList.GetDescription(), lang]}:";

                foreach (var stock in optimalList)
                {
                    risk += stock.Risk * stock.Weight / 100;
                    earnings += stock.Earnings * stock.Weight / 100;

                    resultMessage += $"\n\r{stock.Tiker} | {stock.Weight.ToString("F2", cultureInfo)}% ";
                }

                resultMessage +=
                    $"\n\r{_localizeService[MessagesLangEnum.PortfolioRisk.GetDescription(), lang]}: " +
                    $"{risk.ToString("F2", cultureInfo)}%";
                resultMessage +=
                    $"\n\r{_localizeService[MessagesLangEnum.PortfolioEarnings.GetDescription(), lang]}: {earnings.ToString("F2", cultureInfo)}%";

                return resultMessage;
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                _logger.Error($"Ошибка формирования оптимального портфеля. Метод {nameof(GetOptimalStocks)} \n\r" +
                              $"{message}");
                
                return $"{_localizeService[MessagesLangEnum.NotOptimalStocks.GetDescription(), lang]}.";
                throw new GetOptimalListException(ex, nameof(GetOptimalStocks));
            }
        }
    }
}