using carpark.api.Models;
using carpark.api.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace carpark.api.Tests.Service
{
    [TestClass]
    public class FixedRatesNightCalcTest
    {
        private UserData _ud;
        private RatesCalculator _calculator;

        public FixedRatesNightCalcTest()
        {
            _ud = new UserData();
            _calculator = new RatesCalculator();
            _calculator.FilePath = Path.GetFullPath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\..\..\..\carpark.api\App_Data\");
        }

        [TestMethod]
        public void RatesCalculatorNightRateTestPositive1()
        {
            
            // Act
            //weekday 
            //ud.Entry is 6PM 
            //6PM ud.Entry and 11PM exit
            //Night rate

            _ud.Entry = DateTime.Parse("2017-03-01 18:00:00");
            _ud.Exit = DateTime.Parse("2017-03-01 23:00:00");
            Rate result = _calculator.CalculateFlatRate(_ud);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Night Rate", result.RateName);
            Assert.AreEqual("Flat Rate", result.RateType);
            Assert.AreEqual(6.50M, result.RatePrice);
        }

        [TestMethod]
        public void RatesCalculatorNightRateTestPositive2()
        {
            //weekday 
            //ud.Entry is 6PM 
            //6PM ud.Entry and 5AM exit (next day) 
            //Night rate

            _ud.Entry = DateTime.Parse("2017-03-01 18:00:00");
            _ud.Exit = DateTime.Parse("2017-03-02 05:00:00");
            Rate result = _calculator.CalculateFlatRate(_ud);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Night Rate", result.RateName);
            Assert.AreEqual("Flat Rate", result.RateType);
            Assert.AreEqual(6.50M, result.RatePrice);
        }

        [TestMethod]
        public void RatesCalculatorNightRateTestNegative1()
        {            
            // Act
            //weekday 
            //ud.Entry is 5PM 
            //will use hourly rate
            _ud.Entry = DateTime.Parse("2017-03-01 17:00:00");
            _ud.Exit = DateTime.Parse("2017-03-01 23:00:00");
            Rate result = _calculator.CalculateFlatRate(_ud);

            // Assert
            Assert.IsNull(result);
            
        }
        
    }
}
