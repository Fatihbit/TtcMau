using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Devices.Sensors;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class ProfileViewModel : ObservableObject
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProfileViewModel> _logger;

    public ProfileViewModel(HttpClient httpClient, ILogger<ProfileViewModel> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        LoadProfileCommand = new AsyncRelayCommand(LoadProfileAsync);
        UpdateLocationCommand = new AsyncRelayCommand(UpdateLocationAsync);
    }

    private string _userName;
    public string UserName
    {
        get => _userName;
        set => SetProperty(ref _userName, value);
    }

    private string _email;
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    private string _location;
    public string Location
    {
        get => _location;
        set => SetProperty(ref _location, value);
    }

    private string _type;
    public string Type
    {
        get => _type;
        set => SetProperty(ref _type, value);
    }

    private string _uniekEuropeesScheepsidentificatienummer;
    public string UniekEuropeesScheepsidentificatienummer
    {
        get => _uniekEuropeesScheepsidentificatienummer;
        set => SetProperty(ref _uniekEuropeesScheepsidentificatienummer, value);
    }

    private bool _isShip;
    public bool IsShip
    {
        get => _isShip;
        set => SetProperty(ref _isShip, value);
    }

    private double _latitude;
    public double Latitude
    {
        get => _latitude;
        set => SetProperty(ref _latitude, value);
    }

    private double _longitude;
    public double Longitude
    {
        get => _longitude;
        set => SetProperty(ref _longitude, value);
    }

    public IAsyncRelayCommand LoadProfileCommand { get; }
    public IAsyncRelayCommand UpdateLocationCommand { get; }

    private async Task LoadProfileAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/Account/me");
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                dynamic profile = Newtonsoft.Json.JsonConvert.DeserializeObject(responseData);

                if (profile.terminalName != null)
                {
                    IsShip = false;
                    UserName = profile.terminalName;
                    Email = profile.email;
                    Location = profile.location;
                }
                else if (profile.shipName != null)
                {
                    IsShip = true;
                    UserName = profile.shipName;
                    Email = profile.email;
                    Type = profile.type;
                    UniekEuropeesScheepsidentificatienummer = profile.uniekEuropeesScheepsidentificatienummer;
                    Location = profile.location;
                }

                _logger.LogInformation("Profile data loaded successfully.");
                _logger.LogInformation(responseData);
            }
            else
            {
                _logger.LogError("Error loading profile data: {StatusCode}", response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while loading profile data.");
        }
    }

    public async Task UpdateLocationAsync()
    {
        try
        {
            var location = await Geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Best));
            }

            if (location != null)
            {
                Latitude = location.Latitude;
                Longitude = location.Longitude;

                var locationData = new
                {
                    Location = $"{Latitude}, {Longitude}"
                };

                var response = await _httpClient.PutAsJsonAsync("/api/Ship/UpdateLocation", locationData);
                if (response.IsSuccessStatusCode)
                {
                    Location = locationData.Location;
                    _logger.LogInformation("Location updated successfully.");
                }
                else
                {
                    _logger.LogError("Error updating location: {StatusCode}", response.StatusCode);
                }
            }
            else
            {
                _logger.LogError("Failed to get location.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while updating location.");
        }
    }
}
