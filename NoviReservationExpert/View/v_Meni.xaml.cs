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
        public v_Meni(string vreme,string sto)
        {
            InitializeComponent();

            this.Vreme.Text = vreme;
            this.Sto.Text = "Sto " + sto;
        }

        private void NovaRezervacija(object sender, RoutedEventArgs e)
        {
            this.ActionNovaRezervacija();
        }

        public void ZatvoriMeni()
        {
            this.Close();
        }
    }
}
