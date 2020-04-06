using BaseTypes;
using Microsoft.Extensions.Localization;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Services.Impl
{
    public class LocalizeService : ILocalizeService
    {
        Dictionary<string, Dictionary<string, string>> resources;

        //Ключи ресурсов
        private readonly string _secLowerYields = MessagesLangEnum.SecLowerYields.GetDescription();
        private readonly string _optimalList = MessagesLangEnum.OptimalList.GetDescription();
        private readonly string _portfolioRisk = MessagesLangEnum.PortfolioRisk.GetDescription();
        private readonly string _portfolioEarnings = MessagesLangEnum.PortfolioEarnings.GetDescription();
        private readonly string _notOptimalStocks = MessagesLangEnum.NotOptimalStocks.GetDescription();
        private readonly string _veryBadStock = MessagesLangEnum.VeryBadStock.GetDescription();
        private readonly string _startText = MessagesLangEnum.StartText.GetDescription();
        private readonly string _badTikerName = MessagesLangEnum.BadTikerName.GetDescription();
        private readonly string _companyTitle = MessagesLangEnum.CompanyTitle.GetDescription();
        private readonly string _riskTitle = MessagesLangEnum.RiskTitle.GetDescription();
        private readonly string _earningsTitle = MessagesLangEnum.EarningsTile.GetDescription();
        private readonly string _addMoreStocksGetOptimal = MessagesLangEnum.AddMoreStocksGetOptimal.GetDescription();

        public LocalizeService()
        {
            //Eng res
            Dictionary<string, string> enDict = new Dictionary<string, string>
            {
                {_secLowerYields, "The earnings on these financial instruments is less than the return on the indicator"},
                {_optimalList, "Optimal distribution of stocks"},
                {_portfolioRisk, "Portfolio risk"},
                {_portfolioEarnings, "Portfolio earnings"},
                {_notOptimalStocks, "These financial instruments do not form an optimal portfolio"},
                {_veryBadStock, "The performance of this financial instrument is weaker than the rest."},
                {
                    _startText, "Hi, friend!" +
                                "\n\rThis bot will increase your accuracy when making investment decisions. " +
                                "It will help to reduce risk and form an optimal investment portfolio from the assets you have chosen. " +
                                "\n\rTo perform an analysis, enter a ticker (short name of the quoted instrument: stock, index, fund etf). " +
                                "\n\rThe result will be optimal if you enter several tickers - more than 4. " +
                                "\n\rAn example of the sber.me aapl ticker. (.me indicates that the instrument is listed on the Moscow stock exchange) " +
                                "\n\rBot responses are not intended for trading and are not investment recommendations. Good luck and profit!"
                },
                {_badTikerName, "This ticker does not exist"},
                {_companyTitle, "Company"},
                {_riskTitle, "Risk"},
                {_earningsTitle, "Earnings"},
                {_addMoreStocksGetOptimal, "To create an optimal portfolio, enter several tickers with a space"}
                
            };
            //Ru res
            Dictionary<string, string> ruDict = new Dictionary<string, string>
            {
                {_secLowerYields, "Доходность этих финансовых инструментов меньше доходности по индикатору"},
                {_optimalList, "Оптимальное распределение долей"},
                {_portfolioRisk, "Риск портфеля"},
                {_portfolioEarnings, "Средняя доходность (год)"},
                {_notOptimalStocks, "Из этих финансовых инструментов не сформировать оптимальный портфель"},
                {_veryBadStock, "Показатели этого финансового инструмента слабее остальных"},
                {
                    _startText, "Приветствую!" +
                                "\n\rЭтот бот повысит вашу точность при принятии инвестиционных решений. " +
                                "Поможет снизить риск и сформировать оптимальный инвестиционный портфель из выбранных вами активов. " +
                                "\n\rЧтобы выполнить анализ введите тикер (краткое название котируемого инструмента: акции, индекса, etf фонда). " +
                                "\n\rРезультат будет оптимальнее, если ввести несколько тикеров - больше 4. " +
                                "\n\rПример тикера sber.me aapl. (.me указывает, что инструмент представлен на московской бирже) " +
                                "\n\rОтветы бота не предназначены для трейдинга и не являются инвестиционными рекомендациями. Желаем Вам успехов и прибыли!"
                },
                {_badTikerName, "Данный тикер не существует"},
                {_companyTitle, "Компания"},
                {_riskTitle, "Риск"},
                {_earningsTitle, "Доходность"},
                {_addMoreStocksGetOptimal, "Чтобы сформировать оптимальный портфель, введите несколько тикеров через пробел"}
            };

            resources = new Dictionary<string, Dictionary<string, string>>
            {
                {"en", enDict},
                {"ru", ruDict}
            };
        }

        public LocalizedString this[string name]
        {
            get
            {
                var currentCulture = CultureInfo.CurrentUICulture;

                string val = "";
                if (resources.ContainsKey(currentCulture.Name))
                {
                    if (resources[currentCulture.Name].ContainsKey(name))
                    {
                        val = resources[currentCulture.Name][name];
                    }
                }

                return new LocalizedString(name, val);
            }
        }

        /// <summary>
        /// Get translated string to specified language 
        /// </summary>
        /// <param name="name">Name of string containing the desired translation</param>
        /// <param name="lang">Required language</param>
        public LocalizedString this[string name, string lang]
        {
            get
            {
                string val = "";
                if (resources.ContainsKey(lang))
                {
                    if (resources[lang].ContainsKey(name))
                    {
                        val = resources[lang][name];
                    }
                }
                else
                {
                    lang = "en";
                    if (resources[lang].ContainsKey(name))
                        val = resources[lang][name];
                }

                return new LocalizedString(name, val);
            }
        }

        /// <summary>
        /// Unused prop
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        public LocalizedString this[string name, params object[] arguments] => default;

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}