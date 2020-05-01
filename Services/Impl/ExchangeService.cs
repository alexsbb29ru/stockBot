using BaseTypes;
using Exceptions;
using SecuritiesEvaluation;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Services.Impl
{
    public class ExchangeService : BaseController, IExchangeService
    {
        /// <summary>
        /// Get tickers evaluation data
        /// </summary>
        /// <param name="tikers">List of tikers</param>
        /// <returns></returns>
        public List<EvaluationCriteria> GetEvaluation(IList<string> tikers)
        {
            try
            {
                if (!tikers.Any())
                    return new List<EvaluationCriteria>();

                var cultureInfo = CultureInfo.CurrentCulture;
                var dates = GetDates();

                List<EvaluationCriteria> evaluationList =
                    tikers.Select(item => EvaluateSecurities(item.ToLower(cultureInfo), dates.startDate, dates.endDate)).ToList();

                Logger.Information($"Получение данных риска / доходности по тикерам {nameof(GetEvaluation)}");

                return evaluationList;
            }
            catch (Exception e)
            {
                var message = e.InnerException?.Message ?? e.Message;
                Logger.Error($"Ошибка оценочных данных тикеров. Метод {nameof(GetEvaluation)} \n\r" +
                             $"{message}");
                return new List<EvaluationCriteria>();
            }
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

                Logger.Information($"Получение хреновых акций в методе {nameof(GetExceptionList)}");
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
                Logger.Information($"Получение оптимальных долей в методе {nameof(GetOptimalSecurities)}");
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
                Logger.Information($"Получение самой слабой акции в методе {nameof(GetWeakerStock)}");
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
            try
            {
                var dates = GetDates();
                var indicator = EvaluationMethods.MADEvaluateSecurities(exchangeName, dates.startDate, dates.endDate);

                Logger.Information($"Получение индикатора в методе {nameof(GetIndicator)}: {indicator.Tiker}");
                return indicator;
            }
            catch (Exception e)
            {
                var message = e.InnerException?.Message ?? e.Message;
                Logger.Error($"Ошибка получения индикатора ({exchangeName}. Метод {nameof(GetIndicator)} \n\r" +
                             $"{message}");
                return new EvaluationCriteria(exchangeName + "error", 0, 0, 0, 0);
                // return default;
            }
        }

        /// <summary>
        /// Get Russian stocks
        /// </summary>
        /// <param name="tikers">Tikers list</param>
        /// <param name="lang"></param>
        /// <returns>Summary list with russian stocks</returns>
        public IEnumerable<string> GetRussianStocks(string tikers, string lang = "en")
        {
            try
            {
                if (string.IsNullOrEmpty(tikers))
                    return default;
                
                Logger.Information($"Получение русских тикеров {nameof(GetRussianStocks)}");

                var tikersList = tikers.ToLower(CultureInfo.GetCultureInfo(lang)).Trim().Split(' ').Distinct()
                    .Where(x => !string.IsNullOrEmpty(x)).ToArray();

                var russianStocks = EvaluationMethods.GetSecuritiesPostfix(tikersList).ToList();
                return russianStocks;
            }
            catch (Exception e)
            {
                var message = e.InnerException?.Message ?? e.Message;
                Logger.Error($"Ошибка получения русских тикеров {tikers}. Метод {nameof(GetRussianStocks)} \n\r" +
                             $"{message}");
                return new List<string>();
            }
        }
        /// <summary>
        /// Get tickers valuation start date and end date
        /// </summary>
        /// <returns></returns>
        private (DateTime startDate, DateTime endDate) GetDates()
        {
            var startDate = new DateTime(2007, 1, 1);

            var endYear = DateTime.Now.Year;
            var endMonth = DateTime.Now.Month == 1 ? 1 : DateTime.Now.Month - 1;
            var endDay = DateTime.DaysInMonth(endYear, endMonth);
            var endDate = new DateTime(endYear, endMonth, endDay);

            Logger.Information($"Формирование даты в методе {nameof(GetDates)}");

            return (startDate, endDate);
        }
        /// <summary>
        /// Get evaluation data for each tiker from tikers list
        /// </summary>
        /// <param name="tiker">Tiker from tikers list</param>
        /// <param name="startDate">Ticker valuation start date</param>
        /// <param name="endDate">Ticker valuation end date</param>
        /// <returns></returns>
        private EvaluationCriteria EvaluateSecurities(string tiker, DateTime startDate, DateTime endDate)
        {
            try
            {
                Logger.Information($"Получение данных по тикеру {tiker}. Метод {nameof(EvaluateSecurities)}");

                var securities = tiker;
                var evalCriteria = EvaluationMethods.MADEvaluateSecurities(securities, startDate, endDate);

                return evalCriteria;
            }
            catch (Exception e)
            {
                var message = e.InnerException?.Message ?? e.Message;
                Logger.Error($"Ошибка получения данных по тикеру {tiker}. Метод {nameof(EvaluateSecurities)} \n\r" +
                             $"{message}");
                return new EvaluationCriteria(tiker + "error", 0, 0, 0, 0);
            }
        }
    }
}