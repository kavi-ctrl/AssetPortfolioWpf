using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetPortfolioManager.Model
{
    public class AllAssetData
    {
        public int AssetId { get; set; }
        public string AssetName { get; set; }
        public string AssetType { get; set; }
        public int Quantity { get; set; }
        public Decimal Price { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
