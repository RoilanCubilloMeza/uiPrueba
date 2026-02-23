using UiPrueba1.Pages;

namespace UiPrueba1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ClientePage), typeof(ClientePage));
        }
    }
}
