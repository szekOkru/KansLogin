using LoginForm.Views;

namespace LoginForm
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        }
    }
}
