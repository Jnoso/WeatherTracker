using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Windows.Input;
using WeatherTracker.Models;
using WeatherTracker.Services;

namespace WeatherTracker.Pages;

public partial class SearchPage : ContentPage
{
	public ObservableCollection<WeatherDaily> WeatherForecast { get; set; } = new ObservableCollection<WeatherDaily>();
	private string _city;
	private string _searchedCity;
	private bool _isAddFormVisible;
	private WeatherAPI weatherAPI;

	public ICommand ShowWeather { get; }
	public ICommand DetailPage { get; }
	public ICommand ShowFavorite { get; }

	//Property to store user city input
	public string City
	{
		get => _city;
		set
		{
			_city = value;
			OnPropertyChanged(nameof(City));
		}
	}

	//Property that stores city that was found in Third-Party API
	//Property used to allow other functionalities on the app to work
	public string SearchedCity
	{
		get => _searchedCity;
		set => _searchedCity = value;
	}

	//Property to store IsVisible bool for Form
	public bool IsAddFormVisible
	{
		get => _isAddFormVisible;
		set
		{
			_isAddFormVisible = value;
			OnPropertyChanged(nameof(IsAddFormVisible));
		}
	}

	public SearchPage()
	{
        InitializeComponent();

		weatherAPI = new WeatherAPI();
		ShowWeather = new Command(BtnClicked);
        DetailPage = new Command(OnReportBtnTapped);
		ShowFavorite = new Command(FavBtnClicked);


        BindingContext = this;

		IsAddFormVisible = false;
    }

	//Search Button Clicked to Fetch Data
	public async void BtnClicked()
	{
		GetWeather();
	}

	//Fav Button Clicked to Open Fav Page
	public async void FavBtnClicked()
	{
		await Navigation.PushAsync(new FavoritesPage());
	}

	//Check if city is searched then navigate to detailed page
	private async void OnReportBtnTapped()
	{

		if(WeatherForecast == null || WeatherForecast.Count == 0)
		{
			await DisplayAlert("Warning", "Please enter a city first to see the detail weather", "OK");
			return;
		}

		await Navigation.PushAsync(new DailyDetail(WeatherForecast));
	}

	//Check if city is searched, then form opens to add to favorite
	private async void addToFavorite(object sender, EventArgs e)
	{
		if(WeatherForecast == null || WeatherForecast.Count == 0)
		{
			await DisplayAlert("Warning", "Please enter a city first to add to favorite.", "OK");
			return;
		}
		IsAddFormVisible = true;
	}

	//Form Add Button Clicked, adds to SQL Database
	private async void AddButton_Clicked(object sender, EventArgs e)
	{
		string cityName = SearchedCity;
		string optionalNotes = optionalNotesEntry.Text;

		try
		{
			await DatabaseSQL.AddCity(cityName, optionalNotes);
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", "Could not add to favorite list", "OK");
		}
		
		optionalNotesEntry.Text = string.Empty;

		IsAddFormVisible = false;

		await DisplayAlert("Favorites", "City Added to Favorites", "OK");
	}

	//Form Cancel Button Clicked, remove form
	private void AddCancelButton_Clicked(object sender, EventArgs e)
	{
		IsAddFormVisible = false;
	}

	//Get weather data based on City Input once button is pressed.
	public async Task GetWeather()
	{
		IsBusy = true;

		await Task.Delay(1000);
		City = City.Trim();

		try
		{
			var forecastData = await weatherAPI.GetWeather(City);
			WeatherForecast.Clear();

			foreach(var day in forecastData)
			{
				WeatherForecast.Add(day);
				Debug.WriteLine($"Added: {day.datetime} & {day.temp}");
			}

			//Sets SearchedCity to user input once GetWeather() confirms city exists
			SearchedCity = City;
		}
		catch (ArgumentException ex)
		{
			await DisplayAlert("Error", ex.Message, "OK");
		}

		IsBusy = false;
	}
}