using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

public class LoginViewModel : ObservableObject
{
    private string _email;
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    private string _password;
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    private string _loginMessage;
    public string LoginMessage
    {
        get => _loginMessage;
        set => SetProperty(ref _loginMessage, value);
    }

    private readonly HttpClient _httpClient;

    public LoginViewModel(HttpClient httpClient)
    {
        _httpClient = httpClient;
        LoginCommand = new AsyncRelayCommand(LoginAsync);
    }

    public IAsyncRelayCommand LoginCommand { get; }

    private async Task LoginAsync()
    {
        try
        {
            var loginRequest = new { Email, Password };
            var response = await _httpClient.PostAsJsonAsync("api/Account/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                LoginMessage = "Login successful";
                System.Diagnostics.Debug.WriteLine("Login successful");
            }
            else
            {
                LoginMessage = "Login failed: " + await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("Login failed: " + await response.Content.ReadAsStringAsync());
            }
        }
        catch (Exception ex)
        {
            LoginMessage = $"Login failed: {ex.Message}";
            System.Diagnostics.Debug.WriteLine($"Login failed: {ex.Message}");
        }
    }
}
