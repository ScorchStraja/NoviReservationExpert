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

namespace NoviReservationExpert.View.UserKontrole
{
    /// <summary>
    /// Interaction logic for uc_PrikazSto.xaml
    /// </summary>
    public partial class uc_PrikazSto : UserControl
    {
        public re_Sto sto = new re_Sto();
        public bool zauzetost = false;

        private bool izabransto = false;

        BitmapImage covek_Belo = new BitmapImage();
        BitmapImage covek_Sivo = new BitmapImage();
        BitmapImage sto_Belo = new BitmapImage();
        BitmapImage sto_Sivo = new BitmapImage();
        public uc_PrikazSto(int visinaStola, int sirinaStola,re_Sto sto)
        {
            InitializeComponent();
            this.Height = visinaStola;
            this.Width = sirinaStola;

            this.sto = sto;

            this.tbBrojLjudi.Text = sto.BrojOsoba.ToString();
            if(sto.Sto.Length > 10)
            {
                this.tbImeStola.Text = sto.Sto.Substring(0, 10);
            } else if (sto.Sto.Length < 3)
            {
                this.tbImeStola.FontSize = 20;
                this.tbImeStola.Text = sto.Sto;
            } else
            {
                this.tbImeStola.Text = sto.Sto;
            }

            covek_Belo.BeginInit();
            covek_Belo.UriSource = new Uri("pack://application:,,,/NoviReservationExpert;component/Resursi/Slike/Covek.png");
            covek_Belo.EndInit();
            covek_Sivo.BeginInit();
            covek_Sivo.UriSource = new Uri("pack://application:,,,/NoviReservationExpert;component/Resursi/Slike/Covek_Sivo.png");
            covek_Sivo.EndInit();
            sto_Belo.BeginInit();
            sto_Belo.UriSource = new Uri("pack://application:,,,/NoviReservationExpert;component/Resursi/Slike/Sto_Belo.png");
            sto_Belo.EndInit();
            sto_Sivo.BeginInit();
            sto_Sivo.UriSource = new Uri("pack://application:,,,/NoviReservationExpert;component/Resursi/Slike/Sto_Sivo.png");
            sto_Sivo.EndInit();
        }

        public void ProveriZauzetost()
        {
            zauzetost = Broker.BrokerSelect.dajSesiju().ZauzetostStola(sto);
            if (zauzetost)
            {
                ObrniIzgled();
            }
        }

        public void ProveriBrojLjudi(int brojljudi)
        {
            if(brojljudi > sto.BrojOsoba)
            {
                brdBrojLjudi.Background = (Brush)Application.Current.FindResource("Crvena");
            }
            else
            {
                brdBrojLjudi.Background = Brushes.Transparent;
            }
        }

        public void StoNeispunjavaUslove()
        {
            ObrniIzgled();
        }

        private void ObrniIzgled()
        {
            if(brdRoot.Background == (Brush)Application.Current.FindResource("Crvena"))
            {
                brdRoot.Background = (Brush)Application.Current.FindResource("Bela");
                tbImeStola.Foreground = (Brush)Application.Current.FindResource("Siva");
                tbBrojLjudi.Foreground = (Brush)Application.Current.FindResource("Siva");
                imgSto.Source = sto_Sivo;
                imgCovek.Source = covek_Sivo;
            }
            else
            {
                brdRoot.Background = (Brush)Application.Current.FindResource("Crvena");
                tbImeStola.Foreground = (Brush)Application.Current.FindResource("Bela");
                tbBrojLjudi.Foreground = (Brush)Application.Current.FindResource("Bela");
                imgSto.Source = sto_Belo;
                imgCovek.Source = covek_Belo;
            }
        }
        public void IzabranSto()
        {
            if(brdIzabranSto.Visibility == Visibility.Visible)
            {
                brdIzabranSto.Visibility = Visibility.Hidden;
            }
            else
            {
                brdIzabranSto.Visibility = Visibility.Visible;
            }
        }
    }
}
