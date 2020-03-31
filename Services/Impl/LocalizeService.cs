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
        private readonly string SecLowerYields = MessagesLangEnum.SecLowerYields.GetDescription();
        private readonly string OptimalList = MessagesLangEnum.OptimalList.GetDescription();
        private readonly string PortfolioRisk = MessagesLangEnum.PortfolioRisk.GetDescription();
        private readonly string PortfolioEarnings = MessagesLangEnum.PortfolioEarnings.GetDescription();
        private readonly string NotOptimalStocks = MessagesLangEnum.NotOptimalStocks.GetDescription();

        public LocalizeService()
        {
            //Eng res
            Dictionary<string, string> enDict = new Dictionary<string, string>
            {
                {SecLowerYields, "These securities have lower yields then the indicator (imoex)" },
                {OptimalList, "Optimal distribution of stocks" },
                {PortfolioRisk, "Portfolio risk" },
                {PortfolioEarnings, "Portfolio earnings" },
                {NotOptimalStocks, "These stocks do not constitute an optimal portfolio" }
            };
            //Ru res
            Dictionary<string, string> ruDict = new Dictionary<string, string>
            {
                {SecLowerYields, "Доходность этих акций хуже доходности по индикатору (imoex):" },
                {OptimalList, "Оптимальное распределение долей" },
                {PortfolioRisk, "Риск портфеля" },
                {PortfolioEarnings, "Доходность портфеля" },
                {NotOptimalStocks, "Из этих акций не составить оптимальный портфель" }
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
