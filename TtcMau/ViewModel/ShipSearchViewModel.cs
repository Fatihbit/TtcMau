using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class ShipSearchViewModel : ObservableObject
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ShipSearchViewModel> _logger;

    public ShipSearchViewModel(HttpClient httpClient, ILogger<ShipSearchViewModel> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        SearchShipCommand = new AsyncRelayCommand(SearchShipAsync);
    }

    private string _shipName;
    public string ShipName
    {
        get => _shipName;
        set => SetProperty(ref _shipName, value);
    }

    private ShipDetails _shipDetails;
    public ShipDetails ShipDetails
    {
        get => _shipDetails;
        set => SetProperty(ref _shipDetails, value);
    }

    public IAsyncRelayCommand SearchShipCommand { get; }

    private async Task SearchShipAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/Ship/{ShipName}");
            if (response.IsSuccessStatusCode)
            {
                ShipDetails = await response.Content.ReadFromJsonAsync<ShipDetails>();
                _logger.LogInformation("Ship data loaded successfully.");
            }
            else
            {
                _logger.LogError("Error loading ship data: {StatusCode}", response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while loading ship data.");
        }
    }
}

public class ShipDetails
{
    public string ShipName { get; set; }
    public string Email { get; set; }
    public string Type { get; set; }
    public string UniekEuropeesScheepsidentificatienummer { get; set; }
    public string Location { get; set; }
}
