using carpark.api.Models;
using carpark.api.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace carpark.api.Tests.Service
{
    [TestClass]
    public class RatesCalculatorTest
    {
        private UserData ud = new UserData();

        [TestMethod]
        public void RatesCalculatorWeekendTestPositive1()
        {
            // Arrange
            RatesCalculator calculator = new RatesCalculator();
            
            // Act
            //weekend 
            //ud.Entry is midnight sat morn 
            //exit 11.59 sun night
            //Weekend rate

            ud.Entry = DateTime.Parse("2017-03-04 00:00:00");
            ud.Exit = DateTime.Parse("2017-03-05 23:59:00");

            Rate result = calculator.CalculateFlatRate(ud);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Weekend Rate", result.RateName);
            Assert.AreEqual("Flat Rate", result.RateType);
            Assert.AreEqual(10M, result.RatePrice);
        }

        [TestMethod]
        public void RatesCalculatorWeekendTestNeg1()
        {
            // Arrange
            RatesCalculator calculator = new RatesCalculator();

            // Act
            //entry friday 11.59 
           
            //exit 11.59 sun night
            //not weekend
            ud.Entry = DateTime.Parse("2017-03-03 23:59:00");
            ud.Exit = DateTime.Parse("2017-03-05 23:59:00");

            Rate result = calculator.CalculateFlatRate(ud);

            // Assert
            Assert.IsNull(result);
            
        }

        [TestMethod]
        public void RatesCalculatorWeekendTestNeg2()
        {
            // Arrange
            RatesCalculator calculator = new RatesCalculator();

            // Act
            //entry friday 11.59 

            //exit 5.59 sat morn
            //not weekend but is night rate
            ud.Entry = DateTime.Parse("2017-03-03 23:59:00");
            ud.Exit = DateTime.Parse("2017-03-04 05:59:00");

            Rate result = calculator.CalculateFlatRate(ud);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Night Rate", result.RateName);
            Assert.AreEqual("Flat Rate", result.RateType);
            Assert.AreEqual(6.5M, result.RatePrice);

        }

        [TestMethod]
        public void RatesCalculatorWeekendTestNeg3()
        {
            // Arrange
            RatesCalculator calculator = new RatesCalculator();

            // Act
            //entry sat midnight 

            //exit mon midnight - left 1 min too late for weekend rate
            //not weekend
            ud.Entry = DateTime.Parse("2017-03-04 00:00:00");
            ud.Exit = DateTime.Parse("2017-03-06 00:00:00");

            Rate result = calculator.CalculateFlatRate(ud);

            // Assert
            Assert.IsNull(result);

        }


        [TestMethod]
        public void RatesCalculatorNightRateTestPositive1()
        {
            // Arrange
            RatesCalculator calculator = new RatesCalculator();

            // Act
            //weekday 
            //ud.Entry is 6PM 
            //6PM ud.Entry and 11PM exit
            //Night rate

            ud.Entry = DateTime.Parse("2017-03-01 18:00:00");
            ud.Exit = DateTime.Parse("2017-03-01 23:00:00");
            Rate result = calculator.CalculateFlatRate(ud);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Night Rate", result.RateName);
            Assert.AreEqual("Flat Rate", result.RateType);
            Assert.AreEqual(6.50M, result.RatePrice);
        }

        [TestMethod]
        public void RatesCalculatorNightRateTestPositive2()
        {
            // Arrange
            RatesCalculator calculator = new RatesCalculator();

            // Act
            //weekday 
            //ud.Entry is 6PM 
            //6PM ud.Entry and 5AM exit (next day) 
            //Night rate

            ud.Entry = DateTime.Parse("2017-03-01 18:00:00");
            ud.Exit = DateTime.Parse("2017-03-02 05:00:00");
            Rate result = calculator.CalculateFlatRate(ud);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Night Rate", result.RateName);
            Assert.AreEqual("Flat Rate", result.RateType);
            Assert.AreEqual(6.50M, result.RatePrice);
        }

        [TestMethod]
        public void RatesCalculatorNightRateTestNegative1()
        {
            // Arrange
            RatesCalculator calculator = new RatesCalculator();

            // Act
            //weekday 
            //ud.Entry is 5PM 
            //will use hourly rate
            ud.Entry = DateTime.Parse("2017-03-01 17:00:00");
            ud.Exit = DateTime.Parse("2017-03-01 23:00:00");
            Rate result = calculator.CalculateFlatRate(ud);

            // Assert
            Assert.IsNull(result);
            
        }


        [TestMethod]
        public void RatesCalculatorEarlyBirdTestPositive1()
        {
            // Arrange
            RatesCalculator calculator = new RatesCalculator();

            // Act
            //this date is a wednesday so weekend rate not applicable
            //ud.Entry is 6am 
            //exit 3.30PM
            //Early bird

            ud.Entry = DateTime.Parse("2017-03-01 06:00:00");
            ud.Exit = DateTime.Parse("2017-03-01 15:30:00");
            Rate result = calculator.CalculateFlatRate(ud);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Early Bird", result.RateName);
            Assert.AreEqual("Flat Rate", result.RateType);
            Assert.AreEqual(13M, result.RatePrice);
        }

        [TestMethod]
        public void RatesCalculatorEarlyBirdTestNeg1()
        {
            // Arrange
            RatesCalculator calculator = new RatesCalculator();

            // Act
            //weekday
            //ud.Entry is 5.59 am 
            //exit 3.30PM
            //ud.Entry minute too early for early bird
            //not early bird

            ud.Entry = DateTime.Parse("2017-03-01 05:59:00");
            ud.Exit = DateTime.Parse("2017-03-01 15:30:00");
            Rate result = calculator.CalculateFlatRate(ud);

            // Assert
            Assert.IsNull(result);
            
        }

        [TestMethod]
        public void RatesCalculatorEarlyBirdTestNeg2()
        {
            // Arrange
            RatesCalculator calculator = new RatesCalculator();

            // Act
            //weekday
            //ud.Entry is 6.00 am 
            //exit 11.31PM
            //exit minute too late for early bird
            //not early bird

            ud.Entry = DateTime.Parse("2017-03-01 06:00:00");
            ud.Exit = DateTime.Parse("2017-03-01 23:31:00");
            Rate result = calculator.CalculateFlatRate(ud);

            // Assert
            Assert.IsNull(result);
            
        }

        [TestMethod]
        public void RatesCalculatorEarlyBirdTestNeg3()
        {
            // Arrange
            RatesCalculator calculator = new RatesCalculator();

            // Act
            //weekday
            //ud.Entry is 9.01 am 
            //exit 3.30PM
            //ud.Entry minute too late for early bird
            //not early bird

            ud.Entry = DateTime.Parse("2017-03-01 09:01:00");
            ud.Exit = DateTime.Parse("2017-03-01 15:30:00");
            Rate result = calculator.CalculateFlatRate(ud);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void RatesCalculatorEarlyBirdTestNeg4()
        {
            // Arrange
            RatesCalculator calculator = new RatesCalculator();

            // Act
            //weekday
            //ud.Entry is 6.00 am 
            //exit 03.29PM
            //exit minute too early for early bird
            //not early bird

            ud.Entry = DateTime.Parse("2017-03-01 06:00:00");
            ud.Exit = DateTime.Parse("2017-03-01 15:29:00");
            Rate result = calculator.CalculateFlatRate(ud);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void RatesCalculatorEarlyBirdTestNeg5()
        {
            // Arrange
            RatesCalculator calculator = new RatesCalculator();

            // Act
            //weekend
            //ud.Entry is 6am 
            //exit 11.30PM
            //times good for early bird but weekrate takes precedence
            //weekend rate

            ud.Entry = DateTime.Parse("2017-03-04 06:00:00");
            ud.Exit = DateTime.Parse("2017-03-04 23:31:00");
            Rate result = calculator.CalculateFlatRate(ud);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Weekend Rate", result.RateName);
            Assert.AreEqual("Flat Rate", result.RateType);
            Assert.AreEqual(10M, result.RatePrice);
        }

    }
}
