using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetPortfolioManager.Services
{
    public interface IDeleteAssetService
    {
        Task<bool> DeleteAsset(int id);
    }
}
