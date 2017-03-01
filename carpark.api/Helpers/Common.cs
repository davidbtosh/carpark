using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace carpark.api.Helpers
{
    public class Common
    {
        public List<T> LoadRates<T>(string file)
        {
            string filepath = ConfigurationManager.AppSettings.Get("pathtofiles");

            file = string.Format("{0}{1}", filepath, file);

            using (StreamReader r = new StreamReader(file))
            {
                string json = r.ReadToEnd();
                List<T> items = JsonConvert.DeserializeObject<List<T>>(json);
                return items;
            }
        }
    }
}