using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TtcMau.ViewModels
{
    public class ShipStatusViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ShipStatusViewModel> _logger;
        private bool _isLoading;

        public ObservableCollection<LadingStatus> Ladings { get; set; } = new ObservableCollection<LadingStatus>();

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        public ShipStatusViewModel(HttpClient httpClient, ILogger<ShipStatusViewModel> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task LoadLadingsAsync()
        {
            IsLoading = true;
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7143/api/StatusLading/ByShip");
                response.EnsureSuccessStatusCode();

                var ladings = await response.Content.ReadFromJsonAsync<List<LadingStatus>>();
                Ladings.Clear();
                foreach (var lading in ladings)
                {
                    Ladings.Add(lading);
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error while loading ladings.");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error while loading ladings.");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class LadingStatus
    {
        public int LadingId { get; set; }
        public string ProductName { get; set; }
        public string ShipName { get; set; }
        public string TerminalName { get; set; }
        public int Hoeveelheid { get; set; }
        public string Datum { get; set; }
        public string Tijd { get; set; }
        public bool? Status { get; set; }
        public string Reason { get; set; }
    }
}
