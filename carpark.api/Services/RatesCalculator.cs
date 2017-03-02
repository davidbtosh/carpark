using carpark.api.Helpers;
using carpark.api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace carpark.api.Services
{
    public class RatesCalculator : IRatesCalculator
    {
        public Rate CalculateRate(UserUI userEntry)
        {
            var userData = new UserData(userEntry);

            Rate rate = CalculateFlatRate(userData);

            if (rate == null)
            {
                rate = CalculateHourlyRate(userData);
            }

            return rate;
        }



        //FLAT RATE RULES
        Func<int, FlatRate, bool> entryDay = (dy, r) => { return dy.IsBetween(r.StartDay, r.EndDay); };
        Func<UserData, FlatRate, bool> entryTimes = (ud, r) => { return ud.EntryHour.IsBetween(r.EntryStart, r.EntryEnd) && ud.ExitHour.IsBetween(r.ExitStart, r.ExitEnd); };

        public Rate CalculateFlatRate(UserData ud)
        {
            //find all rates that apply to the FLAT RATE RULES and return one with highest precedence
            //null if none apply
            var fr = FlatRates.FindAll(f => entryDay(ud.EntryDoW, f) && entryTimes(ud, f)).OrderBy(o => o.OrderOfPrecedence).FirstOrDefault();
           

            return fr as Rate;

        }

        public Rate CalculateHourlyRate(UserData ud)
        {
            var hr = HourlyRates.FindAll(f => !f.IsMaxRate && ud.TotalHours <= f.MaxHours).OrderBy(o => o.MaxHours).FirstOrDefault();

            if (hr == null)
            {
                hr = HourlyRates.Find(f => f.IsMaxRate);
                if(ud.TotalDays > 1)
                {
                    hr.RatePrice = hr.RatePrice * ud.TotalDays;
                }
            }
            return hr as Rate;
        }

        public string FilePath { get; set; }

        private List<HourlyRate> _hourlyRates = null;
        private List<HourlyRate> HourlyRates
        {
            get
            {
                if (_hourlyRates == null)
                {
                    _hourlyRates = LoadRates<HourlyRate>("HourlyRates.json");
                }
                return _hourlyRates;
            }
        }

        private List<FlatRate> _flatRates = null;
        private List<FlatRate> FlatRates
        {
            get
            {
                if (_flatRates == null)
                {
                    _flatRates = LoadRates<FlatRate>("FlatRates.json");
                }
                return _flatRates;
            }
        }


        private List<T> LoadRates<T>(string file)
        {                        
            file = string.Format("{0}{1}", FilePath, file);

            using (StreamReader r = new StreamReader(file))
            {
                string json = r.ReadToEnd();
                List<T> items = JsonConvert.DeserializeObject<List<T>>(json);
                return items;
            }
        }

        
    }
}