using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeatherTracker.Models;

namespace WeatherTracker.Services
{
    public class WeatherAPI
    {
        private const string APIKey = "34TWKDEH3DR6NPLSJJ72LM6LA";
        private const string BaseUrl = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/";

        HttpClient _client;

        public WeatherAPI()
        {
            _client = new HttpClient();
        }

        //JSON file has 'Days' 'Hours' Array. Only Want 'Days' Information
        public class WeatherDailyResponse
        {
            public List<WeatherDaily> days { get; set; }
        }

        public async Task<List<WeatherDaily>> GetWeather(string city)
        {
            try
            {
                if (string.IsNullOrEmpty(city) || !Regex.IsMatch(city, @"^[a-zA-Z\s]+$") || string.IsNullOrWhiteSpace(city))
                {
                    throw new ArgumentException("Please Enter a valid city name with only letters.");
                }
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"{ex.Message}");
            }
            
            try
            {
                string url = $"{BaseUrl}{city}/next6days?unitGroup=us&key={APIKey}&contentType=json";
                HttpResponseMessage response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    var deserialize = JsonSerializer.Deserialize<WeatherDailyResponse>(json);

                    if (deserialize == null)
                    {
                        throw new Exception("City could not be found or no weather data available.");
                    }

                    return deserialize.days;
                }
                else
                {
                    throw new Exception("Error no response from website");
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
