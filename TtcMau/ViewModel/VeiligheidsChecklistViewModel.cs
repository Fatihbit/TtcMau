using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Collections.Generic;

public class VeiligheidsChecklistViewModel : INotifyPropertyChanged
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;

    public VeiligheidsChecklistViewModel(HttpClient httpClient, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private int _veiligheidsChecklistId;
    public int VeiligheidsChecklistId
    {
        get => _veiligheidsChecklistId;
        set
        {
            _veiligheidsChecklistId = value;
            OnPropertyChanged(nameof(VeiligheidsChecklistId));
        }
    }

    private int _ladingId;
    public int LadingId
    {
        get => _ladingId;
        set
        {
            _ladingId = value;
            OnPropertyChanged(nameof(LadingId));
        }
    }

    private Lading _currentLading;
    public Lading CurrentLading
    {
        get => _currentLading;
        set
        {
            _currentLading = value;
            OnPropertyChanged(nameof(CurrentLading));
        }
    }

    public bool IsSchipGoedGemeerd { get; set; }
    public bool IsDoeltreffendeVerlichtingVerzekerd { get; set; }
    public bool IsNooduitgangVrij { get; set; }
    public bool IsSchipWalVerbindingVeilig { get; set; }
    public bool ZijnLaadarmenVrijBeweegbaar { get; set; }
    public bool ZijnAansluitingenAfgeblind { get; set; }
    public bool ZijnLeidingenInGoedeStaat { get; set; }
    public bool ZijnAlleKleppenGecontroleerd { get; set; }
    public bool IsAlarmNoodstopBekend { get; set; }
    public bool ZijnBrandblusApparatenBedrijfsklaar { get; set; }
    public bool ZijnVerwarmingsapparatenUitgeschakeld { get; set; }
    public bool IsRookverbodAfgekondigd { get; set; }
    public bool ZijnRadarEnAndereElektrischeApparatenUit { get; set; }
    public bool ZijnDeurenEnRamenGesloten { get; set; }
    public bool IsOvervulbeveiligingBeproefd { get; set; }
    public bool IsUitschakelingPompVanafWalMogelijk { get; set; }
    public bool IsGasafvoerleidingCorrectAangesloten { get; set; }
    public bool IsDrukGasterugvoerleidingVeilig { get; set; }
    public bool IsVlamkerendeInrichtingAanwezig { get; set; }
    public bool IsVerblijftijdVastgesteldEnGedocumenteerd { get; set; }
    public bool IsLaadtemperatuurBinnenToegestaneBandbreedte { get; set; }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public async Task LoadLadingDetailsAsync(int ladingId)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7143/api/VeiligheidsChecklist/lading/{ladingId}");
        if (response.IsSuccessStatusCode)
        {
            CurrentLading = await response.Content.ReadFromJsonAsync<Lading>();
        }
        else
        {
            // Handle error
        }
    }

    public async Task SubmitChecklistAsync()
    {
        var checklist = new
        {
            VeiligheidsChecklistId,
            LadingId,
            IsSchipGoedGemeerd,
            IsDoeltreffendeVerlichtingVerzekerd,
            IsNooduitgangVrij,
            IsSchipWalVerbindingVeilig,
            ZijnLaadarmenVrijBeweegbaar,
            ZijnAansluitingenAfgeblind,
            ZijnLeidingenInGoedeStaat,
            ZijnAlleKleppenGecontroleerd,
            IsAlarmNoodstopBekend,
            ZijnBrandblusApparatenBedrijfsklaar,
            ZijnVerwarmingsapparatenUitgeschakeld,
            IsRookverbodAfgekondigd,
            ZijnRadarEnAndereElektrischeApparatenUit,
            ZijnDeurenEnRamenGesloten,
            IsOvervulbeveiligingBeproefd,
            IsUitschakelingPompVanafWalMogelijk,
            IsGasafvoerleidingCorrectAangesloten,
            IsDrukGasterugvoerleidingVeilig,
            IsVlamkerendeInrichtingAanwezig,
            IsVerblijftijdVastgesteldEnGedocumenteerd,
            IsLaadtemperatuurBinnenToegestaneBandbreedte
        };

        var response = await _httpClient.PostAsJsonAsync("https://localhost:7143/api/VeiligheidsChecklist", checklist);
        if (response.IsSuccessStatusCode)
        {
            _navigationManager.NavigateTo("/checklist-success");
        }
        else
        {
            // Handle error
        }
    }

    public string GetChecklistJson()
    {
        return System.Text.Json.JsonSerializer.Serialize(new
        {
            VeiligheidsChecklistId,
            LadingId,
            IsSchipGoedGemeerd,
            IsDoeltreffendeVerlichtingVerzekerd,
            IsNooduitgangVrij,
            IsSchipWalVerbindingVeilig,
            ZijnLaadarmenVrijBeweegbaar,
            ZijnAansluitingenAfgeblind,
            ZijnLeidingenInGoedeStaat,
            ZijnAlleKleppenGecontroleerd,
            IsAlarmNoodstopBekend,
            ZijnBrandblusApparatenBedrijfsklaar,
            ZijnVerwarmingsapparatenUitgeschakeld,
            IsRookverbodAfgekondigd,
            ZijnRadarEnAndereElektrischeApparatenUit,
            ZijnDeurenEnRamenGesloten,
            IsOvervulbeveiligingBeproefd,
            IsUitschakelingPompVanafWalMogelijk,
            IsGasafvoerleidingCorrectAangesloten,
            IsDrukGasterugvoerleidingVeilig,
            IsVlamkerendeInrichtingAanwezig,
            IsVerblijftijdVastgesteldEnGedocumenteerd,
            IsLaadtemperatuurBinnenToegestaneBandbreedte
        });
    }
}

public class Lading
{
    public int LadingId { get; set; }
    public string ShipName { get; set; }
    public string TerminalName { get; set; }
    public string ProductName { get; set; }
    public double Hoeveelheid { get; set; }
    public DateTime Datum { get; set; }
    public TimeSpan Tijd { get; set; }
}
