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
using carpark.api.Services;
using Newtonsoft.Json;
using System.IO;

namespace carpark.api.Tests.Controllers
{
    [TestClass]
    public class RatesControllerTest
    {
        private RatesController _controller; 

        public RatesControllerTest()
        {
            var calculator = new RatesCalculator();
            calculator.FilePath = Path.GetFullPath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\..\..\..\carpark.api\App_Data\");

            _controller = new RatesController(calculator);
            _controller.Request = new HttpRequestMessage();
            _controller.Request.SetConfiguration(new HttpConfiguration());
        }

        [TestMethod]
        public void RatesControllerValidationTest1()
        {
            //exit after entry - bad request
            string entry = "2017-03-01 18:00:00";
            string exit = "2017-03-01 17:00:00";
            var response = _controller.CalculateRates(new UserUI(entry, exit));
            
            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);           
        }

        [TestMethod]
        public void RatesControllerValidationTest2()
        {
            //bad date - bad request
            string entry = "2017-13-01 18:00:00";
            string exit = "2017-03-01 17:00:00";
            var response = _controller.CalculateRates(new UserUI(entry, exit));

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void RatesControllerValidationTest3()
        {
            //bad date - bad request
            string entry = "2017-03-01 18:00:00";
            string exit = "2017-03-32 17:00:00";
            var response = _controller.CalculateRates(new UserUI(entry, exit));

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void RatesControllerValidationTest4()
        {
            //bad date - bad request
            string entry = "2017-03-01 25:00:00";
            string exit = "2017-03-01 17:00:00";
            var response = _controller.CalculateRates(new UserUI(entry, exit));

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void RatesControllerValidationTest5()
        {
            //bad date - bad request
            string entry = "2017-03-01 18:00:00";
            string exit = "2017-03-01 17:61:00";
            var response = _controller.CalculateRates(new UserUI(entry, exit));

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void RatesControllerValidationTest6()
        {
            //bad date - bad request
            string entry = "rubbish";
            string exit = "2017-03-01 17:55:00";
            var response = _controller.CalculateRates(new UserUI(entry, exit));

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void RatesControllerWeekendTestPositive1()
        {
                      
            // Act
            //weekend 
            //entry is midnight sat morn 
            //exit 11.59 sun night
            //Weekend rate

            string entry = "2017-03-04 00:00:00";
            string exit = "2017-03-05 23:59:00";
            var response = _controller.CalculateRates(new UserUI(entry, exit));

            var result = JsonConvert.DeserializeObject<Rate>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Weekend Rate", result.RateName);
            Assert.AreEqual("Flat Rate", result.RateType);
            Assert.AreEqual(10M, result.RatePrice);
        }


        [TestMethod]
        public void RatesControllerNightRateTestPositive1()
        {            
            // Act
            //weekday 
            //entry is 6PM 
            //6PM entry and 11PM exit
            //Night rate

            string entry = "2017-03-01 18:00:00";
            string exit = "2017-03-01 23:00:00";
            var response = _controller.CalculateRates(new UserUI(entry, exit));

            var result = JsonConvert.DeserializeObject<Rate>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Night Rate", result.RateName);
            Assert.AreEqual("Flat Rate", result.RateType);
            Assert.AreEqual(6.50M, result.RatePrice);
        }

        [TestMethod]
        public void RatesControllerNightRateTestPositive2()
        {            
            // Act
            //weekday 
            //entry is 6PM 
            //6PM entry and 5AM exit (next day) 
            //Night rate

            string entry = "2017-03-01 18:00:00";
            string exit = "2017-03-02 05:00:00";
            var response = _controller.CalculateRates(new UserUI(entry, exit));

            var result = JsonConvert.DeserializeObject<Rate>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Night Rate", result.RateName);
            Assert.AreEqual("Flat Rate", result.RateType);
            Assert.AreEqual(6.50M, result.RatePrice);
        }

        [TestMethod]
        public void RatesControllerNightRateTestNegative1()
        {            
            // Act
            //weekday 
            //entry is 5PM 
            //will use hourly rate
            string entry = "2017-03-01 17:00:00";
            string exit = "2017-03-01 23:00:00";
            var response = _controller.CalculateRates(new UserUI(entry, exit));

            var result = JsonConvert.DeserializeObject<Rate>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Standard Rate", result.RateName);
            Assert.AreEqual("Hourly Rate", result.RateType);
            Assert.AreEqual(20M, result.RatePrice);
        }


        [TestMethod]
        public void RatesControllerEarlyBirdTestPositive1()
        {
           
            // Act
            //this date is a wednesday so weekend rate not applicable
            //entry is 6am 
            //exit 3.30PM
            //Early bird

            string entry = "2017-03-01 06:00:00";
            string exit = "2017-03-01 15:30:00";
            var response = _controller.CalculateRates(new UserUI(entry, exit));

            var result = JsonConvert.DeserializeObject<Rate>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Early Bird", result.RateName);
            Assert.AreEqual("Flat Rate", result.RateType);
            Assert.AreEqual(13M, result.RatePrice);
        }

        [TestMethod]
        public void RatesControllerEarlyBirdTestNeg1()
        {
           
            // Act
            //weekday
            //entry is 5.59 am 
            //exit 3.30PM
            //entry minute too early for early bird
            //not early bird

            string entry = "2017-03-01 05:59:00";
            string exit = "2017-03-01 15:30:00";
            var response = _controller.CalculateRates(new UserUI(entry, exit));

            var result = JsonConvert.DeserializeObject<Rate>(response.Content.ReadAsStringAsync().Result);
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual("Early Bird", result.RateName);
            Assert.AreNotEqual("Flat Rate", result.RateType);
            Assert.AreNotEqual(13M, result.RatePrice);
        }

        [TestMethod]
        public void RatesControllerEarlyBirdTestNeg2()
        {
            
            // Act
            //weekday
            //entry is 6.00 am 
            //exit 11.31PM
            //exit minute too late for early bird
            //not early bird

            string entry = "2017-03-01 06:00:00";
            string exit = "2017-03-01 23:31:00";
            var response = _controller.CalculateRates(new UserUI(entry, exit));

            var result = JsonConvert.DeserializeObject<Rate>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual("Early Bird", result.RateName);
            Assert.AreNotEqual("Flat Rate", result.RateType);
            Assert.AreNotEqual(13M, result.RatePrice);
        }

        [TestMethod]
        public void RatesControllerEarlyBirdTestNeg3()
        {
           
            // Act
            //weekday
            //entry is 9.01 am 
            //exit 3.30PM
            //entry minute too late for early bird
            //not early bird

            string entry = "2017-03-01 09:01:00";
            string exit = "2017-03-01 15:30:00";
            var response = _controller.CalculateRates(new UserUI(entry, exit));

            var result = JsonConvert.DeserializeObject<Rate>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual("Early Bird", result.RateName);
            Assert.AreNotEqual("Flat Rate", result.RateType);
            Assert.AreNotEqual(13M, result.RatePrice);
        }

        [TestMethod]
        public void RatesControllerEarlyBirdTestNeg4()
        {
            
            // Act
            //weekday
            //entry is 6.00 am 
            //exit 03.29PM
            //exit minute too early for early bird
            //not early bird

            string entry = "2017-03-01 06:00:00";
            string exit = "2017-03-01 15:29:00";
            var response = _controller.CalculateRates(new UserUI(entry, exit));

            var result = JsonConvert.DeserializeObject<Rate>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual("Early Bird", result.RateName);
            Assert.AreNotEqual("Flat Rate", result.RateType);
            Assert.AreNotEqual(13M, result.RatePrice);
        }

        [TestMethod]
        public void RatesControllerEarlyBirdTestNeg5()
        {           
            // Act
            //weekend
            //entry is 6am 
            //exit 11.30PM
            //times good for early bird but weekrate takes precedence
            //weekend rate

            string entry = "2017-03-04 06:00:00";
            string exit = "2017-03-04 23:31:00";
            var response = _controller.CalculateRates(new UserUI(entry, exit));

            var result = JsonConvert.DeserializeObject<Rate>(response.Content.ReadAsStringAsync().Result);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Weekend Rate", result.RateName);
            Assert.AreEqual("Flat Rate", result.RateType);
            Assert.AreEqual(10M, result.RatePrice);
        }
    }
}
