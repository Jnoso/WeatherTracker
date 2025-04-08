using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using WeatherTracker.Models;
using WeatherTracker.Services;

namespace WeatherTracker.Pages;

public partial class FavoritesPage : ContentPage
{
	public ObservableCollection<FavoriteCity> FavoriteCity { get; set; } = new ObservableCollection<FavoriteCity>();
	public ObservableCollection<FavCity> FavList { get; set; } = new ObservableCollection<FavCity>();
	private int primaryId;
	private bool _isEditFormVisible;
    private WeatherAPI weatherAPI;
    public ICommand OpenEditForm { get; }
	public ICommand DeleteCommand { get; }

	public int PrimaryId
	{
		get => primaryId;
		set => primaryId = value;
	}

	public bool IsEditFormVisible
	{
		get => _isEditFormVisible;
		set
		{
			_isEditFormVisible = value;
			OnPropertyChanged(nameof(IsEditFormVisible));
		}
	}

	public FavoritesPage()
	{
		InitializeComponent();

        weatherAPI = new WeatherAPI();
        OpenEditForm = new Command<FavCity>(EditForm);
		DeleteCommand = new Command<FavCity>(Delete);

        BindingContext = this;

        IsEditFormVisible = false;

		GetTemp();
	}

	//Open Edit Form
	private void EditForm(FavCity favCity)
	{
		IsEditFormVisible = true;
		editCityName.Text = favCity.Name;
		editOptionalNotesEntry.Text = favCity.Notes;
		PrimaryId = favCity.Id;
	}

	//Apply button clicked, applies changes to SQL Database
	//Then refreshes what is displayed on the fav page
	private async void ApplyButton_Clicked(object sender, EventArgs e)
	{
		int id = PrimaryId;
		string cityName = editCityName.Text;
		string notes = editOptionalNotesEntry.Text;

		await DatabaseSQL.EditCity(id, cityName, notes);

		await GetTemp();

		IsEditFormVisible = false;
	}

	//Delete from Fav List and SQL Database
	private async void Delete(FavCity favCity)
	{
		PrimaryId = favCity.Id;

		bool answer = await DisplayAlert("Delete Confirmation", "Are you sure you want to delete?", "Yes", "No");

		if (answer)
		{
			await DatabaseSQL.RemoveCity(PrimaryId);

			await GetTemp();
		}
		else
		{
			return;
		}
	}

	private async void EditCancelButton_Clicked(object sender, EventArgs e)
	{
		IsEditFormVisible = false;
	}


	//Gets the city from SQL Database
	//Adds current day avg temp to observablecollection
	//to be displayed
    public async Task GetTemp()
    {
        IsBusy = true;

        await Task.Delay(1000);

		await RefreshFav();
		
		if(FavoriteCity.Count == 0)
		{
			FavList.Clear();
			return;
		}

		FavList.Clear();

        try
        {
            foreach (var city in FavoriteCity)
			{
				var tempData = await weatherAPI.GetWeather(city.CityName);

				if (tempData != null)
				{
					int id = city.Id;
					string name = city.CityName;
					float temp = tempData[0].temp;
					string notes = city.OptionalNotes;

					var fav = new FavCity
					{
						Id = id,
						Name = name,
						Temp = temp,
						Notes = notes
					};

					FavList.Add(fav);
				}
			}
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No city data found", "OK");
        }

        IsBusy = false;
    }

	//Refresh the SQL Database with new data
	async Task RefreshFav()
	{
		IsBusy = true;

		await Task.Delay(1000);

		FavoriteCity.Clear();

		var cities = await DatabaseSQL.GetFavoriteCities();

		foreach(var city in cities)
		{
			FavoriteCity.Add(city);
		}

		IsBusy = false;
	}


}