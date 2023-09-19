using NoviReservationExpert.Broker;
using NoviReservationExpert.Model;
using NoviReservationExpert.View;
using NoviReservationExpert.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace NoviReservationExpert.ViewModel
{
    public class vm_Login : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


        #region komande
        public Action ZatvoriFormu { get; set; }
        //public Action SkloniFormu { get; set; }
        //public Action PrikaziFormu { get; set; }
        public RelayCommand Prijava_Command { get; private set; }
        public RelayCommand Zatvori_Command { get; private set; }
        public RelayCommand UgasiTastaturu_Command { get; private set; }
        public RelayCommand PrikaziPinTastatura_Command { get; private set; }
        public RelayCommand PrikaziUserTastatura_Command { get; private set; }
        public RelayCommand PadajuciMeni_Command { get; private set; }
        public RelayCommand PrikaziTastaturuBrojevi_Command { get; private set; }
        public RelayCommand PrikaziTastaturuSlova_Command { get; private set; }
        public RelayCommand TasterKlik_Command { get; private set; }
        public RelayCommand TastaturaObrisi_Command { get; private set; }
        public RelayCommand UnosSaTastature_Command { get; private set; }
        public RelayCommand PrijavaEnter_Command { get; private set; }
        #endregion

        #region propertiji

        string _sifra;
        public string sifra
        {
            get
            {
                return _sifra;
            }
            set
            {
                _sifra = value;
                NotifyPropertyChanged(nameof(sifra));
            }
        }

        Button _btnPin;
        public Button btnPin
        {
            get
            {
                return _btnPin;
            }
            set
            {
                _btnPin = value;
                NotifyPropertyChanged(nameof(btnPin));
            }
        }

        Button _btnUser;
        public Button btnUser
        {
            get
            {
                return _btnUser;
            }
            set
            {
                _btnUser = value;
                NotifyPropertyChanged(nameof(btnUser));
            }
        }

        Border _brdPadajuciMeni;
        public Border brdPadajuciMeni
        {
            get
            {
                return _brdPadajuciMeni;
            }
            set
            {
                _brdPadajuciMeni = value;
            }
        }

        ObservableCollection<re_Objekat> _listaObjekata;
        public ObservableCollection<re_Objekat> listaObjekata
        {
            get
            {
                return _listaObjekata;
            }
            set
            {
                _listaObjekata = value;
                NotifyPropertyChanged(nameof(listaObjekata));
            }
        }

        TextBlock _tbIzabraniObj;
        public TextBlock tbIzabraniObj
        {
            get
            {
                return _tbIzabraniObj;
            }
            set
            {
                _tbIzabraniObj = value;
                NotifyPropertyChanged(nameof(tbIzabraniObj));
            }
        }

        re_Objekat _izabraniObj;
        public re_Objekat izabraniObj
        {
            get
            {
                return _izabraniObj;
            }
            set
            {
                _izabraniObj = value;
                NotifyPropertyChanged(nameof(izabraniObj));
            }
        }


        Grid _tasteriBrojevi;
        public Grid tasteriBrojevi
        {
            get
            {
                return _tasteriBrojevi;
            }
            set
            {
                _tasteriBrojevi = value;
                NotifyPropertyChanged(nameof(tasteriBrojevi));
            }
        }

        Grid _tasteriMalaSlova;
        public Grid tasteriMalaSlova
        {
            get
            {
                return _tasteriMalaSlova;
            }
            set
            {
                _tasteriMalaSlova = value;
                NotifyPropertyChanged(nameof(tasteriMalaSlova));
            }
        }

        Grid _tasteriVelikaSlova;
        public Grid tasteriVelikaSlova
        {
            get
            {
                return _tasteriVelikaSlova;
            }
            set
            {
                _tasteriVelikaSlova = value;
                NotifyPropertyChanged(nameof(tasteriVelikaSlova));
            }
        }

        Visibility _prikazTastatura = Visibility.Visible;
        public Visibility prikazTastatura
        {
            get
            {
                return _prikazTastatura;
            }
            set
            {
                _prikazTastatura = value;
                NotifyPropertyChanged(nameof(prikazTastatura));
            }
        }

        Visibility _prikazPinTastatura = Visibility.Visible;
        public Visibility prikazPinTastatura
        {
            get
            {
                return _prikazPinTastatura;
            }
            set
            {
                _prikazPinTastatura = value;
                NotifyPropertyChanged(nameof(prikazPinTastatura));
            }
        }

        PasswordBox _pb;
        public PasswordBox pb
        {
            get
            {
                return _pb;
            }
            set
            {
                _pb = value;
                NotifyPropertyChanged(nameof(pb));
            }
        }

        PasswordBox _pbPin;
        public PasswordBox pbPin
        {
            get
            {
                return _pbPin;
            }
            set
            {
                _pbPin = value;
                NotifyPropertyChanged(nameof(pbPin));
            }
        }
        #endregion

        public vm_Login(Button btnPin, Button btnUser, Border brdPadajuciMeni, Grid tasteriBrojevi, Grid tasteriMalaSlova, Grid tasteriVelikaSlova, PasswordBox pb, PasswordBox pbPin)
        {
            Globalno.Varijable.otvorenaTastaturaLogin = false;

            listaObjekata = BrokerSelect.dajSesiju().vratiObjektePoSifri();

            this.pb = pb;
            this.pbPin = pbPin;

            this.btnPin = btnPin;
            this.btnUser = btnUser;
            this.brdPadajuciMeni = brdPadajuciMeni;
            tbIzabraniObj = new TextBlock();
            izabraniObj = null;
            this.tasteriBrojevi = tasteriBrojevi;
            this.tasteriMalaSlova = tasteriMalaSlova;
            this.tasteriVelikaSlova = tasteriVelikaSlova;

            Prijava_Command = new RelayCommand(Prijava_Metoda);
            Zatvori_Command = new RelayCommand(Zatvori_Metoda);
            PadajuciMeni_Command = new RelayCommand(PadajuciMeni_Metoda);
            PrikaziPinTastatura_Command = new RelayCommand(PrikaziPinTastatura_Metoda);
            PrikaziUserTastatura_Command = new RelayCommand(PrikaziUserTastatura_Metoda);
            PrikaziTastaturuBrojevi_Command = new RelayCommand(PrikaziTastaturuBrojevi_Metoda);
            PrikaziTastaturuSlova_Command = new RelayCommand(PrikaziTastaturuSlova_Metoda);
            TasterKlik_Command = new RelayCommand(TasterKlik_Metoda);
            TastaturaObrisi_Command = new RelayCommand(TastaturaObrisi_Metoda);
            UgasiTastaturu_Command = new RelayCommand(UgasiTastaturu_Metoda);
            PrijavaEnter_Command = new RelayCommand(PrijavaEnter_Metoda);

            prikazPinTastatura = Visibility.Hidden;
            prikazTastatura = Visibility.Visible;

            tasteriMalaSlova.Visibility = Visibility.Visible;
            tasteriVelikaSlova.Visibility = Visibility.Hidden;
            tasteriBrojevi.Visibility = Visibility.Hidden;

            brdPadajuciMeni.Height = listaObjekata.Count * 45;
            StackPanel stcListaObj = new StackPanel();
            stcListaObj.Orientation = Orientation.Vertical;
            stcListaObj.Background = Brushes.Transparent;
            //stcListaObj.Margin = new System.Windows.Thickness(20, 70, 50, 50);
            stcListaObj.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            stcListaObj.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

            brdPadajuciMeni.Child = stcListaObj;
            foreach (re_Objekat item in listaObjekata)
            {
                TextBlock objekat = new TextBlock();
                objekat.Background = Brushes.Transparent;
                objekat.Foreground = Brushes.White;
                objekat.Margin = new System.Windows.Thickness(20, 5, 0, 0);
                objekat.FontSize = 15;
                objekat.Height = 40;
                objekat.Text = item.Naziv;
                objekat.TextWrapping = TextWrapping.Wrap;
                objekat.FontWeight = FontWeights.SemiBold;
                objekat.Tag = item;

                objekat.MouseLeftButtonDown += IzabranObjekat_MouseLeftButtonDown;

                stcListaObj.Children.Add(objekat);
            }
        }

        private void PrijavaEnter_Metoda(object obj)
        {
            Prijava_Metoda(obj);
        }

        private void UgasiTastaturu_Metoda(object obj)
        {
            pb.Password = "";
            pbPin.Password = "";
            prikazPinTastatura = Visibility.Hidden;
            prikazTastatura = Visibility.Hidden;
        }
        private void PrikaziUserTastatura_Metoda(object obj)
        {
            pb.Password = "";
            pbPin.Password = "";
            if (prikazTastatura == Visibility.Visible)
            {
                prikazTastatura = Visibility.Hidden;
            }
            else
            {
                prikazPinTastatura = Visibility.Hidden;
                prikazTastatura = Visibility.Visible;
            }
        }
        private void PrikaziPinTastatura_Metoda(object obj)
        {
            pb.Password = "";
            pbPin.Password = "";
            if (prikazPinTastatura == Visibility.Visible)
            {
                prikazPinTastatura = Visibility.Hidden;
            }
            else
            {
                prikazPinTastatura = Visibility.Visible;
                prikazTastatura = Visibility.Hidden;
            }
        }
        //u okviru tastature - funkcije
        private void TastaturaObrisi_Metoda(object obj)
        {
            if (prikazTastatura == Visibility.Visible)
            {
                if (pb.Password == null)
                {
                    return;
                }
                if (pb.Password.Length != 0)
                {
                    pb.Password = pb.Password.Substring(0, pb.Password.Length - 1);
                }
            }
            else
            {
                if (pbPin.Password == null)
                {
                    return;
                }
                if (pbPin.Password.Length != 0)
                {
                    pbPin.Password = pbPin.Password.Substring(0, pbPin.Password.Length - 1);
                }
            }


        }
        private void TasterKlik_Metoda(object obj)
        {
            if (prikazPinTastatura == Visibility.Visible)
            {
                pbPin.Password += obj.ToString();
            }
            else
            {
                pb.Password += obj.ToString();
            }
        }
        private void PrikaziTastaturuSlova_Metoda(object obj)
        {
            if (tasteriVelikaSlova.Visibility == Visibility.Visible)
            {
                tasteriVelikaSlova.Visibility = Visibility.Hidden;
                tasteriMalaSlova.Visibility = Visibility.Visible;
                tasteriBrojevi.Visibility = Visibility.Hidden;
            }
            else if (tasteriMalaSlova.Visibility == Visibility.Visible)
            {
                tasteriVelikaSlova.Visibility = Visibility.Visible;
                tasteriMalaSlova.Visibility = Visibility.Hidden;
                tasteriBrojevi.Visibility = Visibility.Hidden;
            }
        }
        private void PrikaziTastaturuBrojevi_Metoda(object obj)
        {
            if (tasteriVelikaSlova.Visibility == Visibility.Visible || tasteriMalaSlova.Visibility == Visibility.Visible)
            {
                tasteriVelikaSlova.Visibility = Visibility.Hidden;
                tasteriMalaSlova.Visibility = Visibility.Hidden;
                tasteriBrojevi.Visibility = Visibility.Visible;
            }
            else
            {
                tasteriVelikaSlova.Visibility = Visibility.Hidden;
                tasteriMalaSlova.Visibility = Visibility.Visible;
                tasteriBrojevi.Visibility = Visibility.Hidden;
            }
        }
        private void PadajuciMeni_Metoda(object obj)
        {
            if (brdPadajuciMeni.Height == 0)
            {
                DoubleAnimation heightAnimation = new DoubleAnimation(listaObjekata.Count * 45, new Duration(TimeSpan.FromSeconds(0.5)));
                brdPadajuciMeni.BeginAnimation(TextBlock.HeightProperty, heightAnimation);
            }
            else
            {
                DoubleAnimation heightAnimation = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(1)));
                brdPadajuciMeni.BeginAnimation(TextBlock.HeightProperty, heightAnimation);
            }
            StackPanel stcListaObj = new StackPanel();
            stcListaObj.Orientation = Orientation.Vertical;
            stcListaObj.Background = Brushes.Transparent;
            //stcListaObj.Margin = new System.Windows.Thickness(20, 70, 50, 50);
            stcListaObj.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            stcListaObj.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

            brdPadajuciMeni.Child = stcListaObj;

            foreach (re_Objekat item in listaObjekata)
            {
                TextBlock objekat = new TextBlock();
                objekat.Background = Brushes.Transparent;
                objekat.Foreground = Brushes.White;
                objekat.Margin = new System.Windows.Thickness(20, 5, 0, 0);
                objekat.FontSize = 15;
                objekat.Height = 40;
                objekat.Text = item.Naziv;
                objekat.TextWrapping = TextWrapping.Wrap;
                objekat.FontWeight = FontWeights.SemiBold;
                objekat.Tag = item;

                objekat.MouseLeftButtonDown += IzabranObjekat_MouseLeftButtonDown;

                stcListaObj.Children.Add(objekat);
            }
        }
        private void IzabranObjekat_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TextBlock txt = sender as TextBlock;

            if (izabraniObj == null)
            {
                tbIzabraniObj = txt;
                tbIzabraniObj.Foreground = (Brush)Application.Current.FindResource("Plava_SV");
                izabraniObj = (re_Objekat)txt.Tag;
            }
            else
            {
                tbIzabraniObj.Foreground = (Brush)Application.Current.FindResource("Bela");
                tbIzabraniObj = txt;
                tbIzabraniObj.Foreground = (Brush)Application.Current.FindResource("Plava_SV");
                izabraniObj = (re_Objekat)txt.Tag;
            }
        }
        private void Prijava_Metoda(object obj)
        {
            //odabir objekta
            if (izabraniObj == null)
            {
                v_WarningBox prozor = new v_WarningBox("Niste odabrali objekat.");
                prozor.ShowDialog();
                return;
            }
            Globalno.Varijable.Objekat = izabraniObj;

            //odabir korisnika(operatera)
            Globalno.Varijable.Korisnik = null;
            sifra = null;
            if (pb.Password != "")
            {
                sifra = pb.Password;
            }
            else if (pbPin.Password != "")
            {
                sifra = pbPin.Password;
            }

            if (String.IsNullOrEmpty(sifra))
            {
                v_WarningBox prozor = new v_WarningBox("Šifra nije uneta.");
                prozor.ShowDialog();
                return;
            }
            else
            {
                try
                {
                    //proverava dal postoji korisnik sa tom sifrom
                    ObservableCollection<re_Korisnik> korisnici = BrokerSelect.dajSesiju().vratiOperateraPoSifri(sifra);
                    re_Korisnik korisnik = korisnici.FirstOrDefault();
                    if (korisnik == null)
                    {
                        v_WarningBox pr = new v_WarningBox("Neuspešno prijavljivanje!\nPogrešna šifra.");
                        pr.ShowDialog();
                        return;
                    }
                    Globalno.Varijable.Korisnik = korisnik;

                    //sifra je adm03
                    //Sistem.GlobalneVarijable.logovanikorisnik = "BrankoR";

                    //ovde palim svoj glavni ekran

                    BrokerInsert.dajSesiju().ZapisiLog(9010, $"Uspešo ulogovan korisnik {korisnik.LogOnIme}.", "Login");

                    v_RadniProstor prozor = new v_RadniProstor();
                    prozor.Show();
                    this.ZatvoriFormu();

                }
                catch (Exception)
                {
                    v_WarningBox pr = new v_WarningBox("Neuspešno prijavljivanje!");
                    pr.ShowDialog();
                }
            }
        }
        private void Zatvori_Metoda(object obj)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
