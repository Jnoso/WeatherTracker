using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherTracker.Models
{
    public class FavCity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Temp { get; set; }
        public string Notes { get; set; }
    }
}
