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
        public string EntryDate { get; set; }
        public string EntryTime { get; set; }


        public string Exit { get; set; }
        public string ExitDate { get; set; }
        public string ExitTime { get; set; }

        public void AssembleDatesTimes()
        {
            Entry = string.Format("{0} {1}", EntryDate.Trim(), EntryTime.Trim());
            Exit = string.Format("{0} {1}", ExitDate.Trim(), ExitTime.Trim());
        }

        public Rate RateDisplay { get; set; }
    }
}