using BaseTypes;
using Microsoft.Extensions.Localization;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

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
        private readonly string _verBadStock = MessagesLangEnum.VeryBadStock.GetDescription();
        private readonly string _startText = MessagesLangEnum.StartText.GetDescription();
        

        public LocalizeService()
        {
            //Eng res
            Dictionary<string, string> enDict = new Dictionary<string, string>
            {
                {_secLowerYields, "These securities have lower yields then the indicator (imoex)" },
                {_optimalList, "Optimal distribution of stocks" },
                {_portfolioRisk, "Portfolio risk" },
                {_portfolioEarnings, "Portfolio earnings" },
                {_notOptimalStocks, "These stocks do not constitute an optimal portfolio" },
                {_verBadStock, "This stock has weaker indicators than the rest"},
                {_startText, "Hi, friend!" +
                             "\n\rThis bot is designed to perform a comparative analysis of stocks in terms of risk/earnings." +
                             "\n\rTo obtain the required score, enter a list of tickers of interest, for example, AAPL SBER.ME" +
                             "\n\r.ME indicates that the company's shares are listed on the Moscow stock exchange."}
            };
            //Ru res
            Dictionary<string, string> ruDict = new Dictionary<string, string>
            {
                {_secLowerYields, "Доходность этих акций хуже доходности по индикатору (imoex):" },
                {_optimalList, "Оптимальное распределение долей" },
                {_portfolioRisk, "Риск портфеля" },
                {_portfolioEarnings, "Доходность портфеля" },
                {_notOptimalStocks, "Из этих акций не составить оптимальный портфель" },
                {_verBadStock, "У этой акции показатели слабее остальных"},
                {_startText, "Здравствуй, дорогой друг!" +
                             "\n\rДанный бот предназначен для выполнения сравнительного анализа акций по показателю риск/доходность." +
                             "\n\rДля получения необходимой оценки введите через пробел список интересующих тикеров, например AAPL SBER.ME" +
                             "\n\r.ME указывает, что акции компании представлены на московской бирже."}
            };

            resources = new Dictionary<string, Dictionary<string, string>>
            {
                {"en", enDict },
                {"ru", ruDict }
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
                return new LocalizedString(name, val);
            }
        }

        public LocalizedString this[string name, params object[] arguments] => throw new NotImplementedException();

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
