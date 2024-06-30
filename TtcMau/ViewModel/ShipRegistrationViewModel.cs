using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

public class ShipRegistrationViewModel
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ShipRegistrationViewModel> _logger;

    public ShipRegistrationViewModel(HttpClient httpClient, ILogger<ShipRegistrationViewModel> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    [Required]
    public string ShipName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Type { get; set; }

    [Required]
    public string UniekEuropeesScheepsidentificatienummer { get; set; }

    [Required]
    public string Location { get; set; }

    public async Task RegisterShipAsync(NavigationManager navigationManager)
    {
        try
        {
            var ship = new
            {
                ShipName,
                Email,
                Password,
                Type,
                UniekEuropeesScheepsidentificatienummer,
                Location
            };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7143/api/Account/register-ship", ship);
            if (response.IsSuccessStatusCode)
            {
                navigationManager.NavigateTo("/login");
            }
            else
            {
                _logger.LogError("Ship registration failed: {Message}", response.ReasonPhrase);
                // Handle error (e.g., show a message to the user)
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ship registration failed.");
            // Handle error (e.g., show a message to the user)
        }
    }
}
