using Serilog;
using AutoMapper;
using Models.ViewModels;
using SecuritiesEvaluation;

namespace BaseTypes
{
    public class BaseController
    {
        //Logger initialization
        protected readonly ILogger Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        
        protected readonly Mapper MapServ = new Mapper(
            new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EvaluationCriteriaVm, EvaluationCriteria>();
                cfg.CreateMap<EvaluationCriteria, EvaluationCriteriaVm>();
            }));
    }
}

