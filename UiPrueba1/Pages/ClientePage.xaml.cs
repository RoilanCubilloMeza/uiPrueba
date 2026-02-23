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

        private void OnClientIdCompleted(object sender, EventArgs e)
        {
            if (BindingContext is ClienteViewModel vm)
                vm.SyncCommand.Execute(null);
        }

        private void OnActivityCodeCompleted(object sender, EventArgs e)
        {
            if (BindingContext is ClienteViewModel vm)
                vm.SearchActivityCommand.Execute(null);
        }
    }
}
