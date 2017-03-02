using carpark.api.Models;
using carpark.api.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace carpark.api.Tests.Service
{
    [TestClass]
    public class FixedRatesEarlyBirdCalcTest
    {
        private UserData ud;
        private RatesCalculator _calculator;

        public FixedRatesEarlyBirdCalcTest()
        {
            ud = new UserData();
            _calculator = new RatesCalculator();
            _calculator.FilePath = Path.GetFullPath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\..\..\..\carpark.api\App_Data\");
        }

       
        [TestMethod]
        public void RatesCalculatorEarlyBirdTestPositive1()
        {           
            // Act
            //this date is a wednesday so weekend rate not applicable
            //ud.Entry is 6am 
            //exit 3.30PM
            //Early bird

            ud.Entry = DateTime.Parse("2017-03-01 06:00:00");
            ud.Exit = DateTime.Parse("2017-03-01 15:30:00");
            Rate result = _calculator.CalculateFlatRate(ud);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Early Bird", result.RateName);
            Assert.AreEqual("Flat Rate", result.RateType);
            Assert.AreEqual(13M, result.RatePrice);
        }

        [TestMethod]
        public void RatesCalculatorEarlyBirdTestNeg1()
        {
          
            // Act
            //weekday
            //ud.Entry is 5.59 am 
            //exit 3.30PM
            //ud.Entry minute too early for early bird
            //not early bird

            ud.Entry = DateTime.Parse("2017-03-01 05:59:00");
            ud.Exit = DateTime.Parse("2017-03-01 15:30:00");
            Rate result = _calculator.CalculateFlatRate(ud);

            // Assert
            Assert.IsNull(result);
            
        }

        [TestMethod]
        public void RatesCalculatorEarlyBirdTestNeg2()
        {
            // Act
            //weekday
            //ud.Entry is 6.00 am 
            //exit 11.31PM
            //exit minute too late for early bird
            //not early bird

            ud.Entry = DateTime.Parse("2017-03-01 06:00:00");
            ud.Exit = DateTime.Parse("2017-03-01 23:31:00");
            Rate result = _calculator.CalculateFlatRate(ud);

            // Assert
            Assert.IsNull(result);
            
        }

        [TestMethod]
        public void RatesCalculatorEarlyBirdTestNeg3()
        {
            
            // Act
            //weekday
            //ud.Entry is 9.01 am 
            //exit 3.30PM
            //ud.Entry minute too late for early bird
            //not early bird

            ud.Entry = DateTime.Parse("2017-03-01 09:01:00");
            ud.Exit = DateTime.Parse("2017-03-01 15:30:00");
            Rate result = _calculator.CalculateFlatRate(ud);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void RatesCalculatorEarlyBirdTestNeg4()
        {
            
            // Act
            //weekday
            //ud.Entry is 6.00 am 
            //exit 03.29PM
            //exit minute too early for early bird
            //not early bird

            ud.Entry = DateTime.Parse("2017-03-01 06:00:00");
            ud.Exit = DateTime.Parse("2017-03-01 15:29:00");
            Rate result = _calculator.CalculateFlatRate(ud);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void RatesCalculatorEarlyBirdTestNeg5()
        {
            
            // Act
            //weekend
            //ud.Entry is 6am 
            //exit 11.30PM
            //times good for early bird but weekrate takes precedence
            //weekend rate

            ud.Entry = DateTime.Parse("2017-03-04 06:00:00");
            ud.Exit = DateTime.Parse("2017-03-04 23:31:00");
            Rate result = _calculator.CalculateFlatRate(ud);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Weekend Rate", result.RateName);
            Assert.AreEqual("Flat Rate", result.RateType);
            Assert.AreEqual(10M, result.RatePrice);
        }

    }
}
