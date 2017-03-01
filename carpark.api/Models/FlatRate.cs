using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace carpark.api.Models
{
    public class FlatRate : Rate
    {        
        public double EntryStart { get; set; }

        public double EntryEnd { get; set; }

        public double ExitStart { get; set; }

        public double ExitEnd { get; set; }

        public int StartDay { get; set; }

        public int EndDay { get; set; }

        public int OrderOfPrecedence { get; set; }

        
    }

    
}