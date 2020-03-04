using BaseTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class EvaluationCriteriaModel : BaseController
    {
        private string _tiker;
        private decimal _risk;
        private decimal _earnings;

        public string Tiker
        {
            get => _tiker;
            set => SetValue(ref _tiker, value, nameof(Tiker));
        }

        public decimal Risk
        {
            get => _risk;
            set => SetValue(ref _risk, value, nameof(Risk));
        }

        public decimal Earnings
        {
            get => _earnings;
            set => SetValue(ref _earnings, value, nameof(_earnings));
        }

        public EvaluationCriteriaModel(string tiker, decimal risk, decimal earnings)
        {
            _tiker = tiker;
            _risk = risk;
            _earnings = earnings;
        }


    }
}
