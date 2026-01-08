#pragma warning disable CA1416
using AssetPortfolioManager.Model;
using AssetPortfolioManager.Services;
using AssetPortfolioManager.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using OfficeOpenXml;
using System.Windows;

namespace AssetPortfolioManager.ViewModels
{
    public class AllAssetViewPageViewModel : BindableBase
    {
        private readonly IAssetDataService _assetDataService;
        private readonly IRegionManager _regionManager;
        private readonly INotificationService _notificationService;
        private readonly IDeleteAssetService _deleteAssetService;
        private readonly ICreateAssetService _createAssetService;

        private bool _isbusy;

        private ObservableCollection<AllAssetData> _assetdata;
        public ObservableCollection<AllAssetData> AssetData
        {
            get { return _assetdata; }
            set{SetProperty(ref _assetdata, value);}
        }
        public DelegateCommand RefreshCommand { get; }
        public DelegateCommand AddCommand { get; }
        public DelegateCommand<AllAssetData> EditCommand { get; }
        public DelegateCommand<AllAssetData> DeleteCommand { get; }
        public DelegateCommand<object> ExportCommand { get; }
        public DelegateCommand ImportCommand { get; }
        public bool IsBusy
        {
            get { return _isbusy; }
            set { SetProperty(ref _isbusy, value); }
        }

        public AllAssetViewPageViewModel(IAssetDataService assetDataService, IRegionManager regionManager,
            INotificationService notificationService, IDeleteAssetService deleteAssetService,
            ICreateAssetService createAssetService)
        {
            _assetDataService = assetDataService;
            _regionManager = regionManager; 
            _notificationService = notificationService;
            _deleteAssetService = deleteAssetService;
            _createAssetService = createAssetService;

            AssetData = new ObservableCollection<AllAssetData>();
            //LoadData();

            RefreshCommand = new DelegateCommand(ExecuteRefresh);
            AddCommand = new DelegateCommand(ExecuteAdd);
            EditCommand = new DelegateCommand<AllAssetData>(ExecuteEdit);
            DeleteCommand = new DelegateCommand<AllAssetData>(ExecuteDelete);
            ExportCommand = new DelegateCommand<object>(ExecuteExportToExcel);
            ImportCommand = new DelegateCommand(ExecuteImportExcel);
        }

        public async void LoadData()
        {
            IsBusy = true;
            try
            {
                var data = await _assetDataService.GetAllAssetData();
                AssetData.Clear();
                foreach (var i in data)
                {
                    AssetData.Add(i);
                }
            }
            catch (HttpRequestException ex) { _notificationService.ShowError("The Server is Not Responding."); }
            catch (Exception ex)
            {
                _notificationService.ShowError($"{ex}");
            }
            finally { IsBusy = false; }
        }

        private void ExecuteRefresh() {
            LoadData();
            _notificationService.ShowSuccess("The Data is Loaded");
        }

        private void ExecuteAdd() {
            IsBusy = true;
            _regionManager.RequestNavigate("MainRegion",nameof(AddNewAssetPage));
            IsBusy = false;
        }

        private void ExecuteEdit(AllAssetData data)
        {
            IsBusy = true;
            var param = new NavigationParameters
            {
                { "Data", data}
            };
            _regionManager.RequestNavigate("MainRegion", nameof(EditAssetPage), param);
            IsBusy = false;
        }

        private async void ExecuteDelete(AllAssetData data)
        {
            if (data == null)
                return;

            var result = MessageBox.Show(
                $"Are you sure you want to delete {data.AssetName}?", "Confirm Delete",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;
            IsBusy = true;
            try
            {
                var issuccess = await _deleteAssetService.DeleteAsset(data.AssetId); 
                if (issuccess)
                {
                    LoadData();
                    _notificationService.ShowSuccess("Asset is Deleted Successfully.");
                }
                else
                {
                    _notificationService.ShowError("Db not responding.");
                }
            }
            catch (HttpRequestException ex) { _notificationService.ShowError($"{ex}"); }
            catch (Exception ex) { _notificationService.ShowError($"{ex}"); }
            finally { IsBusy = false; }
        }

        public void ExecuteExportToExcel(object gridobj)
        {
            IsBusy = true;
            if(gridobj is DevExpress.Xpf.Grid.GridControl grid)
            {
                string folder_path = @"E:\AssetsData";
                string file_path = Path.Combine(folder_path, "All_Asset_Data.xlsx");
                if (!Directory.Exists(folder_path))
                {
                    Directory.CreateDirectory(folder_path);
                }
                grid.View.ExportToXlsx(file_path);

                IsBusy = false;
                if (File.Exists(file_path)) { _notificationService.ShowSuccess("File Download Successfully."); }
                else { _notificationService.ShowError("File Not Found."); }
            }
        }

        public void ExecuteImportExcel()
        {
            var dialog = new OpenFileDialog { Filter = "Excel Files|*.xlsx" };
            if (dialog.ShowDialog() != true)
                return;
            ImportExcel(dialog.FileName);
        }
        public async void ImportExcel(string filepath)
        {
            IsBusy = true;
            try
            {

                using var package = new ExcelPackage(new FileInfo(filepath));
                var worksheet = package.Workbook.Worksheets[0];

                AssetData.Clear();
                int rowcount = worksheet.Dimension.Rows;
                for (int i = 2; i <= rowcount; i++)
                {
                    var model = new CreateAsset()
                    {
                        AssetName = worksheet.Cells[i,1].Text,
                        AssetType = worksheet.Cells[i, 2].Text,
                        Quantity = int.Parse(worksheet.Cells[i, 3].Text),
                        Price = decimal.Parse(worksheet.Cells[i, 4].Text)
                    };
                    var newid = await _createAssetService.CreateNewAsset(model);

                    _notificationService.ShowSuccess($"id : {newid}");

                    AssetData.Add(new AllAssetData
                    {
                        AssetName = worksheet.Cells[i, 1].Text,
                        AssetType = worksheet.Cells[i, 2].Text,
                        Quantity = int.Parse(worksheet.Cells[i, 3].Text),
                        Price = decimal.Parse(worksheet.Cells[i, 4].Text),
                        LastUpdated = DateTime.Now
                    });
                }
                _notificationService.ShowSuccess("Excel is Imported SuccessFully.");
            }

            catch (Exception ex) { _notificationService.ShowError($"{ex}"); }
            finally { IsBusy = false; }
        }
    }
}
