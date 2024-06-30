using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

public class TerminalRegistrationViewModel
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TerminalRegistrationViewModel> _logger;

    public TerminalRegistrationViewModel(HttpClient httpClient, ILogger<TerminalRegistrationViewModel> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    [Required]
    public string TerminalName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Location { get; set; }

    public async Task RegisterTerminalAsync(NavigationManager navigationManager)
    {
        try
        {
            var terminal = new
            {
                TerminalName,
                Email,
                Password,
                Location
            };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7143/api/Account/register-terminal", terminal);
            if (response.IsSuccessStatusCode)
            {
                navigationManager.NavigateTo("/login");
            }
            else
            {
                _logger.LogError("Terminal registration failed: {Message}", response.ReasonPhrase);
                // Handle error (e.g., show a message to the user)
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Terminal registration failed.");
            // Handle error (e.g., show a message to the user)
        }
    }
}
