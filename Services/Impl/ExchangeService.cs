using BaseTypes;
using Exceptions;
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


            var tikersArr = tikers.ToLower(cultureInfo).Trim().Split(' ').Distinct()
                .Where(x => !string.IsNullOrEmpty(x));

            List<EvaluationCriteria> evaluationList = tikersArr.Select(item => EvaluateSecurities(item, dates.startDate, dates.endDate)).ToList();

            _logger.Information($"Получение данных риска / доходности по тикерам {nameof(GetEvaluation)}");

            return evaluationList;
        }

        /// <summary>
        /// Return exception stoks list
        /// </summary>
        /// <param name="evalList">Evaluation list</param>
        /// <param name="indicatorName">Base indicator name</param>
        /// <returns></returns>
        public List<EvaluationCriteria> GetExceptionList(List<EvaluationCriteria> evalList, string indicatorName)
        {
            try
            {
                var indicator = GetIndicator(indicatorName);

                _logger.Information($"Получение хреновых акций в методе {nameof(GetExceptionList)}");
                return EvaluationMethods.GetBelowIndicatorSecurities(indicator, evalList);
            }
            catch (Exception e)
            {
                //TODO: add exception
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Get optimal distribution of shares 
        /// </summary>
        /// <param name="earningLevel">level of profitability</param>
        /// <param name="evalList">Evaluation list</param>
        /// <returns></returns>
        public List<EvaluationCriteria> GetOptimalSecurities(double earningLevel, List<EvaluationCriteria> evalList)
        {
            try
            {
                _logger.Information($"Получение оптимальных долей в методе {nameof(GetOptimalSecurities)}");
                var optimalList = EvaluationMethods.OptimizeSecurities(earningLevel, evalList);

                return optimalList;
            }
            catch (Exception ex)
            {
                throw new GetOptimalListException(ex, nameof(GetOptimalSecurities));
            }
        }

        /// <summary>
        /// Get weaker stock
        /// </summary>
        /// <param name="evalList">Evaluation list</param>
        /// <returns>Weaker stock</returns>
        public EvaluationCriteria GetWeakerStock(List<EvaluationCriteria> evalList)
        {
            try
            {
                _logger.Information($"Получение самой слабой акции в методе {nameof(GetWeakerStock)}");
                var weak = EvaluationMethods.GetWeakSecurity(evalList);
                return weak;
            }
            catch (Exception ex)
            {
                throw new GetWeakerStockException(ex, nameof(GetWeakerStock));
            }
        }
        /// <summary>
        /// Get base indicator for compare with other stoks
        /// </summary>
        /// <param name="exchangeName">Base indicator name</param>
        /// <returns></returns>
        public EvaluationCriteria GetIndicator(string exchangeName)
        {
            var dates = GetDates();
            var indicator = EvaluationMethods.MADEvaluateSecurities(exchangeName, dates.startDate, dates.endDate);

            _logger.Information($"Получение индикатора в методе {nameof(GetIndicator)}: {indicator}");
            return indicator;
        }

        private (DateTime startDate, DateTime endDate) GetDates()
        {
            var startYear = DateTime.Now.Year - 5;
            var startDate = new DateTime(2007, 1, 1);

            var endYear = DateTime.Now.Year;
            var endMonth = DateTime.Now.Month == 1 ? 1 : DateTime.Now.Month - 1;
            var endDay = DateTime.DaysInMonth(endYear, endMonth);
            var endDate = new DateTime(endYear, endMonth, endDay);

            _logger.Information($"Формирование даты в методе {nameof(GetDates)}");

            return (startDate, endDate);
        }

        private EvaluationCriteria EvaluateSecurities(string tiker, DateTime startDate, DateTime endDate)
        {
            try
            {
                _logger.Information($"Получение данных по тикеру {tiker}. Метод {nameof(EvaluateSecurities)}");

                string securities = tiker;
                var evalCriteria = EvaluationMethods.MADEvaluateSecurities(securities, startDate, endDate);

                return evalCriteria;
            }
            catch (Exception e)
            {
                var message = e.InnerException?.Message ?? e.Message;
                _logger.Error($"Ошибка получения данных по тикеру {tiker}. Метод {nameof(EvaluateSecurities)} \n\r" +
                              $"{message}");
                return new EvaluationCriteria(tiker + "error", 0, 0, 0, 0);
            }
        }
    }
}