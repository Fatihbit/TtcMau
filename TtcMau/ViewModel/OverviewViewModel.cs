using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TtcMau.Dtos;

public class OverviewViewModel
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OverviewViewModel> _logger;

    public OverviewViewModel(HttpClient httpClient, ILogger<OverviewViewModel> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public List<LadingDto> Ladings { get; private set; }
    public List<VeiligheidsChecklistDTO> Checklists { get; private set; }

    public async Task LoadDataAsync()
    {
        try
        {
            Ladings = await _httpClient.GetFromJsonAsync<List<LadingDto>>("https://localhost:7143/api/Overview/lading");
            Checklists = await _httpClient.GetFromJsonAsync<List<VeiligheidsChecklistDTO>>("https://localhost:7143/api/Overview/veiligheidschecklist");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading overview data.");
        }
    }
}
