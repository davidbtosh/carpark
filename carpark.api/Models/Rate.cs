using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace carpark.api.Models
{
    public class Rate 
    {
        public string RateName { get; set; }

        public string RateType { get; set; }

        public decimal RatePrice { get; set; }

       
    }
}