using BaseTypes;
using Models;
using SecuritiesEvaluation;
using Services.Interfaces;
using System;
using System.Collections.Generic;
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
            var tikersArr = tikers.ToLower().Trim().Split(' ').Distinct();
            
            List<EvaluationCriteria> evaluationList = new List<EvaluationCriteria>();

            foreach(var item in tikersArr)
            {
                evaluationList.Add(EvaluateSecurities(item));
            }

            var resultMessage = "Company: Risk | Earnings | CV | Period";

            foreach(var item in evaluationList)
            {
                if (!item.Tiker.ToLower().Contains("error"))
                    resultMessage += $"\n\r{item.Tiker}: {item.Risk.ToString("F2")} | {item.Earnings.ToString("F2")}" +
                        $" | {(item.Risk / item.Earnings)} | {DateTime.Now.Year - 5} - {DateTime.Now.Year}";
                else
                    resultMessage += $"\n\r{item.Tiker.Replace("error", "")} is wrong tiker";
            }

            return resultMessage;
        }

        private EvaluationCriteria EvaluateSecurities(string tiker)
        {
            try
            {
                _logger.Information($"Получение данных по тикеру {tiker}. Метод {nameof(EvaluateSecurities)}");
                //string securities = string.Concat(tiker, ".ME");
                string securities = tiker;

                var startYear = DateTime.Now.Year - 5;
                var startDate = new DateTime(startYear, 1, 1);

                var endYear = DateTime.Now.Year;
                var endMonth = DateTime.Now.Month == 1 ? 1: DateTime.Now.Month -1 ;
                var endDay = DateTime.DaysInMonth(endYear, endMonth);
                var endDate = new DateTime(endYear, endMonth, endDay);

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
