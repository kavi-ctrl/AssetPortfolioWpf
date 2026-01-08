using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AssetPortfolioManager.Services
{
    public class DeleteAssetService : IDeleteAssetService
    {
        private readonly HttpClient _httpClient;

        public DeleteAssetService()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:7053/")
            };
        }
        public async Task<bool> DeleteAsset(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/assets/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
