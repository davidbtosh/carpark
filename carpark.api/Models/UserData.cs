using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace carpark.api.Models
{
    public class UserData
    {
        public UserData() { }

        public UserData(DateTime entry, DateTime exit)
        {
            Entry = entry;
            Exit = exit;
        }

        public UserData(UserUI userEntry)
        {
            Entry = Convert.ToDateTime(userEntry.Entry);
            Exit = Convert.ToDateTime(userEntry.Exit); ;
        }

        public DateTime Entry { get; set; }

        public DateTime Exit { get; set; }

        public int EntryDoW
        {
            get
            {
                var dow = (int)Entry.DayOfWeek;
                return dow < 1 ? 7 : dow;
            }
        }
        //public int ExitDoW
        //{
        //    get { return (int)Exit.DayOfWeek.; }
        //}

        //Func<DateTime, double> hourCalc = (dt) =>
        //{
        //    double dm = 0;



        //    return dm;
        //};

        public double EntryHour
        {
            get { return Entry.TimeOfDay.TotalHours; }
        }

        public double ExitHour
        {
            get
            {
                TimeSpan diff = Exit.Date - Entry.Date;
                int wholeDaysinHours = Convert.ToInt32(diff.TotalDays) * 24;
                
                return Exit.TimeOfDay.TotalHours + wholeDaysinHours;
            }
        }

        public int TotalHours
        {
            get
            {
                TimeSpan diff = Exit - Entry;
                int hours = Convert.ToInt32(Math.Truncate(diff.TotalHours));

                return hours;
            }
        }

        public int TotalDays
        {
            get
            {
                double d = TotalHours / 24;
                int days = Convert.ToInt32(Math.Truncate(d));

                return days;
            }
        }
    }
}