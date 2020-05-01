using System;
using System.ComponentModel;
using System.Linq;

namespace BaseTypes
{
    public enum MessagesLangEnum
    {
        [Description("SecLowerYields")]
        SecLowerYields,
        [Description("OptimalList")]
        OptimalList,
        [Description("PortfolioRisk")]
        PortfolioRisk,
        [Description("PortfolioEarnings")]
        PortfolioEarnings,
        [Description("NotOptimalStocks")]
        NotOptimalStocks,
        [Description("VeryBadStock")]
        VeryBadStock,
        [Description("StartText")]
        StartText,
        [Description("BadTikerName")]
        BadTikerName,
        [Description("CompanyTitle")]
        CompanyTitle,
        [Description("RiskTitle")]
        RiskTitle,
        [Description("EarningsTile")]
        EarningsTile,
        [Description("AddMoreStocksGetOptimal")]
        AddMoreStocksGetOptimal,
        [Description("RusExchangeOnly")]
        RusExchangeOnly,
        [Description("IntExchangeOnly")]
        IntExchangeOnly
    }

    public static class EnumExtensionMethods
    {
        public static string GetDescription(this Enum genericEnum)
        {
            var genericEnumType = genericEnum.GetType();
            var memberInfo = genericEnumType.GetMember(genericEnum.ToString());
            if (memberInfo.Length <= 0) return genericEnum.ToString();
            var attribs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attribs.Any() ? ((DescriptionAttribute) attribs.ElementAt(0)).Description : genericEnum.ToString();
        }

    }
}
