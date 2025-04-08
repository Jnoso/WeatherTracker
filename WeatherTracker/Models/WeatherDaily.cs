using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherTracker.Models
{
    public class WeatherDaily
    {
        public DateTime datetime { get; set; }
        public float temp { get; set; }
        public float tempmin { get; set; }
        public float tempmax { get; set; }
        public float precip { get; set; }
        public float precipprob { get; set; }

        public string conditions { get; set; }

        public string description { get; set; }
    }
}
