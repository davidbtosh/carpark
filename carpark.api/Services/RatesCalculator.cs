﻿using carpark.api.Helpers;
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
       
        //FLAT RATE RULES
        Func<int, FlatRate, bool> entryDay = (dy, r) => { return dy.IsBetween(r.StartDay, r.EndDay); };
        Func<UserData, FlatRate, bool> entryTimes = (ud, r) => { return ud.EntryHour.IsBetween(r.EntryStart, r.EntryEnd) && ud.ExitHour.IsBetween(r.ExitStart, r.ExitEnd); };

        public Rate CalculateFlatRate(UserData ud)
        {
            //find all rates that apply to the FLAT RATE RULES and return one with highest precedence
            //null if none apply
            var fr = FlatRates.FindAll(f => entryDay(ud.EntryDoW, f) && entryTimes(ud, f)).OrderBy(o => o.OrderOfPrecedence).FirstOrDefault();


            foreach (var frt in FlatRates)
            {
                Debug.Write(frt.RateName);

                bool ed = entryDay(ud.EntryDoW, frt);
                bool et = entryTimes(ud, frt);

            }

            //return null;

            return fr as Rate;

        }

        public Rate CalculateHourlyRate(UserData ud)
        {
            var hr = HourlyRates.FindAll(f => !f.IsMaxRate && ud.TotalHours <= f.MaxHours).OrderByDescending(o => o.MaxHours).FirstOrDefault();

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
            string fp = Path.GetFullPath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\..\..\..\carpark.api\App_Data\");
            file = string.Format("{0}{1}", fp, file);

            using (StreamReader r = new StreamReader(file))
            {
                string json = r.ReadToEnd();
                List<T> items = JsonConvert.DeserializeObject<List<T>>(json);
                return items;
            }
        }
    }
}