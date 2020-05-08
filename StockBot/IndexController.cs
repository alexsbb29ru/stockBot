using BaseTypes;
using Exceptions;
using SecuritiesEvaluation;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Models.Enities;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace StockBot
{
    /// <summary>
    /// This controller is a main place. 
    /// </summary>
    public class IndexController : BaseController
    {
        private readonly ISettingsService _settingsService;
        private readonly IExchangeService _exchangeService;
        private readonly ILocalizeService _localizeService;
        private readonly IUserService<Users, Guid> _userService;

        private ITelegramBotClient _botClient;
        private User _me;

        public IndexController(ISettingsService settingsService,
            IExchangeService exchangeService,
            ILocalizeService localizeService, 
            IUserService<Users, Guid> userService)
        {
            _settingsService = settingsService;
            _exchangeService = exchangeService;
            _localizeService = localizeService;
            _userService = userService;
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
                Logger.Information($"Запуск бота");

                _botClient.OnMessage += Bot_OnMessage;
                _botClient.OnCallbackQuery += Bot_OnCallbackQuery;

                _botClient.StartReceiving(Array.Empty<UpdateType>());

                Logger.Information($"Start listening for @{_me.Username}");
                Console.ReadLine();

                _botClient.StopReceiving();
            }
            catch (Exception ex)
            {
                throw new InitBotException(ex, nameof(BotConfig));
            }
        }

        /// <summary>
        /// Bot callback to message from user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="BotOnMessageException"></exception>
        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            try
            {
                if (e.Message.Text == null) return;

                var chat = e.Message.Chat;
                var msg = e.Message.Text;
                var lang = e.Message.From.LanguageCode ?? CultureInfo.CurrentCulture.Name;
                var cultureInfo = CultureInfo.GetCultureInfo(lang);
                var answer = "";

                //Сохраняем пользователей в БД
                var user = _userService.Find(x => x.UserChatId == chat.Id)
                    .ToList().FirstOrDefault();
                
                if (user == null)
                {
                    user = new Users()
                    {
                        Id = new Guid(),
                        UserChatId = chat.Id,
                        UserLogin = chat.Username,
                        UserFirstName = chat.FirstName,
                        UserLastName = chat.LastName,
                        UserRole = ""
                    };
                    await _userService.CreateAsync(user);
                }
                
                //Message for start command
                if (msg.ToLower(cultureInfo) == BotCommands.Start.GetDescription())
                {
                    answer = _localizeService[MessagesLangEnum.StartText.GetDescription(), lang];

                    Logger.Information(
                        $"В чат @{_me.Username} пользователем {(string.IsNullOrEmpty(chat.Username) ? chat.FirstName + ' ' + chat.LastName : chat.Username)} " +
                        $"было отправлено сообщение: {msg}. Ответ: {answer}");

                    await _botClient.SendTextMessageAsync(
                        chatId: chat,
                        text: answer);
                    return;
                }
                //Вывод количества пользователей *только для админов
                if (msg.ToLower(cultureInfo) == BotCommands.UsersCount.GetDescription())
                {
                    Logger.Information(
                        $"Пользователь {(string.IsNullOrEmpty(chat.Username) ? chat.FirstName + ' ' + chat.LastName : chat.Username)} " +
                        $"запросил данные о количестве пользователей");
                    if (user.UserRole == UserRoles.Admin.GetDescription())
                    {
                        var count = _userService.GetCount();
                        Logger.Information(
                            $"Количество пользователей для админа {chat.Username}: {count}");
                        await _botClient.SendTextMessageAsync(
                            chatId: chat,
                            text: count.ToString());
                        return;
                    }
                }
                
                //Вывод списка комманд *только для админов
                if (msg.ToLower(cultureInfo) == BotCommands.AdminCommands.GetDescription())
                {
                    Logger.Information(
                        $"Пользователь {(string.IsNullOrEmpty(chat.Username) ? chat.FirstName + ' ' + chat.LastName : chat.Username)} " +
                        $"запросил команды админов");
                    if (user.UserRole == UserRoles.Admin.GetDescription())
                    {
                        var commandsRow = new List<InlineKeyboardButton>()
                        {
                            InlineKeyboardButton.WithCallbackData("Users count", 
                                $"{nameof(GetUserCount)}")
                        };
                        await _botClient.SendTextMessageAsync(
                            chatId: chat,
                            text: "Команды для админов",
                            replyMarkup: new InlineKeyboardMarkup(commandsRow));

                        return;
                    }
                }
                
                Logger.Information(
                    $"В чат @{_me.Username} пользователем {(string.IsNullOrEmpty(chat.Username) ? chat.FirstName + ' ' + chat.LastName : chat.Username)} " +
                    $"было отправлено сообщение: {msg}.");
                
                if(msg.Contains("/"))
                    return;

                //Полечение списка тикеров с московской биржи
                var russianList = _exchangeService.GetRussianStocks(msg, lang).ToList();
                IList<string> tikersList = msg.ToLower(CultureInfo.GetCultureInfo(lang)).Trim()
                    .Split(' ')
                    .Distinct()
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();
                //После получения русских акций, необходимо их удалить из общего списка, чтобы получить два списка:
                //1. Тикеры с моссковской биржи
                //2. Иностранные тикеры
                if (russianList.Any())
                {
                    tikersList = tikersList.Except(russianList.Select(x => x.ToLower(cultureInfo).Replace(".me", ""))).ToList();
                }

                //Список акций моссковской биржи с их показателями
                var rusEvaluationList = _exchangeService.GetEvaluation(russianList);
                //Список акций иностранной биржи с их показателями
                var interEvaluationList = _exchangeService.GetEvaluation(tikersList);
                
                //Проверяем, что список содержит данные и цикл не пройдет зря
                if (interEvaluationList.Any())
                {
                    if(rusEvaluationList.Any())
                        answer = $"{_localizeService[MessagesLangEnum.IntExchangeOnly.GetDescription(), lang]}";
                    //Формируем данные для акций, представленных на международной бирже
                    answer += GetTikersData(interEvaluationList, "SPY", lang);
                    
                    //Если нет ошибок, выведем в ответе все, что до этого момента накопили в переменную answer
                    await _botClient.SendTextMessageAsync(
                        chatId: chat,
                        text: answer);
                }

                if (rusEvaluationList.Any())
                {
                    if(interEvaluationList.Any())
                        answer = $"{_localizeService[MessagesLangEnum.RusExchangeOnly.GetDescription(), lang]}";
                    
                    //Формируем данные для акций, представленных на московской бирже
                    answer += GetTikersData(rusEvaluationList, "IMOEX.ME", lang);
                    
                    //Если нет ошибок, выведем в ответе все, что до этого момента накопили в переменную answer
                    await _botClient.SendTextMessageAsync(
                        chatId: chat,
                        text: answer);
                }

                Logger.Information(
                    $"Ответ в чате@{_me.Username} пользователю {(string.IsNullOrEmpty(chat.Username) ? chat.FirstName + ' ' + chat.LastName : chat.Username)} " +
                    $"\n\r{answer} \n\r----- На сообщение: {msg}.");
            }
            catch (Exception ex)
            {
                await _botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text:
                    $"\n\r{_localizeService[MessagesLangEnum.NotOptimalStocks.GetDescription(), e.Message.From.LanguageCode ?? CultureInfo.CurrentCulture.Name]}.");
                throw new BotOnMessageException(ex, nameof(Bot_OnMessage));
            }
        }

        private async void Bot_OnCallbackQuery(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            try
            {
                var callbackQuery = callbackQueryEventArgs.CallbackQuery;
                var answer = "";

                if (callbackQuery.Data == nameof(GetUserCount))
                {
                    answer = $"Users count: {GetUserCount()}";
                    Logger.Information($"В чате @{_me.Username} от пользователя {callbackQuery.Message.Chat.Username} " +
                                       $"сработал callback {nameof(GetUserCount)}. Ответ: {answer}");
                }

                await _botClient.AnswerCallbackQueryAsync(
                    callbackQueryId: callbackQuery.Id,
                    text: answer);

                await _botClient.SendTextMessageAsync(
                    chatId: callbackQuery.Message.Chat.Id,
                    text: answer);
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка получения ответа callback. Метод {nameof(Bot_OnCallbackQuery)}" +
                             $"Error: {ex.Message}");
                // throw new BotOnCallBackException(ex, nameof(Bot_OnCallbackQuery));
            }
        }

        /// <summary>
        /// Get optimal stocks from tikers list
        /// </summary>
        /// <param name="earningsRange">Required earnings</param>
        /// <param name="evalList">Evaluation list</param>
        /// <param name="lang">User language</param>
        /// <returns></returns>
        private string GetOptimalStocks(double earningsRange, IList<EvaluationCriteria> evalList, string lang = "en")
        {
            Logger.Information($"Формирование оптимального портфеля в методе {nameof(GetOptimalStocks)}");
            try
            {
                var optimalList = _exchangeService.GetOptimalSecurities(earningsRange, evalList.ToList());
                var cultureInfo = CultureInfo.GetCultureInfo(lang);
                string resultMessage;

                if (!optimalList.Any())
                {
                    resultMessage = $"\n\r{_localizeService[MessagesLangEnum.NotOptimalStocks.GetDescription(), lang]}.";
                    return resultMessage;
                }

                double risk = 0;
                double earnings = 0;

                resultMessage = $"\n\r{_localizeService[MessagesLangEnum.OptimalList.GetDescription(), lang]}:";

                foreach (var stock in optimalList)
                {
                    risk += stock.Risk * stock.Weight / 100;
                    earnings += stock.Earnings * stock.Weight / 100;

                    resultMessage += $"\n\r{stock.Tiker}: {stock.Weight.ToString("F2", cultureInfo)}% ";
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
                Logger.Error($"Ошибка формирования оптимального портфеля. Метод {nameof(GetOptimalStocks)} \n\r" +
                             $"{message}");

                return $"\n\r{_localizeService[MessagesLangEnum.NotOptimalStocks.GetDescription(), lang]}.";
            }
        }
        /// <summary>
        /// Generate data for tikers with different indicators
        /// </summary>
        /// <param name="tikersList">Tikers list</param>
        /// <param name="indicatorName">Name of indicator for different exchange</param>
        /// <param name="lang">Lang for CultureInfo</param>
        /// <returns></returns>
        private string GetTikersData(IList<EvaluationCriteria> tikersList, string indicatorName, string lang = "en")
        {
            string answer = string.Empty;
            var cultureInfo = CultureInfo.GetCultureInfo(lang);
            //Наиболее слабая акция
            var weak = default(EvaluationCriteria);

            tikersList = tikersList.Where(x => !double.IsNaN(x.Deviation)).ToList();

            if (tikersList.Any(x => x.Tiker.ToLower(cultureInfo).Contains("error")))
            {
                var errorTikers = tikersList.Where(x => x.Tiker.ToLower(cultureInfo)
                        .Contains("error"))
                        .Select(x => x)
                        .ToList();
                //Удаляем херовые тикеры из списка, чтобы не учитывать в дальнейших выборках
                tikersList = tikersList.Except(errorTikers).ToList();
                answer += $"\n\r{_localizeService[MessagesLangEnum.BadTickerName.GetDescription(), lang]}:";

                foreach (var tiker in errorTikers)
                {
                    answer += $"\n\r{tiker.Tiker.ToLower(cultureInfo).Replace("error", "")}";
                }

                answer += "\n\r";
            }

            //Проверяем, что количество оставшихся тикеров больше или равно 3
            if (tikersList.Count >= 3)
            {
                //Список акций, показатели которых хуже по индикатору
                var exceptionList = _exchangeService.GetExceptionList(tikersList.ToList(), indicatorName);
                var indicator = _exchangeService.GetIndicator(indicatorName);

                //Если не удалось получить индикатор, напишем пользователю это
                if (indicator.Tiker.ToLower(cultureInfo).Contains("error"))
                    return $"{_localizeService[MessagesLangEnum.BadIndicatorName.GetDescription(), lang]}";

                //Получение самой слабой акции
                weak = _exchangeService.GetWeakerStock(tikersList.ToList());

                //Удаляем ее из общего списка, если он содержит 4 и более записей
                if (tikersList.Count > 4)
                    tikersList.Remove(weak);
                
                //Получение медианы для нахождения оптимального распределения долей
                var median = _exchangeService.GetMedian(tikersList.Select(x => x.Earnings));

                answer += $"{GetOptimalStocks(median, tikersList, lang)}";
                //Если есть плохие акции, выведем их (с голой жопой на мороз) пользователю (чтобы стыдно им стало)
                if (exceptionList.Any())
                {
                    answer +=
                        $"\n\n\r{_localizeService[MessagesLangEnum.SecLowerYields.GetDescription(), lang]}({indicator.Tiker}: {indicator.Earnings.ToString("F2", cultureInfo)}%):";
                    answer = exceptionList.Aggregate(answer,
                        (current, stock) =>
                            current + $"\n\r{stock.Tiker}: {stock.Earnings.ToString("F2", cultureInfo)}%");
                }
            }
            else if (tikersList.Any())
            {
                answer += $"\n\r{_localizeService[MessagesLangEnum.CompanyTitle.GetDescription(), lang]} | " +
                          $"{_localizeService[MessagesLangEnum.RiskTitle.GetDescription(), lang]} | " +
                          $"{_localizeService[MessagesLangEnum.EarningsTile.GetDescription(), lang]} | CV";
                foreach (var stock in tikersList)
                {
                    answer +=
                        $"\n\r{stock.Tiker} | {stock.Risk.ToString("F2", cultureInfo)}% " +
                        $"| {stock.Earnings.ToString("F2", cultureInfo)}% " +
                        $"| {(stock.Risk / stock.Earnings).ToString("F2", cultureInfo)}";
                }

                answer +=
                    $"\n\n\r{_localizeService[MessagesLangEnum.AddMoreStocksGetOptimal.GetDescription(), lang]}.";
                //Получение самой слабой акции
                weak = _exchangeService.GetWeakerStock(tikersList.ToList());
            }

            //Вывод самой плохой (стыдной) акции
            if (tikersList.Count > 1 && weak != null)
            {
                answer += $"\n\n\r{_localizeService[MessagesLangEnum.VeryBadStock.GetDescription(), lang]}:";
                answer += $"\n\r{weak.Tiker}";
            }

            return answer;
        }
        /// <summary>
        /// Get user count method for commands row
        /// </summary>
        /// <returns>Count of bot users</returns>
        private int GetUserCount()
        {
            return _userService.GetCount();
        }
    }
}