using AssetPortfolioManager.Model;
using AssetPortfolioManager.Services;
using AssetPortfolioManager.Views;
using DevExpress.XtraRichEdit.Fields;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AssetPortfolioManager.ViewModels
{
    public class AddNewAssetPageViewModel : BindableBase
    {
        private readonly ICreateAssetService _createAssetService;
        private readonly IRegionManager _regionManager;
        private readonly INotificationService _notificationService;

        private string _assetname;
        private string _assettype;
        private int _quantity;
        private decimal _price;

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
        public ObservableCollection<string> AssetTypes { get; }
        public AddNewAssetPageViewModel(ICreateAssetService createAssetService, IRegionManager regionManager, INotificationService notificationService = null)
        {
            _createAssetService = createAssetService;
            _regionManager = regionManager;
            _notificationService = notificationService;

            SaveCommand = new DelegateCommand(async () => await ExecuteSaveDate())
                .ObservesProperty(() => AssetName)
                .ObservesProperty(() => Quantity).ObservesProperty(() => Price);
            BackCommand = new DelegateCommand(ExecuteBackCommand);
            AssetTypes = new ObservableCollection<string>{ "Equity", "Bond", "ETF" };
            AssetType = AssetTypes.First();
        }

        public async Task ExecuteSaveDate()
        {
            if (string.IsNullOrWhiteSpace(AssetName))
            {
                _notificationService.ShowError("Asset Name is required.");
                return;
            }
            if (!int.TryParse(Quantity.ToString(), out int quantity) || quantity <= 0)
            {
                _notificationService.ShowError("Quantity must be Numeric and greater than zero.");
                return;
            }
            if (!decimal.TryParse(Price.ToString(), out decimal price) || price <= 0)
            {
                _notificationService.ShowError("Price must be Numeric and greater than zero.");
                return;
            }
            var model = new CreateAsset()
            {
                AssetName = AssetName, AssetType = AssetType, Quantity = quantity, Price = price
            };

            try
            {
                var id = await _createAssetService.CreateNewAsset(model);

                _notificationService.ShowSuccess($"Asset {id} Created Successfully.");
            }
            catch(HttpRequestException ex)
            {
                _notificationService.ShowError("Server is not responding.");
            }
            catch (Exception ex)
            {
                _notificationService.ShowError(ex.Message);
            }
        }
        
        private void ExecuteBackCommand()
        {
            _regionManager.RequestNavigate("MainRegion", nameof(AllAssetViewPage));
        }
    }
}
