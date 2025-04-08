using System.Collections.ObjectModel;
using WeatherTracker.Models;

namespace WeatherTracker.Pages;

public partial class DailyDetail : ContentPage
{
	public ObservableCollection<WeatherDaily> weatherDailies { get; set; } = new ObservableCollection<WeatherDaily>();

	public ObservableCollection<WeatherData> weatherData { get; set; } = new ObservableCollection<WeatherData>();

	private bool _useCelsius;

	public ObservableCollection<WeatherDaily> WeatherDailies
	{
		get => weatherDailies;
		set => weatherDailies = value;
	}

	public ObservableCollection<WeatherData> WeatherData
	{
		get => weatherData;
		set => weatherData = value;
	}

	public bool UseCelsius
	{
		get => _useCelsius;
		set
		{
			_useCelsius = value;
			OnPropertyChanged(nameof(UseCelsius));
		}
	}

	public DailyDetail(ObservableCollection<WeatherDaily> weatherForecast)
	{
		InitializeComponent();
		BindingContext = this;
		WeatherDailies = weatherForecast;

		UseCelsius = false;

		ConvertTemp();
	}

	private async void ConvertTemp()
	{
		weatherData.Clear();
		
		foreach (var day in WeatherDailies)
		{
			if (UseCelsius)
			{
				weatherData.Add(new WeatherCelsius
				{
					Date = day.datetime,
					TempF = day.temp,
					TempMinF = day.tempmin,
					TempMaxF = day.tempmax,
					Conditions = day.conditions,
					Description = day.description,
					precip = day.precip,
					precipprob = day.precipprob
				});
			}
			else
			{
				weatherData.Add(new WeatherFahrenheit
				{
					Date = day.datetime,
					TempF = day.temp,
					TempMinF = day.tempmin,
					TempMaxF = day.tempmax,
					Conditions = day.conditions,
					Description = day.description,
					precip = day.precip,
					precipprob = day.precipprob
				});
			}
		}
	}

    private void Fahrenheit_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
		if (e.Value == true)
		{
			UseCelsius = false;
			ConvertTemp();
		}
    }

    private void Celsius_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
		if (e.Value == true)
		{
			UseCelsius = true;
			ConvertTemp();
		}
    }
}