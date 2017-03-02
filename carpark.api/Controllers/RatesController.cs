using carpark.api.Models;
using carpark.api.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace carpark.api.Controllers
{
    public class RatesController : ApiController
    {
        private IRatesCalculator _ratesCalculator;

        public RatesController(IRatesCalculator ratesCalculator)
        {
            _ratesCalculator = ratesCalculator;
        }               
        
        public HttpResponseMessage CalculateRates([FromBody]UserUI userEntry)
        {
            HttpResponseMessage response; 

            try
            {
                response = ValidateUserEntry(userEntry);

                if (response.StatusCode == HttpStatusCode.OK)                
                {
                    var userData = new UserData(userEntry);

                    Rate rate = _ratesCalculator.CalculateFlatRate(userData);

                    if (rate == null)
                    {
                        rate = _ratesCalculator.CalculateHourlyRate(userData);
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, rate);
                }                
            }
            catch(Exception e)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);

            }
            
            return response;
            
        }

        private HttpResponseMessage ValidateUserEntry(UserUI userEntry)
        {
            DateTime entry;
            DateTime exit;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            if (!DateTime.TryParse(userEntry.Entry, out entry))
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid entry date time");
            }

            if (!DateTime.TryParse(userEntry.Exit, out exit))
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid exit date time");
            }

            TimeSpan diff = exit.Subtract(entry);
            if(diff.TotalMinutes < 0)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid exit must be after entry");
            }

            return response;
        }
    }
}
