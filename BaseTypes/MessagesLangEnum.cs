using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

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
        StartText
    }

    public static class EnumExtensionMethods
    {
        public static string GetDescription(this Enum genericEnum)
        {
            var genericEnumType = genericEnum.GetType();
            var memberInfo = genericEnumType.GetMember(genericEnum.ToString());
            if (memberInfo.Length <= 0) return genericEnum.ToString();
            var attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return attribs.Any() ? ((System.ComponentModel.DescriptionAttribute) attribs.ElementAt(0)).Description : genericEnum.ToString();
        }

    }
}
