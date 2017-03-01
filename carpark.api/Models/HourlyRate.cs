using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace carpark.api.Models
{
    public class HourlyRate : Rate
    {
        public int MaxHours{ get; set; }

        public bool IsMaxRate { get; set; }
       
    }
}