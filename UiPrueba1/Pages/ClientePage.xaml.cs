using UiPrueba1.ViewModels;

namespace UiPrueba1.Pages
{
    public partial class ClientePage : ContentPage
    {
        public ClientePage()
        {
            InitializeComponent();
            BindingContext = new ClienteViewModel();
        }

        private void OnClienteIdCompleted(object sender, EventArgs e)
        {
            if (BindingContext is ClienteViewModel vm)
                vm.SincronizarCommand.Execute(null);
        }

        private void OnCodActividadCompleted(object sender, EventArgs e)
        {
            if (BindingContext is ClienteViewModel vm)
                vm.BuscarActividadCommand.Execute(null);
        }
    }
}
