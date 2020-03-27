using BaseTypes;
using Models;
using SecuritiesEvaluation;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YahooFinanceApi;

namespace Services.Impl
{
    public class ExchangeService : BaseController, IExchangeService
    {
        public string GetEvaluation(string tikers)
        {
            if (string.IsNullOrEmpty(tikers))
                return "Error. Tikers list is empty";

            var cultureInfo = CultureInfo.CurrentCulture;

            var startYear = DateTime.Now.Year - 5;
            var startDate = new DateTime(startYear, 1, 1);

            var endYear = DateTime.Now.Year;
            var endMonth = DateTime.Now.Month == 1 ? 1 : DateTime.Now.Month - 1;
            var endDay = DateTime.DaysInMonth(endYear, endMonth);
            var endDate = new DateTime(endYear, endMonth, endDay);

            var tikersArr = tikers.ToLower(cultureInfo).Trim().Split(' ').Distinct().Where(x => !string.IsNullOrEmpty(x));
            var indicator = EvaluationMethods.MADEvaluateSecurities("IMOEX.ME", startDate, endDate);

            List<EvaluationCriteria> evaluationList = new List<EvaluationCriteria>();
            List<EvaluationCriteria> exceptionList = new List<EvaluationCriteria>();

            foreach(var item in tikersArr)
            {
                evaluationList.Add(EvaluateSecurities(item, startDate, endDate));
            }

            exceptionList = EvaluationMethods.GetBelowIndicatorSecurities(indicator, evaluationList);

            var resultMessage = "Company: Risk | Earnings | CV | Period";

            foreach(var item in evaluationList)
            {
                if (!item.Tiker.ToLower(cultureInfo).Contains("error"))
                {
                    resultMessage += $"\n\r{item.Tiker}: {item.Risk.ToString("F2", cultureInfo)} | {item.Earnings.ToString("F2", cultureInfo)}" +
                        $" | {(item.Risk / item.Earnings).ToString("F2", cultureInfo)} | {DateTime.Now.Year - 5} - {DateTime.Now.Year}\n\r";
                }

                else
                    resultMessage += $"\n\r{item.Tiker.Replace("error", "")} is wrong tiker";
            }
            if (exceptionList.Any())
            {
                resultMessage += $"\n\rThese stocks have a worse return on the indicator:\n\r";

                foreach (var except in exceptionList)
                {
                    resultMessage += $"\n\r {except.Tiker}";
                }
            }

            return resultMessage;
        }

        private EvaluationCriteria EvaluateSecurities(string tiker, DateTime startDate, DateTime endDate)
        {
            try
            {
                _logger.Information($"Получение данных по тикеру {tiker}. Метод {nameof(EvaluateSecurities)}");
                //string securities = string.Concat(tiker, ".ME");
                string securities = tiker;

                var evalCriteria = EvaluationMethods.MADEvaluateSecurities(securities, startDate, endDate);

                return evalCriteria;
            }
            catch (Exception e)
            {
                var message = e.InnerException.Message ?? e.Message;
                _logger.Error($"Ошибка получения данных по тикеру {tiker}. Метод {nameof(EvaluateSecurities)} \n\r" +
                    $"{message}");
                return new EvaluationCriteria(tiker + "error", 0, 0, 0, 0);
            }
        }
    }
}
