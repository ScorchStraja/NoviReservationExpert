using System;
using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;

namespace NoviReservationExpert
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IConfiguration _config { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            try
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                _config = builder.Build();

                string str2 = _config.GetSection("PsConfig")["ImeServera"];
                string str3 = _config.GetSection("PsConfig")["ImeBaze"];


                Globalno.Varijable.RadniServer = str2; //vratiInfo("Server");
                Globalno.Varijable.RadnaBaza = str3; //vratiInfo("Baza");
            }
            catch (Exception s)
            {

            }

            Broker.DBBroker.dajSesiju().kreirajKonekcioniString();

            if (!provereOkruzenja())
            {
                MessageBox.Show("Okruženje nije dobro podešeno!" + "\n" + "Aplikacija će biti ugašena.", "ProSoft Sistem", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }
            else
            {
                View.v_Login prozor = new View.v_Login();
                prozor.ShowDialog();
            }
        }

        private bool provereOkruzenja()
        {
            try
            {
                // da li su uneti svi parametri
                bool zaVracanje = true;
                if (String.IsNullOrEmpty(Globalno.Varijable.RadnaBaza) ||
                    String.IsNullOrEmpty(Globalno.Varijable.RadniServer))
                {
                    zaVracanje = false;
                }

                // da li je moguca konekcija sa unetim parametrima
                if (!Broker.DBBroker.dajSesiju().proveriKonekciju()) { zaVracanje = false; }

                return zaVracanje;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
