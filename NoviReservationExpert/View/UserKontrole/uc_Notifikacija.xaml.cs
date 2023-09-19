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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace NoviReservationExpert.View.UserKontrole
{
    /// <summary>
    /// Interaction logic for uc_Notifikacija.xaml
    /// </summary>
    public partial class uc_Notifikacija : UserControl
    {
        public re_Rezervacija rezervacija;
        bool vidjenanotifikacija = false;
        int _status; // -1 rezervacija je prosla, 0 rezervacija je u toku, 1 rezervacija tek dolazi
        SolidColorBrush animatedBrush = new SolidColorBrush();
        Storyboard myStoryboard = new Storyboard();
        ColorAnimationUsingKeyFrames colorAnimation = new ColorAnimationUsingKeyFrames();
       
        public uc_Notifikacija(re_Rezervacija rezervacije)
        {
            InitializeComponent();

            this.rezervacija = rezervacije;

            brOdraslih.Text = rezervacija.BrojOdraslih.ToString();
            brDece.Text = rezervacija.BrojDece.ToString();
            tbNosiocRezervacije.Text = rezervacija.ImeGosta + " " + rezervacija.PrezimeGosta;
            tbVremeISto.Text = "Sto " + rezervacija.Sto; //+ ", " + rezervacija.VremeOd.ToString("HH:mm") + " - " + rezervacija.VremeDo.ToString("HH:mm");

            if (rezervacija.Status == -1)
            {
                brdStatus.Background = (Brush)Application.Current.FindResource("Otkazano");
            }
            if (rezervacija.Status == 0)
            {
                brdStatus.Background = (Brush)Application.Current.FindResource("Rezervisano");
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

        public void Update(re_Rezervacija update)
        {
            this.rezervacija = update;
            if (rezervacija.Status == -1)
            {
                brdStatus.Background = (Brush)Application.Current.FindResource("Otkazano");
            }
            if (rezervacija.Status == 0)
            {
                brdStatus.Background = (Brush)Application.Current.FindResource("Rezervisano");
            }
            if (rezervacija.Status == 1)
            {
                brdStatus.Background = (Brush)Application.Current.FindResource("UToku");
            }
            if (rezervacija.Status == 2)
            {
                brdStatus.Background = (Brush)Application.Current.FindResource("Zavrseno");
            }
            brOdraslih.Text = rezervacija.BrojOdraslih.ToString();
            brDece.Text = rezervacija.BrojDece.ToString();
            tbNosiocRezervacije.Text = rezervacija.ImeGosta + " " + rezervacija.PrezimeGosta;
            tbVremeISto.Text = "Sto " + rezervacija.Sto; 
        }

        public void VidjenaNotifikacija()
        {
            vidjenanotifikacija = true;
            brdVidjenaNotifikacija.Visibility = Visibility.Hidden;
        }
    }
}
