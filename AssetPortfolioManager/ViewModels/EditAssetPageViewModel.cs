using AssetPortfolioManager.Model;
using AssetPortfolioManager.Services;
using AssetPortfolioManager.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AssetPortfolioManager.ViewModels
{
    public class EditAssetPageViewModel : BindableBase, INavigationAware
    {
        private readonly IUpdateAssetService _updateAssetService;
        private readonly INotificationService _notificationService;
        private readonly IRegionManager _regionManager;

        private int _assetid;
        private string _assetname;
        private string _assettype;
        private int _quantity;
        private decimal _price;

        public int AssetId { 
            get { return _assetid; }
            set { SetProperty(ref _assetid, value); }
        }
        public string AssetName
        {
            get { return _assetname; }
            set { SetProperty(ref _assetname, value); }
        }
        public string AssetType
        {
            get { return _assettype; }
            set { SetProperty(ref _assettype, value); }
        }
        public int Quantity
        {
            get { return _quantity; }
            set { SetProperty(ref _quantity, value); }
        }
        public Decimal Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand BackCommand { get; }
        public EditAssetPageViewModel(IUpdateAssetService updateAssetService, INotificationService notificationService,
            IRegionManager regionManager)
        {
            _updateAssetService = updateAssetService;
            _notificationService = notificationService;
            _regionManager = regionManager;

            SaveCommand = new DelegateCommand(async () => await ExecuteUpdate())
                .ObservesProperty(() => AssetName).ObservesProperty(() => AssetType)
                .ObservesProperty(() => Quantity).ObservesProperty(() => Price);
            BackCommand = new DelegateCommand(ExecuteBack);
        }
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var data = navigationContext.Parameters.GetValue<AllAssetData>("Data");
            AssetId = data.AssetId;
            AssetName = data.AssetName;
            AssetType = data.AssetType;
            Quantity = data.Quantity;
            Price = data.Price;
        }

        private async Task ExecuteUpdate()
        {
            if (string.IsNullOrWhiteSpace(AssetName))
            {
                _notificationService.ShowError("Asset Name is required.");
                return;
            }
            if (!int.TryParse(Quantity.ToString(), out int quantity) || quantity <= 0)
            {
                _notificationService.ShowError("Quantity must be greater than zero.");
                return;
            }
            if (!decimal.TryParse(Price.ToString(), out decimal price) || price <= 0)
            {
                _notificationService.ShowError("Price must be greater than zero.");
                return;
            }
            try
            {
                var data = new UpdateAsset()
                {
                    AssetId = AssetId,
                    AssetName = AssetName,
                    AssetType = AssetType,
                    Quantity = quantity,
                    Price = price
                };

                var issuccess = await _updateAssetService.UpdateAsset(AssetId, data);
                if (issuccess)
                {
                    _notificationService.ShowSuccess("Asset is Updated Successfully.");
                }
                else
                {
                    _notificationService.ShowError("Db not responding.");
                }
            }
            catch (HttpRequestException ex) { _notificationService.ShowError($"{ex}"); }
            catch (Exception ex) { _notificationService.ShowError($"{ex}"); }
        }

        private void ExecuteBack()
        {
            _regionManager.RequestNavigate("MainRegion", nameof(AllAssetViewPage));
        }

        public bool IsNavigationTarget(NavigationContext navigationContext){return true;}

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        
    }
}
