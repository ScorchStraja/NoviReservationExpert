using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NoviReservationExpert.View.UserKontrole
{
    /// <summary>
    /// Interaction logic for uc_Meni.xaml
    /// </summary>
    public partial class v_Meni : Window
    {
        public Action ActionNovaRezervacija { get; set; }
        public Action ActionOtvoriDetaljeRezervacije { get; set; }
        public Action ActionAktivirajRezervaciju { get; set; }
        public Action ActionDeaktivirajRezervaciju { get; set; }
        public v_Meni(string vreme, string sto, object kliknutiobjekat)
        {
            InitializeComponent();

            if (kliknutiobjekat is uc_Rezervacija || kliknutiobjekat is uc_Notifikacija)
            {
                btnNovaRezervacija.Visibility = Visibility.Collapsed;
                btnAktiviranje.Visibility = Visibility.Visible;
                btnOtvoriDetaljeRezervacije.Visibility = Visibility.Visible;
                btnDeaktiviranje.Visibility = Visibility.Visible;
            }
            else
            {
                btnNovaRezervacija.Visibility = Visibility.Visible;
                btnAktiviranje.Visibility = Visibility.Collapsed;
                btnOtvoriDetaljeRezervacije.Visibility = Visibility.Collapsed;
                btnDeaktiviranje.Visibility = Visibility.Collapsed;
            }

            this.Vreme.Text = vreme;
            this.Sto.Text = "Sto " + sto;
        }

        private void NovaRezervacija(object sender, RoutedEventArgs e)
        {
            this.ActionNovaRezervacija();
        }

        private void OtvoriDetaljiRezervacije(object sender, RoutedEventArgs e)
        {
            this.ActionOtvoriDetaljeRezervacije();
        }

        private void AktivirajRezervaciju(object sender, RoutedEventArgs e)
        {
            this.ActionAktivirajRezervaciju();
        }
        private void DeaktivirajRezervaciju(object sender, RoutedEventArgs e)
        {
            this.ActionDeaktivirajRezervaciju();
        }

        public void ZatvoriMeni()
        {
            this.Close();
        }
    }
}
