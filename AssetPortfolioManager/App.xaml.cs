#pragma warning disable CA1416
using AssetPortfolioManager.Services;
using AssetPortfolioManager.Views;
using OfficeOpenXml;
using Prism.Ioc;
using Prism.Regions;
using System.Windows;

namespace AssetPortfolioManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            base.OnStartup(e);
        }
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            var regionmanager = Container.Resolve<IRegionManager>();
            regionmanager.RequestNavigate("MainRegion", nameof(AllAssetViewPage));
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IAssetDataService, AssetData>();
            containerRegistry.Register<ICreateAssetService, CreateAssetService>();
            containerRegistry.Register<INotificationService, NotificationService>();
            containerRegistry.Register<IUpdateAssetService, UpdateAssetService>();
            containerRegistry.Register<IDeleteAssetService, DeleteAssetService>();


            containerRegistry.RegisterForNavigation<AllAssetViewPage>();
            containerRegistry.RegisterForNavigation<AddNewAssetPage>();
            containerRegistry.RegisterForNavigation<EditAssetPage>();
        }
        
    }
}
#pragma warning restore CA1416