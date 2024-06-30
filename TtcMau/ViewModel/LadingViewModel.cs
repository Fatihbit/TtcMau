using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.Json;

public class LadingViewModel : ObservableObject
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<LadingViewModel> _logger;

    public LadingViewModel(HttpClient httpClient, ILogger<LadingViewModel> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        LoadDataCommand = new AsyncRelayCommand(LoadDataAsync);
        SubmitLadingCommand = new AsyncRelayCommand(SubmitLadingAsync);
    }

    public IAsyncRelayCommand LoadDataCommand { get; }
    public IAsyncRelayCommand SubmitLadingCommand { get; }

    private List<JsonElement> _products;
    public List<JsonElement> Products
    {
        get => _products;
        set => SetProperty(ref _products, value);
    }

    private List<JsonElement> _terminals;
    public List<JsonElement> Terminals
    {
        get => _terminals;
        set => SetProperty(ref _terminals, value);
    }

    private string _selectedProduct;
    public string SelectedProduct
    {
        get => _selectedProduct;
        set => SetProperty(ref _selectedProduct, value);
    }

    private string _selectedTerminal;
    public string SelectedTerminal
    {
        get => _selectedTerminal;
        set => SetProperty(ref _selectedTerminal, value);
    }

    private string _shipName;
    public string ShipName
    {
        get => _shipName;
        set => SetProperty(ref _shipName, value);
    }

    private int _hoeveelheid;
    public int Hoeveelheid
    {
        get => _hoeveelheid;
        set => SetProperty(ref _hoeveelheid, value);
    }

    private DateTime _datum;
    public DateTime Datum
    {
        get => _datum;
        set => SetProperty(ref _datum, value);
    }

    private TimeSpan _tijd;
    public TimeSpan Tijd
    {
        get => _tijd;
        set => SetProperty(ref _tijd, value);
    }

    private string _message;
    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    private async Task LoadDataAsync()
    {
        try
        {
            _logger.LogInformation("Loading products and terminals...");
            Products = await _httpClient.GetFromJsonAsync<List<JsonElement>>("https://localhost:7143/api/Product");
            Terminals = await _httpClient.GetFromJsonAsync<List<JsonElement>>("https://localhost:7143/api/Terminal");
            _logger.LogInformation("Products and terminals loaded successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load products or terminals.");
            Message = "Failed to load data.";
        }
    }

    private async Task SubmitLadingAsync()
    {
        var lading = new
        {
            ProductName = SelectedProduct,
            TerminalName = SelectedTerminal,
            ShipName = ShipName,
            Hoeveelheid = Hoeveelheid,
            Datum = Datum,
            Tijd = Tijd
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7143/api/Lading", lading);
            var requestBody = JsonSerializer.Serialize(lading); // Log het verzonden JSON-object
            _logger.LogInformation($"Request Body: {requestBody}");

            if (response.IsSuccessStatusCode)
            {
                Message = "Lading successfully added.";
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Message = $"Error: {response.StatusCode} - {responseContent}";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to submit lading.");
            Message = "Failed to submit lading.";
        }
    }
}
