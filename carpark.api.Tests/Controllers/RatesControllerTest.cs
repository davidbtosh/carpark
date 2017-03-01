using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using carpark.api;
using carpark.api.Controllers;
using carpark.api.Models;

namespace carpark.api.Tests.Controllers
{
    [TestClass]
    public class RatesControllerTest
    {
        [TestMethod]
        public void NightRateTestPositive1()
        {
            // Arrange
            RatesController controller = new RatesController();

            // Act
            //this date is a wednesday so weekend rate not applicable
            //entry is 6PM so early bird not applicable
            //6PM entry and 11 PM exit on weekday should use Night rate

            DateTime entry = DateTime.Parse("2017-03-01 18:00:00");
            DateTime exit = DateTime.Parse("2017-03-01 23:00:00");
            Rate result = controller.CalculateRates(entry, exit);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Night Rate", result.RateName);
            Assert.AreEqual("Flat Rate", result.RateType);
            Assert.AreEqual(6.50M, result.RatePrice);
        }

        [TestMethod]
        public void NightRateTestPositive2()
        {
            // Arrange
            RatesController controller = new RatesController();

            // Act
            //this date is a wednesday so weekend rate not applicable
            //entry is 6PM so early bird not applicable
            //6PM entry and 5AM exit (next day) on weekday should use Night rate

            DateTime entry = DateTime.Parse("2017-03-01 18:00:00");
            DateTime exit = DateTime.Parse("2017-03-02 05:00:00");
            Rate result = controller.CalculateRates(entry, exit);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Night Rate", result.RateName);
            Assert.AreEqual("Flat Rate", result.RateType);
            Assert.AreEqual(6.50M, result.RatePrice);
        }

        [TestMethod]
        public void NightRateTestNegative1()
        {
            // Arrange
            RatesController controller = new RatesController();

            // Act
            //this date is a wednesday so weekend rate not applicable
            //entry is 5PM so early bird not applicable
            //5PM entry is too early for night rate
            //will use hourly rate
            DateTime entry = DateTime.Parse("2017-03-01 17:00:00");
            DateTime exit = DateTime.Parse("2017-03-01 23:00:00");
            Rate result = controller.CalculateRates(entry, exit);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Hourly Rate", result.RateName);
            Assert.AreEqual("Flat Rate", result.RateType);
            Assert.AreEqual(6.50M, result.RatePrice);
        }


        [TestMethod]
        public void EarlyBirdTest()
        {
            // Arrange
            RatesController controller = new RatesController();

            // Act
            //this date is a wednesday so weekend rate not applicable
            //entry is 6PM so early bird not applicable
            //6PM entry and 11 PM exit on weekday should use Night rate

            DateTime entry = DateTime.Parse("2017-03-01 18:00:00");
            DateTime exit = DateTime.Parse("2017-03-01 23:00:00");
            Rate result = controller.CalculateRates(entry, exit);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Night Rate", result.RateName);
            Assert.AreEqual("Flat Rate", result.RateType);
            Assert.AreEqual(6.50M, result.RatePrice);
        }
    }
}
