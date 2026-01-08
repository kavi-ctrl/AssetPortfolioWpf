using AssetPortfolioManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetPortfolioManager.Services
{
    public interface IUpdateAssetService
    {
        Task<bool> UpdateAsset(int id, UpdateAsset model);
    }
}
