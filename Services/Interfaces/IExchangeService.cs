using SecuritiesEvaluation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IExchangeService
    {
        List<EvaluationCriteria> GetEvaluation(string tikers);
        List<EvaluationCriteria> GetExceptionList(List<EvaluationCriteria> evalList);
        List<EvaluationCriteria> GetOptimalSecurities(double earningLevel, List<EvaluationCriteria> evalList);
        EvaluationCriteria GetWeakerStock(List<EvaluationCriteria> evalList);
    }
}
