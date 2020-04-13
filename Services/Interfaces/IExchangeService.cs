using SecuritiesEvaluation;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IExchangeService
    {
        List<EvaluationCriteria> GetEvaluation(IList<string> tikers);
        List<EvaluationCriteria> GetExceptionList(List<EvaluationCriteria> evalList, string indicatorName);
        List<EvaluationCriteria> GetOptimalSecurities(double earningLevel, List<EvaluationCriteria> evalList);
        EvaluationCriteria GetIndicator(string exchangeName);
        EvaluationCriteria GetWeakerStock(List<EvaluationCriteria> evalList);
        IEnumerable<string> GetRussianStocks(string tikers, string lang = "en");
    }
}
