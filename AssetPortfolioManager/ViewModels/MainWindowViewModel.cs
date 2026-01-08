using Prism.Mvvm;

namespace AssetPortfolioManager.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Asset Portfolio Manager Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
