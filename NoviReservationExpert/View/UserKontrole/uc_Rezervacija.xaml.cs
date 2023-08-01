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
        public Action OsveziRadniProstor { get; set; }

        public uc_Rezervacija(re_Rezervacija rezervacija)
        {
            InitializeComponent();
           
            this.rezervacija = rezervacija;
            tbBrojOdraslih.Text = rezervacija.BrojOdraslih.ToString();
            tbBrojDece.Text = rezervacija.BrojDece.ToString();
            PostaviSirinuUOdnosuNaTrajanje(rezervacija.VremeOd, rezervacija.VremeDo);
            PromeniSto(rezervacija.Sto);
            PromeniVreme(rezervacija.VremeOd, rezervacija.VremeDo);
            PromeniNapomenu(rezervacija.Napomena);
            PromeniImeGosta(rezervacija.ImeGosta + " " + rezervacija.PrezimeGosta);
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
    }
}
