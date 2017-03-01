using carpark.api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using carpark.api.Helpers;
using System.Configuration;
using System.Diagnostics;
using carpark.api.Services;

namespace carpark.api.Controllers
{
    public class RatesController : ApiController
    {
        private IRatesCalculator _ratesCalculator;

        public RatesController(IRatesCalculator ratesCalculator)
        {
            _ratesCalculator = ratesCalculator;
        }

        public RatesController()
        {
            _ratesCalculator = new RatesCalculator();
        }
        
        // GET api/rates/2017-03-01 18:00:00/2017-03-01 22:00:00 
        public Rate CalculateRates(DateTime entry, DateTime exit)
        {
            Rate rate = null;

            var userData = new UserData(entry, exit);

            rate = _ratesCalculator.CalculateFlatRate(userData);

            if(rate == null)
            {
                rate = _ratesCalculator.CalculateHourlyRate(userData);
            }
           
            return rate;
        }
        

       

             

    }
}
