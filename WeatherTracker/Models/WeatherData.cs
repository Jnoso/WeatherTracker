using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherTracker.Models
{
    public abstract class WeatherData
    {
        public DateTime Date { get; set; }
        public string Conditions { get; set; }
        public string Description { get; set; }
        public float precip { get; set; }
        public float precipprob { get; set; }

        public abstract float AvgTemperature { get; }
        public abstract float TempMin { get;  }
        public abstract float TempMax { get;  }
    }

    public class WeatherFahrenheit : WeatherData
    {
        public float TempF { get; set; }
        public float TempMinF { get; set; }
        public float TempMaxF { get; set; }

        public override float AvgTemperature => TempF;
        public override float TempMin => TempMinF;
        public override float TempMax => TempMaxF;
    }

    public class WeatherCelsius : WeatherData
    {
        public float TempF { get; set; }
        public float TempMinF { get; set; }
        public float TempMaxF { get; set; }

        public override float AvgTemperature => (TempF - 32)*5/9;
        public override float TempMin => (TempMinF - 32)*5/9;
        public override float TempMax => (TempMaxF - 32)*5/9;
    }
}
