using NoviReservationExpert.Model;
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

namespace NoviReservationExpert.View
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class uc_Rezervacija : UserControl
    {
        re_Rezervacija rezervacija;
        public re_Rezervacija Rezervacija
        {
            get
            {
                return rezervacija;
            }
        }
        public string ucrez_sto;
        public Action OsveziRadniProstor { get; set; }

        public bool zamracen = false;
        public bool pomerenobelezicac = false;
        public uc_Rezervacija(re_Rezervacija rezervacija, string sto)
        {
            InitializeComponent();
            ucrez_sto = sto; //ovo radi tako sto u radnom prostoru, rezervaciju koja ima vise stolova (1,2,3,4) podelima na 4 rezervacije u kojoj svaka ima svoj sto, kad radim updejt, vrsim REPLACE, gde umesto ucrez_sto pisem novi sto u stringu. Npr. rezervacija.sto = 1,2,3,4 , kad zavrsim pomeranje menjam 3 -> 5 
            this.rezervacija = rezervacija;
            tbBrojOdraslih.Text = rezervacija.BrojOdraslih.ToString();
            tbBrojDece.Text = rezervacija.BrojDece.ToString();
            PostaviSirinuUOdnosuNaTrajanje(rezervacija.VremeOd, rezervacija.VremeDo);
            PromeniSto(sto);
            PromeniVreme(rezervacija.VremeOd, rezervacija.VremeDo);
            PromeniNapomenu(rezervacija.Napomena);
            PromeniImeGosta(rezervacija.ImeGosta + " " + rezervacija.PrezimeGosta);
        }

        public void ZamraciRezervaciju()
        {
            //brdZamracenje.Opacity = 0.5;
            //rootGrid.Opacity = 0.3;
            brdRoot.Background = Brushes.DarkSalmon;
            brdBrojOsoba.Background = Brushes.DarkSalmon;
            brdObelezivac.Background = Brushes.DarkSalmon;
            brdObelezivac.Visibility = Visibility.Visible;

            Panel.SetZIndex(brdObelezivac, 99999);

            zamracen = true;
        }
        public void OtkrijRezervaciju()
        {
            //brdZamracenje.Opacity = 0;
            //rootGrid.Opacity = 1;
            brdRoot.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#284b63");
            brdBrojOsoba.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#284b63");
            brdObelezivac.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#284b63");
            if (!pomerenobelezicac)
            {
                brdObelezivac.Visibility = Visibility.Hidden;
            }
            Panel.SetZIndex(brdObelezivac, 99999);
            zamracen = false;
        }
        public void PomeriObelezivac()
        {
            brdObelezivac.Margin = new Thickness(brdObelezivac.Margin.Left + 30, -20, 0, 0);
            pomerenobelezicac = true;
        }
        private void PromeniImeGosta(string imeprezime)
        {
            tbImeIPrezime.Text = imeprezime;
        }
        public void PostaviSirinuUOdnosuNaTrajanje(DateTime vremeOd, DateTime vremeDo)
        {
            int ukupnoOdMin = vremeOd.Hour * 60 + vremeOd.Minute;
            int ukupnoDoMin = vremeDo.Hour * 60 + vremeDo.Minute;
            int razlika = Math.Abs(ukupnoDoMin - ukupnoOdMin);
            this.Width = razlika * 2;
        }
        public void PromeniSto(string sto)
        {
            tbSto.Text = "Sto - " + sto;
        }           
        public void PromeniVreme(DateTime vremeOd, DateTime vremeDo)
        {
            tbVreme.Text = vremeOd.ToString("HH:mm") + " - " + vremeDo.ToString("HH:mm");
        }
        public void PromeniNapomenu(string napomena)
        {
            tbNapomena.Text = napomena;
        }
        private void tbNapomena_LostFocus(object sender, RoutedEventArgs e)
        {
            rezervacija.Napomena = tbNapomena.Text;
            Broker.BrokerUpdate.dajSesiju().UpdateNapomenu(rezervacija.Id, tbNapomena.Text);
        }
        public void ProveraKapacitetaStola(re_Sto sto) 
        {
            if (sto == null) return;
            if(sto.BrojOsoba < rezervacija.BrojOdraslih)
            {
                brdBrojOsoba.Background = Brushes.Red;
            } else
            {
                brdBrojOsoba.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#284b63");
            }
        }
        private void PrikaziRezervaciju(object sender, MouseButtonEventArgs e)
        {
            OtkrijRezervaciju();
        }
        public void StaviRezervacijuUPozadinu()
        {
            brdZamracenje.Opacity = 0.7;
        }
        public void RezervacijaSePomera()
        {
            brdRoot.Opacity = 0.6;
        }
        public void RezervacijaJePrestalaDaSePomera()
        {
            brdRoot.Opacity = 1;
        }
    }
}
