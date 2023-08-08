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
        re_Rezervacija rezervacija;
        int _status; // -1 rezervacija je prosla, 0 rezervacija je u toku, 1 rezervacija tek dolazi
        SolidColorBrush animatedBrush = new SolidColorBrush();
        Storyboard myStoryboard = new Storyboard();
        ColorAnimationUsingKeyFrames colorAnimation = new ColorAnimationUsingKeyFrames();
        public int Status
        {
            get
            {
                return _status;
            }
            set 
            { 
                _status = value;
                if(_status == -1)
                {
                    brdRoot.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#d11a2a");
                }
                if (_status == 0)
                {
                    ColorAnimation animation;
                    animation = new ColorAnimation();
                    animation.From = (Color)ColorConverter.ConvertFromString("#284b63");
                    animation.To = (Color)ColorConverter.ConvertFromString("#427aa1");
                    animation.Duration = new Duration(TimeSpan.FromSeconds(1));
                    animation.AutoReverse = true;
                    animation.RepeatBehavior = RepeatBehavior.Forever;
                    this.brdRoot.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                }
                if (_status == 1)
                {
                    brdRoot.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#284b63");
                }
            }
        }

        public uc_Notifikacija(re_Rezervacija rezervacije)
        {
            InitializeComponent();

            this.rezervacija = rezervacije;

            brOdraslih.Text = rezervacija.BrojOdraslih.ToString();
            brDece.Text = rezervacija.BrojDece.ToString();
            tbNosiocRezervacije.Text = rezervacija.ImeGosta + " " + rezervacija.PrezimeGosta;
            tbVremeISto.Text = "Sto " + rezervacija.Sto + ", " + rezervacija.VremeOd.ToString("HH:mm") + " - " + rezervacija.VremeDo.ToString("HH:mm");
            tbObjekat.Text = rezervacija.Sema;
          
            if (rezervacija.VremeOd > DateTime.Now)
            {
                Status = -1;
            }
            if (rezervacija.VremeOd < DateTime.Now && rezervacija.VremeDo > DateTime.Now)
            {
                Status = 0;
            }
            if (rezervacija.VremeDo < DateTime.Now)
            {
                Status = 1;
            }
        }



        public void Update()
        {
            rezervacija = Broker.BrokerSelect.dajSesiju().VratiRezervacijuPoId(rezervacija.Id);
            brOdraslih.Text = rezervacija.BrojOdraslih.ToString();
            brDece.Text = rezervacija.BrojDece.ToString();
            tbNosiocRezervacije.Text = rezervacija.ImeGosta + " " + rezervacija.PrezimeGosta;
            tbVremeISto.Text = "Sto " + rezervacija.Sto + ", " + rezervacija.VremeOd.ToString("HH:mm") + " - " + rezervacija.VremeDo.ToString("HH:mm");
            tbObjekat.Text = rezervacija.Sema;         
            if (rezervacija.VremeOd > DateTime.Now)
            {
                Status = -1;
            }
            if (rezervacija.VremeOd < DateTime.Now && rezervacija.VremeDo > DateTime.Now)
            {
                Status = 0;
            }
            if (rezervacija.VremeDo < DateTime.Now)
            {
                Status = 1;
            }
        }
    }
}
