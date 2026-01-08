using AssetPortfolioManager.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AssetPortfolioManager.Services
{
    public class AssetData : IAssetDataService
    {
        private readonly HttpClient _httpClient;

        public AssetData()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7053/")
            };
        }

        public async Task<List<AllAssetData>> GetAllAssetData()
        {
            var response = await _httpClient.GetAsync("api/Assets");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<AllAssetData>>(json);
        }
    }
}
