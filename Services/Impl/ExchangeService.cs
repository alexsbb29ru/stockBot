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
        public List<EvaluationCriteria> GetEvaluation(string tikers)
        {
            if (string.IsNullOrEmpty(tikers))
                return default;

            var cultureInfo = CultureInfo.CurrentCulture;
            var dates = GetDates();
            

            var tikersArr = tikers.ToLower(cultureInfo).Trim().Split(' ').Distinct().Where(x => !string.IsNullOrEmpty(x));

            List<EvaluationCriteria> evaluationList = new List<EvaluationCriteria>();
            List<EvaluationCriteria> exceptionList = new List<EvaluationCriteria>();

            foreach (var item in tikersArr)
            {
                evaluationList.Add(EvaluateSecurities(item, dates.startDate, dates.endDate));
            }

            return evaluationList;
        }
        /// <summary>
        /// Return exception stoks list
        /// </summary>
        /// <param name="evalList"></param>
        /// <returns></returns>
        public List<EvaluationCriteria> GetExceptionList(List<EvaluationCriteria> evalList)
        {
            var indicator = GetIndicator("IMOEX.ME");
            return EvaluationMethods.GetBelowIndicatorSecurities(indicator, evalList);
        }
        /// <summary>
        /// Get optimal distribution of shares 
        /// </summary>
        /// <param name="earningLevel">level of profitability</param>
        /// <param name="evalList">Evaluation list</param>
        /// <returns></returns>
        public List<EvaluationCriteria> GetOptimalSecurities(double earningLevel, List<EvaluationCriteria> evalList)
        {
            var optimalList = EvaluationMethods.OptimizeSecurities(earningLevel, evalList);

            return optimalList;
        }

        private EvaluationCriteria GetIndicator(string exchangeName)
        {
            var dates = GetDates();
            var indicator = EvaluationMethods.MADEvaluateSecurities(exchangeName, dates.startDate, dates.endDate);

            return indicator;
        }

        private (DateTime startDate, DateTime endDate) GetDates()
        {
            var startYear = DateTime.Now.Year - 5;
            var startDate = new DateTime(startYear, 1, 1);

            var endYear = DateTime.Now.Year;
            var endMonth = DateTime.Now.Month == 1 ? 1 : DateTime.Now.Month - 1;
            var endDay = DateTime.DaysInMonth(endYear, endMonth);
            var endDate = new DateTime(endYear, endMonth, endDay);

            return (startDate, endDate);
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
