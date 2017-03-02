using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace carpark.api.Helpers
{
    public static class Extensions
    {
        public static bool IsBetween(this double val, double start, double end)
        {
            return val >= start && val < end;
        }

        public static bool IsBetween(this int val, double start, double end)
        {
            return val >= start && val < end;
        }
    }
}