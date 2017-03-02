using carpark.api.Models;
using carpark.api.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace carpark.api.Tests.Service
{
    [TestClass]
    public class FixedRatesWeekendCalcTest
    {
        private UserData _ud;
        private RatesCalculator _calculator;

        public FixedRatesWeekendCalcTest()
        {
            _ud = new UserData();
            _calculator = new RatesCalculator();
            _calculator.FilePath = Path.GetFullPath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\..\..\..\carpark.api\App_Data\");
        }

        [TestMethod]
        public void RatesCalculatorWeekendTestPositive1()
        {            
            // Act
            //weekend 
            //ud.Entry is midnight sat morn 
            //exit 11.59 sun night
            //Weekend rate

            _ud.Entry = DateTime.Parse("2017-03-04 00:00:00");
            _ud.Exit = DateTime.Parse("2017-03-05 23:59:00");

            Rate result = _calculator.CalculateFlatRate(_ud);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Weekend Rate", result.RateName);
            Assert.AreEqual("Flat Rate", result.RateType);
            Assert.AreEqual(10M, result.RatePrice);
        }

        [TestMethod]
        public void RatesCalculatorWeekendTestNeg1()
        {            
            //entry friday 11.59            
            //exit 11.59 sun night
            //not weekend
            _ud.Entry = DateTime.Parse("2017-03-03 23:59:00");
            _ud.Exit = DateTime.Parse("2017-03-05 23:59:00");

            Rate result = _calculator.CalculateFlatRate(_ud);

            // Assert
            Assert.IsNull(result);
            
        }

        [TestMethod]
        public void RatesCalculatorWeekendTestNeg2()
        {            
            //entry friday 11.59 
            //exit 5.59 sat morn
            //not weekend but is night rate
            _ud.Entry = DateTime.Parse("2017-03-03 23:59:00");
            _ud.Exit = DateTime.Parse("2017-03-04 05:59:00");

            Rate result = _calculator.CalculateFlatRate(_ud);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Night Rate", result.RateName);
            Assert.AreEqual("Flat Rate", result.RateType);
            Assert.AreEqual(6.5M, result.RatePrice);

        }

        [TestMethod]
        public void RatesCalculatorWeekendTestNeg3()
        {            
            //entry sat midnight 
            //exit mon midnight - left 1 min too late for weekend rate
            //not weekend
            _ud.Entry = DateTime.Parse("2017-03-04 00:00:00");
            _ud.Exit = DateTime.Parse("2017-03-06 00:00:00");

            Rate result = _calculator.CalculateFlatRate(_ud);

            // Assert
            Assert.IsNull(result);

        }        

    }
}
