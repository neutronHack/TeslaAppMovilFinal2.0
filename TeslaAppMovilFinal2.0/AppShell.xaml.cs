namespace TeslaAppMovilFinal2._0
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent(); // Este SÍ debe estar aquí

            Routing.RegisterRoute("OrdenPage", typeof(OrdenPage));
        }
    }
}
