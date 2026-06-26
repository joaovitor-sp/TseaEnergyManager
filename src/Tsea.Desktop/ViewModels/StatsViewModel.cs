using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Tsea.Desktop.ViewModels
{
    public partial class StatsViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;

        public StatsViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new System.Uri("http://localhost:5000/") };
        }

        [ObservableProperty]
        private string totalEquipments = "Clique em atualizar";

        [RelayCommand]
        public async Task RefreshStatsAsync()
        {
            try
            {
                TotalEquipments = "Calculando...";
                var data = await _httpClient.GetFromJsonAsync<Tsea.Domain.Models.Equipment[]>("/api/equipments");
                if (data != null)
                {
                    TotalEquipments = $"Total: {data.Length} equipamentos salvos";
                }
            }
            catch
            {
                TotalEquipments = "Erro ao ler API";
            }
        }
    }
}
