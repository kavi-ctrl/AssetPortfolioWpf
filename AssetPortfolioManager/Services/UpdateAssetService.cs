using AssetPortfolioManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AssetPortfolioManager.Services
{
    public class UpdateAssetService : IUpdateAssetService
    {
        private readonly HttpClient _httpClient;

        public UpdateAssetService()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:7053/")
            };
        }
        public async Task<bool> UpdateAsset(int id, UpdateAsset model)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/assets/{id}", model);
            return response.IsSuccessStatusCode;
        }
    }
}
