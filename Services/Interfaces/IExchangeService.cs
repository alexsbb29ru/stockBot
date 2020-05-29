using SecuritiesEvaluation;
using System.Collections.Generic;
using Models.ViewModels;

namespace Services.Interfaces
{
    public interface IExchangeService
    {
        List<EvaluationCriteriaVm> GetEvaluation(IList<string> tikers);
        List<EvaluationCriteria> GetExceptionList(List<EvaluationCriteria> evalList, string indicatorName);
        List<EvaluationCriteria> GetOptimalSecurities(double earningLevel, List<EvaluationCriteria> evalList);
        EvaluationCriteriaVm GetIndicator(string exchangeName);
        EvaluationCriteria GetWeakerStock(List<EvaluationCriteria> evalList);
        IEnumerable<string> GetRussianStocks(string tikers, string lang = "en");
        double GetMedian(IEnumerable<double> earnings);
    }
}
