using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public class StatusLadingViewModel : ObservableObject
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<StatusLadingViewModel> _logger;

    public StatusLadingViewModel(HttpClient httpClient, ILogger<StatusLadingViewModel> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        LoadLadingsCommand = new AsyncRelayCommand(LoadLadingsAsync);
        ApproveLadingCommand = new AsyncRelayCommand<int>(async (id) => await ApproveLadingAsync(id, true));
        RejectLadingCommand = new AsyncRelayCommand<int>(async (id) => await ApproveLadingAsync(id, false));
    }

    private List<JsonElement> _ladings;
    public List<JsonElement> Ladings
    {
        get => _ladings;
        set => SetProperty(ref _ladings, value);
    }

    private string _reason;
    public string Reason
    {
        get => _reason;
        set => SetProperty(ref _reason, value);
    }

    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public IAsyncRelayCommand LoadLadingsCommand { get; }
    public IAsyncRelayCommand<int> ApproveLadingCommand { get; }
    public IAsyncRelayCommand<int> RejectLadingCommand { get; }

    public async Task LoadLadingsAsync()
    {
        try
        {
            _logger.LogInformation("Loading ladings...");
            var response = await _httpClient.GetAsync("https://localhost:7143/api/StatusLading/ByTerminal");

            if (response.IsSuccessStatusCode)
            {
                Ladings = await response.Content.ReadFromJsonAsync<List<JsonElement>>();
                _logger.LogInformation("Ladings loaded successfully.");
            }
            else
            {
                _logger.LogError("Failed to load ladings. Status code: {StatusCode}", response.StatusCode);
                ErrorMessage = $"Failed to load ladings. Status code: {response.StatusCode}";
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to load ladings.");
            ErrorMessage = "Failed to load ladings.";
        }
    }

    public async Task ApproveLadingAsync(int ladingId, bool isApproved)
    {
        try
        {
            var data = new { LadingId = ladingId, IsApproved = isApproved, Reason = Reason };
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7143/api/StatusLading/ApproveLading", data);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Lading approval status sent successfully.");
            }
            else
            {
                _logger.LogError("Failed to update lading status. Status code: {StatusCode}", response.StatusCode);
                ErrorMessage = $"Failed to update lading status. Status code: {response.StatusCode}";
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to update lading status.");
            ErrorMessage = "Failed to update lading status.";
        }
    }
}
