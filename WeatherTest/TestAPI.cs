using System.Diagnostics;
using WeatherTracker;
using WeatherTracker.Services;

namespace WeatherTest
{
    public class TestAPI
    {

        //Correctly writes in a city, not null and has item in list
        [Fact]
        public async Task CheckGetAPI()
        {
            var weatherAPI = new WeatherAPI();

            //User correctly writes in a city
            string cityInput = "Seattle";

            var result = await weatherAPI.GetWeather(cityInput);

            bool status = false;

            if (result.Count > 0)
            {
                status = true;
            }

            Console.WriteLine("Checks that it obtain weather information from third API");
            Console.WriteLine("Expected: Data obtained to be true");
            Console.WriteLine($"Actual: {status} data obtained");

            //Should not be null
            Assert.NotNull(result);
            Assert.True(result.Count > 0);
        }

        //Correctly writes in a city and returns 7 items per API request
        [Fact]
        public async Task CheckDataAmount()
        {
            var weatherAPI = new WeatherAPI();

            //User correctly writes in a city
            string cityInput = "Seattle";

            var result = await weatherAPI.GetWeather(cityInput);

            Console.WriteLine("Obtains 7 days of weather information per get request");
            Console.WriteLine("Expected: 7 days of weather data");
            Console.WriteLine($"Actual: {result.Count} days returned");

            //Should have 7 items in the list
            Assert.Equal(7, result.Count);
        }

        //Incorrectly writes city. Checks if regex works and validation works as expected
        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData("Seattle@")]
        [InlineData("!@#$")]
        [InlineData("Seattle1234")]
        public async Task WrongInput(string input)
        {
            var weatherAPI = new WeatherAPI();

            var result = await Assert.ThrowsAsync<ArgumentException>(() => weatherAPI.GetWeather(input));

            Console.WriteLine("Checks if Regex and Validation works for improper input");
            Console.WriteLine($"Input: {input}");
            Console.WriteLine("Expected: Please Enter a valid city name with only letters.");
            Console.WriteLine($"Actual: {result.Message}");

            Assert.Equal("Please Enter a valid city name with only letters.", result.Message);
        }
    }
}
