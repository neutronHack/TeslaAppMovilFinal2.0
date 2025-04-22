namespace TeslaAppMovilFinal2._0
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Items.Clear();
            Items.Add(new FlyoutItem
            {
                Title = "Home",
                Icon = "home.png",
                Items =
            {
                new ShellContent
                {
                    Title = "Home",
                    ContentTemplate = new DataTemplate(typeof(MainPage)),
                    Route = "MainPage"
                }
            }
            });

            if (Helpers.SessionManager.UsuarioActual != null)
            {
                AgregarPestañaMisCompras();
                AgregarPestañaMisReservas();
                AgregarCerrarSesion();
            }
            else
            {
                AgregarLogin();
                AgregarRegistro();
            }
        }

        private void AgregarLogin()
        {
            Items.Add(new FlyoutItem
            {
                Title = "Iniciar sesión",
                Icon = "login_icon.png",
                Items =
            {
                new ShellContent
                {
                    Title = "Iniciar sesión",
                    ContentTemplate = new DataTemplate(typeof(LoginPage)),
                    Route = "LoginPage"
                }
            }
            });
        }

        private void AgregarRegistro()
        {
            Items.Add(new FlyoutItem
            {
                Title = "Registrarse",
                Icon = "register_icon.png",
                Items =
            {
                new ShellContent
                {
                    Title = "Registrarse",
                    ContentTemplate = new DataTemplate(typeof(RegisterPage)),
                    Route = "RegisterPage"
                }
            }
            });
        }

        private void AgregarCerrarSesion()
        {
            Items.Add(new MenuItem
            {
                Text = "Cerrar sesión",
                IconImageSource = "logout_icon.png",
                Command = new Command(async () =>
                {
                    bool confirmar = await App.Current.MainPage.DisplayAlert("Cerrar sesión", "¿Estás seguro que deseas cerrar sesión?", "Sí", "No");
                    if (confirmar)
                    {
                        Helpers.SessionManager.UsuarioActual = null;

                        // Reiniciar AppShell para mostrar opciones actualizadas
                        Application.Current.MainPage = new AppShell();
                    }
                })
            });
        }

        private void AgregarPestañaMisCompras()
        {
            var misComprasItem = new FlyoutItem
            {
                Title = "Mis Compras",
                Icon = "compras_icon.png"
            };

            misComprasItem.Items.Add(new ShellContent
            {
                Title = "Mis Compras",
                ContentTemplate = new DataTemplate(typeof(MisComprasPage)),
                Route = "MisComprasPage"
            });

            Items.Add(misComprasItem);
        }

        private void AgregarPestañaMisReservas()
        {
            var reservasItem = new FlyoutItem
            {
                Title = "Mis Reservas",
                Icon = "reservas_icon.png"
            };

            reservasItem.Items.Add(new ShellContent
            {
                Title = "Mis Reservas",
                ContentTemplate = new DataTemplate(typeof(MisReservasPage)),
                Route = "MisReservasPage"
            });

            Items.Add(reservasItem);
        }
    }
}
