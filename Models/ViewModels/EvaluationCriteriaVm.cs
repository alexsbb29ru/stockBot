namespace Models.ViewModels
{
    public class EvaluationCriteriaVm
    {
        public string Tiker { get; }
        public double Risk { get; }
        public double Earnings { get; }
        public double Deviation { get; }
        public double Weight { get; }
        
        public string ErrorMessage { get; }
        
        public EvaluationCriteriaVm(
            string tiker,
            double risk,
            double earnings,
            double deviation,
            double weight,
            string errorMessage)
        {
            Tiker = tiker;
            Risk = risk;
            Earnings = earnings;
            Deviation = deviation;
            Weight = weight;
            ErrorMessage = errorMessage;
        }
    }
}