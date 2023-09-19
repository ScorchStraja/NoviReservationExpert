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

        public bool zamracen = false;
        public bool pomerenobelezicac = false;

        // ------------------------------STATUSI----------------------------
        // -1 -----------> OTKAZANA
        // 0  -----------> REZERVISANA 
        // 1  -----------> U TOKU
        // 2  -----------> ZAVRSENA
        public uc_Rezervacija(re_Rezervacija rezervacija, string sto)
        {
            InitializeComponent();

            if(rezervacija.Status == -1)
            {
                brdStatus.Background = (Brush)Application.Current.FindResource("Otkazano");
            }
            if(rezervacija.Status == 0)
            {
                brdStatus.Background = Brushes.Transparent;
            }
            if(rezervacija.Status == 1)
            {
                brdStatus.Background = (Brush)Application.Current.FindResource("UToku");
            }
            if (rezervacija.Status == 2)
            {
                brdStatus.Background = (Brush)Application.Current.FindResource("Zavrseno");
            }

            ucrez_sto = sto; //ovo radi tako sto u radnom prostoru, rezervaciju koja ima vise stolova (1,2,3,4) podelima na 4 rezervacije u kojoj svaka ima svoj sto, kad radim updejt, vrsim REPLACE, gde umesto ucrez_sto pisem novi sto u stringu. Npr. rezervacija.sto = 1,2,3,4 , kad zavrsim pomeranje menjam 3 -> 5 
            this.rezervacija = rezervacija;
            
            PromeniBrojGostiju(rezervacija.BrojOdraslih.ToString());
            PromeniBrojDece(rezervacija.BrojDece.ToString());
            PostaviSirinuUOdnosuNaTrajanje(rezervacija.VremeOd, rezervacija.VremeDo);
            PromeniNapomenu(rezervacija.Napomena);
            PromeniImeGosta(rezervacija.ImeGosta,rezervacija.PrezimeGosta);
        }

        public void PromeniBrojGostiju(string v)
        {
            tbBrojOdraslih.Text = v;
        }
        public void PromeniBrojDece(string v)
        {
            tbBrojDece.Text = v;
        }
        public void PomeriObelezivac()
        {
            brdObelezivac.Margin = new Thickness(brdObelezivac.Margin.Left + 30, -20, 0, 0);
            pomerenobelezicac = true;
        }
        private void PromeniImeGosta(string ime, string prezime)
        {
            tbIme.Text = ime;
            tbPrezime.Text = prezime;
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
            //tbSto.Text = "Sto - " + sto;
        }           
        public void PromeniVreme(DateTime vremeOd, DateTime vremeDo)
        {
            //tbVreme.Text = vremeOd.ToString("HH:mm") + " - " + vremeDo.ToString("HH:mm");
        }
        public void PromeniNapomenu(string napomena)
        {
            tbNapomena.Text = napomena;
        }
        public void ProveraKapacitetaStola(re_Sto sto) 
        {
            if (sto == null) return;
            if(sto.BrojOsoba < rezervacija.BrojOdraslih)
            {
                brdVisakLjudi.Visibility = Visibility.Visible;
            } else
            {
                brdVisakLjudi.Visibility = Visibility.Hidden;
            }
        }
        public void RezervacijaSePomera()
        {
            brdRoot.Opacity = 0.6;
        }
        public void RezervacijaJePrestalaDaSePomera()
        {
            brdRoot.Opacity = 1;
        }
        public void Preklapanje()
        {
            brdObelezivac.Visibility = Visibility.Visible;
            brdObelezivac.Background = (Brush)Application.Current.FindResource("Preklapanje");
            brdRoot.Background = (Brush)Application.Current.FindResource("Preklapanje");
            zamracen = true;
        }
        public void OtkrijObelezivac()
        {
            brdObelezivac.Visibility = Visibility.Visible;
        }
        public void UpdateIzgledUOdnosuNaStatus()
        {
            if (rezervacija.Status == -1)
            {
                brdStatus.Background = (Brush)Application.Current.FindResource("Otkazano");
            }
            if (rezervacija.Status == 0)
            {
                brdStatus.Background = Brushes.Transparent;
            }
            if (rezervacija.Status == 1)
            {
                brdStatus.Background = (Brush)Application.Current.FindResource("UToku");
            }
            if (rezervacija.Status == 2)
            {
                brdStatus.Background = (Brush)Application.Current.FindResource("Zavrseno");
            }
        }

    }
}
