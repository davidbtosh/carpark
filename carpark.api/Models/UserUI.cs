using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace carpark.api.Models
{
    public class UserUI
    {
        public UserUI() { }

        public UserUI(string entry, string exit)
        {
            Entry = entry;
            Exit = exit;
        }
        public string Entry { get; set; }


        public string Exit { get; set; }

        public Rate RateDisplay { get; set; }
    }
}