namespace Models.ViewModels
{
    public class EvaluationCriteriaVm
    {
        public string Tiker { get; set; }
        public double Risk { get; set;}
        public double Earnings { get; set;}
        public double Deviation { get; set;}
        public double Weight { get; set;}
        
        public string ErrorMessage { get; set;}
        
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

        public EvaluationCriteriaVm()
        {
                
        }
    }
}