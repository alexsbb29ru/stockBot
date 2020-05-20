using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecuritiesEvaluation;
using Services.Impl;

namespace ServicesTest
{
    [TestClass]
    public class ExchangeServiceTest
    {
        [TestMethod]
        public void Tickers_null_ListEvalReturned()
        {
            //Arrange
            var expected = new List<EvaluationCriteria>().Count;    
            //Act
            var actual = new ExchangeService().GetEvaluation(null).Count;
            //Assert
            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod]
        public void Tickers_empty_ListEvalReturned()
        {
            //Arrange
            var expected = new List<EvaluationCriteria>().Count;
            var input = new List<string>();
            //Act
            var actual = new ExchangeService().GetEvaluation(input).Count;
            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}