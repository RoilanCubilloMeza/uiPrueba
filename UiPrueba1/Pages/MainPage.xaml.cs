using System.ComponentModel;
using UiPrueba1.ViewModels;

namespace UiPrueba1
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _vm;

        public MainPage()
        {
            InitializeComponent();
            _vm = new MainViewModel();
            BindingContext = _vm;
            _vm.PropertyChanged += OnViewModelPropertyChanged;
        }

        private double _panelWidth;

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainViewModel.IsProductsPanelVisible))
            {
                if (_vm.IsProductsPanelVisible)
                {
                    MainGrid.ColumnDefinitions[0].Width = new GridLength(560);
                    MainGrid.ColumnDefinitions[1].Width = GridLength.Star;
                    MainGrid.ColumnDefinitions[2].Width = new GridLength(220);
                }
                else
                {
                    MainGrid.ColumnDefinitions[0].Width = new GridLength(3, GridUnitType.Star);
                    MainGrid.ColumnDefinitions[1].Width = new GridLength(56);
                    MainGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
                }
            }
            else if (e.PropertyName == nameof(MainViewModel.PreferredSpan))
            {
                UpdateProductsSpan();
            }
        }

        private void OnProductsPanelSizeChanged(object sender, EventArgs e)
        {
            if (sender is VisualElement el && el.Width > 0)
            {
                _panelWidth = el.Width;
                UpdateProductsSpan();
            }
        }

        private void UpdateProductsSpan()
        {
            if (_panelWidth <= 0) return;
            int maxColumns = _panelWidth switch
            {
                <= 360 => 1,
                <= 580 => 2,
                <= 860 => 3,
                _      => 4
            };
            _vm.MaxSpan = maxColumns;
            ProductsItemsLayout.Span = Math.Max(1, Math.Min(_vm.PreferredSpan, maxColumns));
        }

        private async void OnClientSearchTapped(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//ClientePage");
        }
    }
}
