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
    public class CreateAssetService : ICreateAssetService
    {
        private readonly HttpClient _httpClient;

        public CreateAssetService()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:7053/")
            };
        }
        public async Task<int> CreateNewAsset(CreateAsset model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/assets", model);
            response.EnsureSuccessStatusCode();

            var newid = await response.Content.ReadFromJsonAsync<int>();
            return newid;
        }
    }
}
