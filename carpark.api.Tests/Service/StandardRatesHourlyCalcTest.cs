using carpark.api.Models;
using carpark.api.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace carpark.api.Tests.Service
{
    [TestClass]
    public class StandardRatesHourlyCalcTest
    {
        private UserData _ud;
        private RatesCalculator _calculator;

        public StandardRatesHourlyCalcTest()
        {
            _ud = new UserData();
            _calculator = new RatesCalculator();
            _calculator.FilePath = Path.GetFullPath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\..\..\..\carpark.api\App_Data\");
        }

        [TestMethod]
        public void RatesCalculatorHourlyTest1()
        {
            //weekday - no weekend
            //ud.Entry is 9.30 - outside early bird & night
            //ud.Exit 10.00 - < 1 hour
            //Hourly rate $5
            _ud.Entry = DateTime.Parse("2017-03-01 09:30:00");
            _ud.Exit = DateTime.Parse("2017-03-01 10:00:00");

            Rate flat = _calculator.CalculateFlatRate(_ud);
            Assert.IsNull(flat);

            Rate result = _calculator.CalculateHourlyRate(_ud);                 

            Assert.AreEqual("Standard Rate", result.RateName);
            Assert.AreEqual("Hourly Rate", result.RateType);
            Assert.AreEqual(5M, result.RatePrice);
        }

        [TestMethod]
        public void RatesCalculatorHourlyTest2()
        {
            //weekday - no weekend
            //ud.Entry is 9.30 - outside early bird & night
            //ud.Exit 10.30 - exactly 1 hour
            //Hourly rate $5
            _ud.Entry = DateTime.Parse("2017-03-01 09:30:00");
            _ud.Exit = DateTime.Parse("2017-03-01 10:30:00");

            Rate flat = _calculator.CalculateFlatRate(_ud);
            Assert.IsNull(flat);

            Rate result = _calculator.CalculateHourlyRate(_ud);

            Assert.AreEqual("Standard Rate", result.RateName);
            Assert.AreEqual("Hourly Rate", result.RateType);
            Assert.AreEqual(5M, result.RatePrice);
        }

        [TestMethod]
        public void RatesCalculatorHourlyTest3()
        {
            //weekday - no weekend
            //ud.Entry is 9.30 - outside early bird & night
            //ud.Exit 11.00 - < 2 hours > 1 hour
            //Hourly rate $10
            _ud.Entry = DateTime.Parse("2017-03-01 09:30:00");
            _ud.Exit = DateTime.Parse("2017-03-01 11:00:00");

            Rate flat = _calculator.CalculateFlatRate(_ud);
            Assert.IsNull(flat);

            Rate result = _calculator.CalculateHourlyRate(_ud);

            Assert.AreEqual("Standard Rate", result.RateName);
            Assert.AreEqual("Hourly Rate", result.RateType);
            Assert.AreEqual(10M, result.RatePrice);
        }

        [TestMethod]
        public void RatesCalculatorHourlyTest4()
        {
            //weekday - no weekend
            //ud.Entry is 9.30 - outside early bird & night
            //ud.Exit 11.30  = 2 hours
            //Hourly rate $10
            _ud.Entry = DateTime.Parse("2017-03-01 09:30:00");
            _ud.Exit = DateTime.Parse("2017-03-01 11:30:00");

            Rate flat = _calculator.CalculateFlatRate(_ud);
            Assert.IsNull(flat);

            Rate result = _calculator.CalculateHourlyRate(_ud);

            Assert.AreEqual("Standard Rate", result.RateName);
            Assert.AreEqual("Hourly Rate", result.RateType);
            Assert.AreEqual(10M, result.RatePrice);
        }

        [TestMethod]
        public void RatesCalculatorHourlyTest5()
        {
            //weekday - no weekend
            //ud.Entry is 9.30 - outside early bird & night
            //ud.Exit 11.31 - < 3 hours > 2 hours
            //Hourly rate $15
            _ud.Entry = DateTime.Parse("2017-03-01 09:30:00");
            _ud.Exit = DateTime.Parse("2017-03-01 12:00:00");

            Rate flat = _calculator.CalculateFlatRate(_ud);
            Assert.IsNull(flat);

            Rate result = _calculator.CalculateHourlyRate(_ud);

            Assert.AreEqual("Standard Rate", result.RateName);
            Assert.AreEqual("Hourly Rate", result.RateType);
            Assert.AreEqual(15M, result.RatePrice);
        }

        [TestMethod]
        public void RatesCalculatorHourlyTest6()
        {
            //weekday - no weekend
            //ud.Entry is 9.30 - outside early bird & night
            //ud.Exit 12.30  = 3 hours
            //Hourly rate $15
            _ud.Entry = DateTime.Parse("2017-03-01 09:30:00");
            _ud.Exit = DateTime.Parse("2017-03-01 12:30:00");

            Rate flat = _calculator.CalculateFlatRate(_ud);
            Assert.IsNull(flat);

            Rate result = _calculator.CalculateHourlyRate(_ud);

            Assert.AreEqual("Standard Rate", result.RateName);
            Assert.AreEqual("Hourly Rate", result.RateType);
            Assert.AreEqual(15M, result.RatePrice);
        }

        [TestMethod]
        public void RatesCalculatorHourlyTest7()
        {
            //weekday - no weekend
            //ud.Entry is 9.30 - outside early bird & night
            //ud.Exit 12.35  > 3 hours
            //Hourly rate $20
            _ud.Entry = DateTime.Parse("2017-03-01 09:30:00");
            _ud.Exit = DateTime.Parse("2017-03-01 12:35:00");

            Rate flat = _calculator.CalculateFlatRate(_ud);
            Assert.IsNull(flat);

            Rate result = _calculator.CalculateHourlyRate(_ud);

            Assert.AreEqual("Standard Rate", result.RateName);
            Assert.AreEqual("Hourly Rate", result.RateType);
            Assert.AreEqual(20M, result.RatePrice);
        }

        [TestMethod]
        public void RatesCalculatorHourlyTest8()
        {
            //weekday - no weekend
            //ud.Entry is 9.30 - outside early bird & night
            //ud.Exit 09.30  2 days later
            //Hourly rate $20 * 2 = $40
            _ud.Entry = DateTime.Parse("2017-03-01 09:30:00");
            _ud.Exit = DateTime.Parse("2017-03-03 09:30:00");

            Rate flat = _calculator.CalculateFlatRate(_ud);
            Assert.IsNull(flat);

            Rate result = _calculator.CalculateHourlyRate(_ud);

            Assert.AreEqual("Standard Rate", result.RateName);
            Assert.AreEqual("Hourly Rate", result.RateType);
            Assert.AreEqual(40M, result.RatePrice);
        }

        [TestMethod]
        public void RatesCalculatorHourlyTest9()
        {
            //entry weekday - weekend not applicable
            //ud.Entry is 9.30 - outside early bird & night
            //ud.Exit 11.30  7 days later
            //Hourly rate $20 * 7 = $140
            _ud.Entry = DateTime.Parse("2017-03-01 09:30:00");
            _ud.Exit = DateTime.Parse("2017-03-08 11:30:00");

            Rate flat = _calculator.CalculateFlatRate(_ud);
            Assert.IsNull(flat);

            Rate result = _calculator.CalculateHourlyRate(_ud);

            Assert.AreEqual("Standard Rate", result.RateName);
            Assert.AreEqual("Hourly Rate", result.RateType);
            Assert.AreEqual(140M, result.RatePrice);
        }

    }
}
