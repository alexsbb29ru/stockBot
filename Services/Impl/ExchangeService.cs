using BaseTypes;
using Models;
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
            
            List<EvaluationCriteriaModel> evaluationList = new List<EvaluationCriteriaModel>();

            foreach(var item in tikersArr)
            {
                evaluationList.Add(EvaluateSecurities(item));
            }

            var resultMessage = "Company: Risk - Earnings";

            foreach(var item in evaluationList)
            {
                if (!item.Tiker.ToLower().Contains("error"))
                    resultMessage += $"\n\r{item.Tiker}: {item.Risk.ToString("F2")} - {item.Earnings.ToString("F2")}";
                else
                    resultMessage += $"\n\r{item.Tiker.Replace("error", "")} is wrong tiker";
            }

            return resultMessage;
        }

        private EvaluationCriteriaModel EvaluateSecurities(string tiker)
        {
            try
            {
                _logger.Information($"Получение данных по тикеру {tiker}. Метод {nameof(EvaluateSecurities)}");
                //string securities = string.Concat(tiker, ".ME");
                string securities = tiker;

                List<Candle> candleList = new List<Candle>();
                List<DividendTick> dividendList = new List<DividendTick>();

                Task<IReadOnlyList<Candle>> history = Yahoo.GetHistoricalAsync(securities, new DateTime(2015, 1, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), Period.Monthly);
                history.Wait();

                candleList.AddRange(history.Result);

                Task<IReadOnlyList<DividendTick>> dividends = Yahoo.GetDividendsAsync(securities, new DateTime(2015, 1, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                dividends.Wait();

                dividendList.AddRange(dividends.Result);

                List<decimal> earningsList = new List<decimal>();

                for (int year = 2015; year < DateTime.Now.Year; year++)
                {
                    //Получить цены начала и конца года.
                    Candle first = candleList.First(item => item.DateTime.Year == year);
                    Candle last = candleList.Last(item => item.DateTime.Year == year);

                    //Получить сумму дивиденда за год.
                    decimal dividend = dividendList.Where(item => item.DateTime.Year == year).Sum(item => item.Dividend);

                    //Вычислить доходность за год.
                    decimal earnings = (dividend + (last.Close - first.Close)) / first.Close * 100;
                    earningsList.Add(earnings);
                }

                decimal avgEarnings = earningsList.Average();

                //Вычислить стандартное отклонение. Чем выше его значение, тем выше риск, связанный с инвестированием в этот актив, и наоборот.
                decimal sqrDeviation = earningsList.Sum(item => (decimal)Math.Pow(Convert.ToDouble(item - avgEarnings), 2));
                decimal stndDeviation = (decimal)Math.Sqrt(Convert.ToDouble(sqrDeviation / 4));

                return new EvaluationCriteriaModel(tiker, stndDeviation, avgEarnings);
            }
            catch (Exception e)
            {
                var message = e.InnerException.Message ?? e.Message;
                _logger.Error($"Ошибка получения данных по тикеру {tiker}. Метод {nameof(EvaluateSecurities)} \n\r" +
                    $"{message}");
                return new EvaluationCriteriaModel(tiker + "error", 0, 0);
            }
        }
    }
}
