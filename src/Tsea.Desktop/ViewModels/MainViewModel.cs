using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Tsea.Domain.Models;

namespace Tsea.Desktop.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;

        public MainViewModel()
        {
            _httpClient = new HttpClient { BaseAddress = new System.Uri("http://localhost:5000/") };
        }

        [ObservableProperty]
        private ObservableCollection<Equipment> equipments = new();

        [RelayCommand]
        public async Task LoadEquipmentsAsync()
        {
            try
            {
                var data = await _httpClient.GetFromJsonAsync<Equipment[]>("/api/equipments");
                if (data != null)
                {
                    Equipments.Clear();
                    foreach (var item in data)
                    {
                        Equipments.Add(item);
                    }
                }
            }
            catch
            {
            }
        }
    }
}
