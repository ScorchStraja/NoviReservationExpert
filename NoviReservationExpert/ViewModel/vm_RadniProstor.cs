using NoviReservationExpert.Broker;
using NoviReservationExpert.Model;
using NoviReservationExpert.View;
using NoviReservationExpert.View.UserKontrole;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using static NoviReservationExpert.ViewModel.vm_RadniProstor;

namespace NoviReservationExpert.ViewModel
{
    public class vm_RadniProstor : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #region KOMANDE
        public Action ZatvoriFormu { get; set; }
        public RelayCommand Zatvori_Command { get; private set; }
        public RelayCommand OtvoriRadniMeni_Command { get; private set; }
        public RelayCommand DodajNovuRezervaciju_Command { get; private set; }
        public RelayCommand PromenaEkrana_Command { get; private set; }
        public RelayCommand PregledRezervacija_Command { get; private set; }
        public RelayCommand ZatvoriMeni_Command { get; private set; }
        public RelayCommand PromenaDatuma_Command { get; private set; }
        #endregion

        #region PROPERTIJI
        DateTime _IzabranDatum;
        public DateTime IzabranDatum
        {
            get
            {
                return _IzabranDatum;
            }
            set
            {
                _IzabranDatum = value;
                //Globalno.Varijable.IzabranDatum = IzabranDatum;
                //PopuniPrikazRezervacijama(_IzabranDatum);
                NotifyPropertyChanged(nameof(IzabranDatum));
            }
        }
        string _IzabranoVreme;
        public string IzabranoVreme
        {
            get
            {
                return _IzabranoVreme;
            }
            set
            {
                _IzabranoVreme = value;
                NotifyPropertyChanged(nameof(IzabranoVreme));
            }
        }
        string _BrojGostiju;
        public string BrojGostiju
        {
            get
            {
                return _BrojGostiju;
            }
            set
            {
                _BrojGostiju = value;
                NotifyPropertyChanged(nameof(BrojGostiju));
            }
        }
        string _Gost = "";
        public string Gost
        {
            get
            {
                return _Gost;
            }
            set
            {
                _Gost = value;
                NotifyPropertyChanged(nameof(Gost));
            }
        }
        int _SirinaCanvasa;
        public int SirinaCanvasa
        {
            get
            {
                return _SirinaCanvasa;
            }
            set 
            {
                _SirinaCanvasa = value;
                NotifyPropertyChanged(nameof(SirinaCanvasa));
            }
        }
        re_Sema _izabranaSema;
        public re_Sema IzabranaSema
        {
            get
            {
                return _izabranaSema;
            }
            set
            {
                _izabranaSema = value;
                Globalno.Varijable.IzabranaSema = _izabranaSema;
                NotifyPropertyChanged(nameof(IzabranaSema));
            }
        }
        Visibility _v_PrikazSematski;
        public Visibility v_PrikazSematski
        {
            get
            {
                return _v_PrikazSematski;
            }
            set
            {
                _v_PrikazSematski = value;
                NotifyPropertyChanged(nameof(v_PrikazSematski));
            }
        }
        Visibility _v_PrikazTabelarni;
        public Visibility v_PrikazTabelarni
        {
            get
            {
                return _v_PrikazTabelarni;
            }
            set
            {
                _v_PrikazTabelarni = value;
                NotifyPropertyChanged(nameof(v_PrikazTabelarni));
            }
        }
        public class MojBorderSema : Border
        {
            public re_Sema Sema;
            public MojBorderSema(re_Sema sema)
            {
                this.Sema = sema;      
                this.Margin = new System.Windows.Thickness(5, 5, 5, 5);
                this.Width = 150;
                this.CornerRadius = new System.Windows.CornerRadius(3);
                this.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#284b63");
                StackPanel sp = new StackPanel();
                this.Child = sp;
                sp.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                sp.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                TextBlock tb = new TextBlock();
                if(sema.ToString().Count() > 9)
                {
                    tb.Text = sema.ToString().Substring(0, 11);
                }
                else
                {
                    tb.Text = sema.ToString();
                }
                tb.Margin = new System.Windows.Thickness(0, 0, 0, 5);
                tb.Foreground = Brushes.White;
                tb.FontSize = 16;
                tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                sp.Children.Add(tb);
                Border slika = new Border();
                Image Img = new Image();
                Img.Source = new BitmapImage(new Uri("pack://application:,,,/NoviReservationExpert;component/Resursi/Slike/DefaultSema.png"));
                slika.Child = Img;
                slika.Height = 100;
                slika.Width = 130;
                slika.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                slika.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#444444");
                slika.CornerRadius = new System.Windows.CornerRadius(3);
                slika.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                sp.Children.Add(slika);
            }
        }
        #endregion
        public class PrikazSto : Border
        {
            public Image znakuzvika = new Image();
            public re_Sto Sto = new re_Sto();
            public bool zauzetost = false;
            public PrikazSto(int visinaStola, int sirinaStola, re_Sto sto)
            {
                this.Height = visinaStola;
                this.Background = Brushes.WhiteSmoke;
                this.Width = sirinaStola;
                this.Margin = new System.Windows.Thickness(0, 0, 0, 0);
                this.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#444444");
                this.Sto = sto;

                zauzetost = Broker.BrokerSelect.dajSesiju().ZauzetostStola(sto);

                Grid grd = new Grid();
                this.Child = grd;

                Image znakuzvika = new Image();
                this.znakuzvika = znakuzvika;
                BitmapImage bi2 = new BitmapImage();
                znakuzvika.Source = bi2;
                bi2.BeginInit();
                bi2.UriSource = new Uri("/Resursi/Slike/ZnakUzvika.PNG", UriKind.RelativeOrAbsolute);
                bi2.EndInit();               

                RowDefinition row1 = new RowDefinition();
                RowDefinition row2 = new RowDefinition();
                ColumnDefinition column1 = new ColumnDefinition();
                ColumnDefinition column2 = new ColumnDefinition();
                row1.Height = new GridLength(1.6, GridUnitType.Star);
                column2.Width = new GridLength(1.6, GridUnitType.Star);
                grd.RowDefinitions.Add(row1);
                grd.RowDefinitions.Add(row2);
                grd.ColumnDefinitions.Add(column1);
                grd.ColumnDefinitions.Add(column2);

                grd.Children.Add(znakuzvika);
                Grid.SetColumn(znakuzvika, 1);
                Grid.SetRow(znakuzvika, 0);
                znakuzvika.HorizontalAlignment = HorizontalAlignment.Right;
                znakuzvika.VerticalAlignment = VerticalAlignment.Center;
                znakuzvika.Height = 35;
                znakuzvika.Width = 35;
                znakuzvika.Margin = new Thickness(0, 0, -15, -25);
                if (zauzetost)
                {
                    this.znakuzvika.Visibility = Visibility.Visible;
                }
                else
                {
                    this.znakuzvika.Visibility = Visibility.Hidden;
                }

                Border brdslika = new Border();
                grd.Children.Add(brdslika);
                Grid.SetColumn(brdslika, 0);
                Grid.SetRow(brdslika, 0);
                brdslika.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#444444");
                brdslika.BorderThickness = new Thickness(0, 0, 0.5, 0);
                Image img = new Image();
                BitmapImage bi = new BitmapImage();
                img.Source = bi;
                bi.BeginInit();
                bi.UriSource = new Uri("/Resursi/Slike/Covek_Sivo.png", UriKind.RelativeOrAbsolute);
                bi.EndInit();
                brdslika.Child = img;
                img.Height = 30;

                TextBlock brojljudi = new TextBlock();
                brojljudi.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#444444");
                brojljudi.Text = sto.BrojOsoba.ToString();
                grd.Children.Add(brojljudi);
                Grid.SetColumn(brojljudi, 1);
                Grid.SetRow(brojljudi, 0);
                brojljudi.FontSize = 25;
                brojljudi.HorizontalAlignment = HorizontalAlignment.Center;
                brojljudi.VerticalAlignment = VerticalAlignment.Center;

                Border brdime = new Border();                
                grd.Children.Add(brdime);
                Grid.SetColumn(brdime, 0);
                Grid.SetColumnSpan(brdime, 2);
                Grid.SetRow(brdime, 1);
                brdime.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#444444");
                brdime.BorderThickness = new Thickness(0, 0.5, 0, 0);
                StackPanel sp = new StackPanel();
                brdime.Child = sp;             
                sp.Orientation = Orientation.Horizontal;
                sp.HorizontalAlignment = HorizontalAlignment.Center;
                TextBlock imestola = new TextBlock();
                if (sto.Sto.Length > 10)
                {
                    imestola.Text = sto.Sto.Substring(0, 11);
                }
                else
                {
                    imestola.Text = sto.ToString();
                }
                imestola.FontWeight = FontWeights.Bold;
                imestola.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#444444");
                imestola.FontSize = 15;
                imestola.HorizontalAlignment = HorizontalAlignment.Center;
                imestola.VerticalAlignment = VerticalAlignment.Center;

                Image img1 = new Image();
                img1.Height = 20;
                BitmapImage bi1 = new BitmapImage();
                img1.Source = bi1;
                bi1.BeginInit();
                bi1.UriSource = new Uri("/Resursi/Slike/Sto_Sivo.PNG", UriKind.RelativeOrAbsolute);
                bi1.EndInit();
                brdslika.Child = img;
                img1.Margin = new Thickness(0, 0, 4, 0);
                sp.Children.Add(img1);
                sp.Children.Add(imestola);
            }
            public void TrenutniZauzetSto(bool zauzetost) // 0 nije, 1 jeste
            {
                if (zauzetost)
                {
                    znakuzvika.Visibility = Visibility.Visible;
                }
                else
                {
                    znakuzvika.Visibility = Visibility.Hidden;
                }
            }
        }

        Popup BiranjeIkoniceStola;
        ObservableCollection<re_Sema> Seme = new ObservableCollection<re_Sema>();
        ObservableCollection<re_Sto> Stolovi = new ObservableCollection<re_Sto>();
        ObservableCollection<re_Rezervacija> Rezervacije = new ObservableCollection<re_Rezervacija>();
        List<uc_Notifikacija> Notifikacije = new List<uc_Notifikacija>();
        List<uc_Rezervacija> BrdRezervacije = new List<uc_Rezervacija>(); //rezervacije koje se prikazuju, za svaku rezervaciju u Rezervacije, ovde postoji border
        DockPanel dpSemeIDugmad;
        DockPanel dpVremena;
        DockPanel dpStolovi;
        StackPanel spNotifikacije;
        ScrollViewer svCanvas;
        Canvas canvasPrikaz;
        Canvas canvasSematskiPrikaz;
        public int sirinaVremena = 60;
        public int visinaVremena = 60;
        public int visinaStola = 60;
        public int sirinaStola = 100;
        public int brojodeljka = 0;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public vm_RadniProstor(DockPanel dpSemeIDugmad, DockPanel dpStolovi, DockPanel dpVremena, Canvas prikazTabelarni, Canvas prikazSematski, ScrollViewer svCanvas, StackPanel spNotifikacije)
        {
            Zatvori_Command = new RelayCommand(Zatvori_Metoda);
            OtvoriRadniMeni_Command = new RelayCommand(OtvoriRadniMeni_Metoda);
            DodajNovuRezervaciju_Command = new RelayCommand(DodajNovuRezervaciju_Metoda);
            PromenaEkrana_Command = new RelayCommand(PromenaEkrana_Metoda);
            PregledRezervacija_Command = new RelayCommand(PregledRezervacija_Metoda);
            ZatvoriMeni_Command = new RelayCommand(ZatvoriMeni_Metoda);
            PromenaDatuma_Command = new RelayCommand(PromenaDatuma_Metoda);
            Seme = Broker.BrokerSelect.dajSesiju().vratiSeme(); // automatski vraca seme za izabran objekat sa login-a
           
            canvasPrikaz = prikazTabelarni;
            canvasSematskiPrikaz = prikazSematski;
            canvasPrikaz.MouseMove += Pomeranje;
            this.dpStolovi = dpStolovi;
            this.dpVremena = dpVremena;
            this.svCanvas = svCanvas;
            this.spNotifikacije = spNotifikacije;
            this.dpSemeIDugmad = dpSemeIDugmad;
       

            v_PrikazSematski = Visibility.Hidden;
            v_PrikazTabelarni = Visibility.Visible;

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            dispatcherTimer.Start();

            this.IzabranDatum = DateTime.Today;

            foreach (re_Sema sema in Seme)
            {
                if (sema.Odabran == 1)
                {
                    IzabranaSema = sema;
                }
            }
            Stolovi = BrokerSelect.dajSesiju().vratiStolove(IzabranaSema.Sema);

            PopuniVremena();
            PopuniStolove();
            PopuniPrikazSema();

            foreach (MojBorderSema brd1 in dpSemeIDugmad.Children.OfType<MojBorderSema>())
            {
                if (brd1.Sema.Odabran == 1)
                {
                    brd1.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#427aa1");
                }
            }

            UpdateNotifikacije();

            OsveziRadniProstor();
        }     
        private void PromenaDatuma_Metoda(object obj)
        {
            Globalno.Varijable.IzabranDatum = IzabranDatum;
            PopuniPrikazRezervacijama(_IzabranDatum);
        }
        private void ZatvoriMeni_Metoda(object obj)
        {
            if (IsWindowOpen<v_Meni>())
            {
                meni.Close();
            }
        }
        private void PregledRezervacija_Metoda(object obj)
        {
            v_PregledRezervacija prozor = new v_PregledRezervacija();
            prozor.ShowDialog();
        }
        private void PromenaEkrana_Metoda(object obj)
        {
            this.v_PrikazSematski = v_PrikazSematski == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            this.v_PrikazTabelarni = v_PrikazTabelarni == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            PopuniSematskiPrikaz();
        }
        private void DodajNovuRezervaciju_Metoda(object obj)
        {
            v_SlobodniStolovi prozor = new v_SlobodniStolovi();
            prozor.ShowDialog();
        }
        private void dispatcherTimer_Tick(object? sender, EventArgs e)
        {
            UpdateNotifikacije();
        }
        private void PopuniPrikazRezervacijama(DateTime datum)
        {
            for(int i = canvasPrikaz.Children.Count - 1; i >= 0; i--)
            {   
                if (canvasPrikaz.Children[i].GetType() == typeof(uc_Rezervacija))
                {
                    canvasPrikaz.Children.RemoveAt(i);
                }
            }
            BrdRezervacije.Clear();
            Rezervacije = BrokerSelect.dajSesiju().VratiRezervacijeZaDatum(IzabranDatum, Globalno.Varijable.Objekat.Objekat, IzabranaSema.Sema);
            foreach (re_Rezervacija rezervacija in Rezervacije)
            {                
                int vremeUMin = (rezervacija.VremeOd.Hour - 7)*sirinaVremena + rezervacija.VremeOd.Minute;
                int ypozicija = sirinaStola + vremeUMin * 2;

                string[] stolovi = rezervacija.Sto.Split(",");
                foreach(string sto in stolovi)
                {
                    uc_Rezervacija brdRez = new uc_Rezervacija(rezervacija, sto);
                    int xpozicija = Stolovi.IndexOf(Stolovi.FirstOrDefault(x => x.Sto == sto));
                    Panel.SetZIndex(brdRez, xpozicija * 100);
                    if (brdRez.OsveziRadniProstor == null)
                        brdRez.OsveziRadniProstor = new Action(OsveziRadniProstor);
                    brdRez.MouseLeftButtonDown += PocetakPomeranja;
                    brdRez.MouseLeftButtonUp += ZavrsetakPomeranja;

                    canvasPrikaz.Children.Add(brdRez);
                    BrdRezervacije.Add(brdRez);

                    Canvas.SetLeft(brdRez, ypozicija);
                    Canvas.SetTop(brdRez, visinaVremena + xpozicija * visinaStola);

                    brdRez.ProveraKapacitetaStola(Stolovi.FirstOrDefault(x => x.Sto == sto));
                }
            }
            ProveraPreklapanjaStolova();
        }
        private void ProveraPreklapanjaStolova()
        {
            foreach (uc_Rezervacija rezervacija in BrdRezervacije)
            {
                ObservableCollection<re_Rezervacija> poklapanja = Broker.BrokerSelect.dajSesiju().PostojiPoklapanje(rezervacija.Rezervacija.VremeOd, rezervacija.Rezervacija.VremeDo, rezervacija.Rezervacija.Id);
                foreach(re_Rezervacija rez in poklapanja)
                {
                    string[] rezstolovi = rez.Sto.Split(",");
                    foreach (string strsto in rezstolovi)
                    {
                        if (strsto.Contains(rezervacija.ucrez_sto) && strsto.Length == rezervacija.ucrez_sto.Length)
                        {
                            uc_Rezervacija uc_rez = BrdRezervacije.FirstOrDefault(x => x.Rezervacija.Id == rez.Id);
                            if (!uc_rez.zamracen)
                            {
                                rezervacija.ZamraciRezervaciju();
                            }
                            if (rez.Id > rezervacija.Rezervacija.Id)
                            {
                                if (rez.VremeOd == rezervacija.Rezervacija.VremeOd)
                                {
                                    rezervacija.PomeriObelezivac();
                                }
                            }
                        }
                    }              
                }
            }
        }
        #region PomeranjeRezervacija
        bool pocetakPomeranja = false;
        bool pomeranje = false;
        double razlikaX = 0;
        double razlikaY = 0;
        uc_Rezervacija rezervacijaKojaSePomera;
        private void PocetakPomeranja(object sender, MouseButtonEventArgs e)
        {
            pocetakPomeranja = true;
            rezervacijaKojaSePomera = sender as uc_Rezervacija;

            razlikaX = Mouse.GetPosition(canvasPrikaz).X - Canvas.GetLeft(rezervacijaKojaSePomera);
            razlikaY = Mouse.GetPosition(canvasPrikaz).Y - Canvas.GetTop(rezervacijaKojaSePomera);
        }
        private void Pomeranje(object sender, MouseEventArgs e)
        {
            if (pocetakPomeranja == true)
            {
                if (rezervacijaKojaSePomera != null)
                {
                    pomeranje = true;
                    Canvas.SetTop(rezervacijaKojaSePomera, Mouse.GetPosition(canvasPrikaz).Y - razlikaY); //-30
                    Canvas.SetLeft(rezervacijaKojaSePomera, Mouse.GetPosition(canvasPrikaz).X - razlikaX); // -100

                    foreach(uc_Rezervacija ucrez in BrdRezervacije)
                    {
                        if(ucrez.Rezervacija.Id == rezervacijaKojaSePomera.Rezervacija.Id)
                        {
                            ucrez.RezervacijaSePomera();
                            Panel.SetZIndex(ucrez, 9999999);
                        }
                    }
                }
            }
        }
        private void ZavrsetakPomeranja(object sender, MouseButtonEventArgs e) 
        {
            uc_Rezervacija rezervacija = sender as uc_Rezervacija;
            if (pomeranje == false)
            {
                if(!rezervacijaKojaSePomera.zamracen) //svakom sam REDU (stolu) dodelio 100 praznih mesta, 101 je izabrana
                {
                    if (IsWindowOpen<v_Meni>())
                    {
                        meni.Close();
                    }
                    meni = new v_Meni(rezervacija.Rezervacija.VremeOd.ToString("HH:mm"), rezervacija.Rezervacija.Sto, sender);
                    if (meni.ActionNovaRezervacija == null)
                    {
                        meni.ActionNovaRezervacija = new Action(NovaRezervacija);
                        meni.ActionAktivirajRezervaciju = new Action(AktivirajRezervaciju);
                        meni.ActionOtvoriDetaljeRezervacije = new Action(OtvoriDetaljeRezervacije);
                        meni.ActionDeaktivirajRezervaciju = new Action(DeaktivirajRezervaciju);
                    }
                    meni.Left = Mouse.GetPosition(canvasPrikaz).X - 170 - svCanvas.HorizontalOffset;
                    meni.Top = Mouse.GetPosition(canvasPrikaz).Y - svCanvas.VerticalOffset;
                    meni.Show();
                    
                    pocetakPomeranja = false;
                    return;
                } else
                {
                    rezervacijaKojaSePomera.OtkrijRezervaciju();
                    Panel.SetZIndex(rezervacijaKojaSePomera, Stolovi.IndexOf(Stolovi.FirstOrDefault(x => x.Sto == rezervacijaKojaSePomera.Rezervacija.Sto)) * 100 + 1);
                    ObservableCollection<re_Rezervacija> svapoklapanja = BrokerSelect.dajSesiju().VratiRezervacijeKojeSePoklapajuSaVremenom(rezervacija.Rezervacija.VremeOd, rezervacija.Rezervacija.VremeDo, rezervacija.Rezervacija.VremeOd.Date, rezervacija.Rezervacija.Id);
                    ObservableCollection<re_Rezervacija> poklapanjazasto = new ObservableCollection<re_Rezervacija>();

                    foreach (re_Rezervacija rez in svapoklapanja)
                    {
                        string[] rezstolovi = rez.Sto.Split(",");
                        foreach (string strsto in rezstolovi)
                        {
                            if (strsto.Contains(rezervacijaKojaSePomera.ucrez_sto) && strsto.Length == rezervacijaKojaSePomera.ucrez_sto.Length)
                            {
                                poklapanjazasto.Add(rez);
                            }
                        }
                    }
                    foreach (re_Rezervacija rezervacijapoklapanja in poklapanjazasto)
                    {
                        uc_Rezervacija rez = BrdRezervacije.FirstOrDefault(x => x.Rezervacija.Id == rezervacijapoklapanja.Id);
                        rez.ZamraciRezervaciju();
                        Panel.SetZIndex(rez, Stolovi.IndexOf(Stolovi.FirstOrDefault(x => x.Sto == rezervacijaKojaSePomera.Rezervacija.Sto)) * 100);
                    }
                    pocetakPomeranja = false;
                    return;
                }
            }
            if (pocetakPomeranja)
            {
                pocetakPomeranja = false;
                pomeranje = false;
                int vremeUMin = (rezervacijaKojaSePomera.Rezervacija.VremeOd.Hour - 7) * sirinaVremena + rezervacijaKojaSePomera.Rezervacija.VremeOd.Minute;
                double pocetnaY = sirinaStola + vremeUMin * 2;
                double pocetnaX = Stolovi.IndexOf(Stolovi.First(x => x.Sto == rezervacijaKojaSePomera.ucrez_sto));

                //double koordinataX = Mouse.GetPosition(canvasPrikaz).X - sirinaStola - sirinaVremena;
                //double koordinataY = Mouse.GetPosition(canvasPrikaz).Y - visinaVremena;

                double koordinataX = Canvas.GetLeft(rezervacijaKojaSePomera) - sirinaStola - sirinaVremena;
                double koordinataY = Canvas.GetTop(rezervacijaKojaSePomera) - visinaVremena;

                int xbrojCelina = Convert.ToInt32(Math.Floor(koordinataX / sirinaVremena + 1)); //-1
                if (xbrojCelina < 0) xbrojCelina = 0;
                
                int ybrojCelina = Convert.ToInt32(Math.Floor(koordinataY / visinaStola )); //-1
                if (ybrojCelina < 0) ybrojCelina = 0;

                TimeSpan trajanje = rezervacijaKojaSePomera.Rezervacija.VremeDo - rezervacijaKojaSePomera.Rezervacija.VremeOd;

                DateTime osnova = rezervacijaKojaSePomera.Rezervacija.VremeOd.Date;

                DateTime pocetak = osnova.AddHours(7).AddMinutes(xbrojCelina * 30);
                DateTime kraj = pocetak.AddMinutes(trajanje.TotalMinutes);
                re_Sto sto = Stolovi[ybrojCelina];

                if (sto.BrojOsoba < rezervacijaKojaSePomera.Rezervacija.BrojOdraslih)
                {
                    v_MessageBox prozor = new v_MessageBox($"Sto {sto.Sto} ne podržava broj osoba rezervacije. Želite li da nastavite?");
                    prozor.ShowDialog();
                    if (!Globalno.Varijable.sacuvanePromene)
                    {
                        Canvas.SetLeft(rezervacijaKojaSePomera, pocetnaY);
                        Canvas.SetTop(rezervacijaKojaSePomera, visinaVremena + pocetnaX * visinaStola);
                        foreach (uc_Rezervacija ucrez in BrdRezervacije)
                        {
                            if (ucrez.Rezervacija.Id == rezervacijaKojaSePomera.Rezervacija.Id)
                            {
                                ucrez.RezervacijaJePrestalaDaSePomera();
                                Panel.SetZIndex(ucrez, Stolovi.IndexOf(Stolovi.FirstOrDefault(x => x.Sto == ucrez.Rezervacija.Sto)) * 100);
                            }
                        }
                        return;
                    }
                }
                rezervacijaKojaSePomera.ProveraKapacitetaStola(sto);

                string[] stolovi = rezervacijaKojaSePomera.Rezervacija.Sto.Split(",");
                foreach (string strsto in stolovi)
                {
                    if (strsto.Contains(sto.Sto) && strsto.Length == sto.Sto.Length)
                    {
                        if (rezervacijaKojaSePomera.ucrez_sto != sto.Sto)
                        {
                            v_WarningBox prozor = new v_WarningBox($"Ova rezervacija već zauzima ovaj sto.");
                            prozor.ShowDialog();
                            foreach (uc_Rezervacija ucrez in BrdRezervacije)
                            {
                                if (ucrez.Rezervacija.Id == rezervacijaKojaSePomera.Rezervacija.Id)
                                {
                                    ucrez.RezervacijaJePrestalaDaSePomera();
                                    Panel.SetZIndex(ucrez, Stolovi.IndexOf(Stolovi.FirstOrDefault(x => x.Sto == ucrez.Rezervacija.Sto)) * 100);
                                }
                            }
                            Canvas.SetLeft(rezervacijaKojaSePomera, pocetnaY);
                            Canvas.SetTop(rezervacijaKojaSePomera, visinaVremena + pocetnaX * visinaStola);
                            return;
                        }
                    }
                }


                //ObservableCollection<re_Rezervacija> poklapanja = Broker.BrokerSelect.dajSesiju().VratiRezervacijeKojeSePoklapajuSaVremenom_ZaSto(pocetak,kraj, Stolovi[ybrojCelina].Sto, rezervacijaKojaSePomera.Rezervacija.Id);
                ObservableCollection<re_Rezervacija> svapoklapanja = Broker.BrokerSelect.dajSesiju().VratiRezervacijeKojeSePoklapajuSaVremenom(pocetak,kraj,pocetak.Date,rezervacijaKojaSePomera.Rezervacija.Id);
                ObservableCollection<re_Rezervacija> poklapanjazasto = new ObservableCollection<re_Rezervacija>();

                foreach(re_Rezervacija rez in svapoklapanja)
                {
                    string[] rezstolovi = rez.Sto.Split(",");
                    foreach (string strsto in rezstolovi)
                    {
                        if (strsto.Contains(sto.Sto) && strsto.Length == sto.Sto.Length)
                        {
                            poklapanjazasto.Add(rez);
                        }
                    }
                }

                if (poklapanjazasto.Any())
                {
                    v_MessageBox poruka = new v_MessageBox("U datom terminu već postoji rezervacija. Da li želite da nastavite?");
                    poruka.ShowDialog();
                    if (!Globalno.Varijable.sacuvanePromene)
                    {
                        Canvas.SetLeft(rezervacijaKojaSePomera, pocetnaY);
                        Canvas.SetTop(rezervacijaKojaSePomera, visinaVremena + pocetnaX * visinaStola);
                        foreach (uc_Rezervacija ucrez in BrdRezervacije)
                        {
                            if (ucrez.Rezervacija.Id == rezervacijaKojaSePomera.Rezervacija.Id)
                            {
                                ucrez.RezervacijaJePrestalaDaSePomera();
                                Panel.SetZIndex(ucrez, Stolovi.IndexOf(Stolovi.FirstOrDefault(x => x.Sto == ucrez.Rezervacija.Sto)) * 100);
                            }
                        }
                        return;
                    }
                }

                Canvas.SetLeft(rezervacijaKojaSePomera, xbrojCelina * sirinaVremena + sirinaStola);
                Canvas.SetTop(rezervacijaKojaSePomera, ybrojCelina * visinaStola + visinaVremena);

                //rezervacijaKojaSePomera.Rezervacija.Sto = Stolovi[ybrojCelina].Sto;

                rezervacijaKojaSePomera.Rezervacija.Sto = "";
                foreach (string strsto in stolovi)
                {
                    if (strsto.Contains(rezervacijaKojaSePomera.ucrez_sto) && strsto.Length == rezervacijaKojaSePomera.ucrez_sto.Length)
                    {
                        rezervacijaKojaSePomera.Rezervacija.Sto += sto.Sto;
                        rezervacijaKojaSePomera.Rezervacija.Sto += ",";
                        continue;
                    }
                    rezervacijaKojaSePomera.Rezervacija.Sto += strsto;
                    rezervacijaKojaSePomera.Rezervacija.Sto += ",";
                }
                rezervacijaKojaSePomera.Rezervacija.Sto = rezervacijaKojaSePomera.Rezervacija.Sto.Remove(rezervacijaKojaSePomera.Rezervacija.Sto.Length - 1);

                rezervacijaKojaSePomera.Rezervacija.VremeOd = pocetak;
                rezervacijaKojaSePomera.Rezervacija.VremeDo = kraj;
                rezervacijaKojaSePomera.PromeniVreme(pocetak, kraj);
                BrokerUpdate.dajSesiju().UpdateRezervaciju(rezervacijaKojaSePomera.Rezervacija);
                this.OsveziRadniProstor();
            }
        }
        private void DeaktivirajRezervaciju()
        {
            meni.Close();
            Broker.BrokerUpdate.dajSesiju().UpdateRezervaciju(rezervacijaKojaSePomera.Rezervacija.Id, -1);
        }
        private void OtvoriDetaljeRezervacije()
        {
            meni.Close();
            v_DetaljiRezervacije prozor = new v_DetaljiRezervacije(rezervacijaKojaSePomera.Rezervacija);
            prozor.ShowDialog();
        }
        private void AktivirajRezervaciju()
        {
            meni.Close();
            Broker.BrokerUpdate.dajSesiju().UpdateRezervaciju(rezervacijaKojaSePomera.Rezervacija.Id, 1);
        }
        #endregion
        public void OsveziRadniProstor()
        {
            PopuniPrikazRezervacijama(IzabranDatum);
        }
        private void PopuniPrikazSema()
        {
            foreach(re_Sema sema in Seme)
            {
                MojBorderSema brd = new MojBorderSema(sema);
                brd.MouseLeftButtonDown += PromeniSemu;
                dpSemeIDugmad.Children.Add(brd);
                DockPanel.SetDock(brd, Dock.Left);
            }
        }
        List<Line> listaHorizontalnihLinija = new List<Line>();
        private void PopuniStolove()
        {
            Canvas.SetTop(dpStolovi, visinaVremena - 1);
            foreach(Line linija in listaHorizontalnihLinija)
            {
                canvasPrikaz.Children.Remove(linija);
            }
            dpStolovi.Children.Clear();
            int stoporedu = 0;
            Stolovi = BrokerSelect.dajSesiju().vratiStolove(IzabranaSema.Sema);
            foreach(re_Sto sto in Stolovi)
            {
                PrikazSto brd = new PrikazSto(visinaStola, sirinaStola,sto);
                dpStolovi.Children.Add(brd);
                DockPanel.SetDock(brd, Dock.Top);
                if (stoporedu == 0)
                {
                    brd.BorderThickness = new System.Windows.Thickness(0, 2, 2, 0);
                }
                else
                {
                    brd.BorderThickness = new System.Windows.Thickness(0, 1, 2, 0);
                }

                //dodavanje linija radi lakseg pregleda
                Line linija = new Line();
                Canvas.SetLeft(linija, sirinaStola);
                Canvas.SetTop(linija, visinaVremena); 
                linija.StrokeThickness = 2;
                linija.Stroke = Brushes.Black;
                linija.Opacity = 0.05;
                double[] dbl = new double[] { 4, 0 };
                linija.StrokeDashArray = new DoubleCollection(dbl);
                linija.Y1 = visinaStola * stoporedu +1; //+1 da bi izravnao sa borderom
                linija.Y2 = visinaStola * stoporedu +1;
                stoporedu++;
                linija.X1 = 0;
                linija.X2 = 5000;
                listaHorizontalnihLinija.Add(linija);
                canvasPrikaz.Children.Add(linija);

            }
            canvasPrikaz.Height = visinaStola * (Stolovi.Count + 1) - visinaVremena + 18; //18 je visina scrollbar-a
        }
        private void PromeniSemu(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MojBorderSema brd = sender as MojBorderSema;
            foreach(MojBorderSema brd1 in dpSemeIDugmad.Children.OfType<MojBorderSema>())
            {
                brd1.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#284b63");
            }
            brd.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#427aa1");
            if (brd != null)
            {
                IzabranaSema = brd.Sema;               
            }
            Stolovi = Broker.BrokerSelect.dajSesiju().vratiStolove(IzabranaSema.Sema);
            PopuniStolove();
            OsveziRadniProstor();
            PopuniSematskiPrikaz();
        }
        private void PopuniSematskiPrikaz()
        {
            canvasSematskiPrikaz.Children.Clear();
            Stolovi = Broker.BrokerSelect.dajSesiju().vratiStolove(IzabranaSema.Sema);
            foreach (re_Sto sto in Stolovi)
            {
                bool zauzetost = Broker.BrokerSelect.dajSesiju().ZauzetostStola(sto);                
                Border brd = new Border();
                brd.CornerRadius = new System.Windows.CornerRadius(3);
                if (sto.Oblik != 0)
                {
                    brd.CornerRadius = new System.Windows.CornerRadius(100);
                }
                Grid grd = new Grid();
                TextBlock tb = new TextBlock();
                tb.Foreground = Brushes.White;
                brd.Child = grd;
                grd.Children.Add(tb);
                brd.Opacity = 0.7;
                tb.Text = sto.Sto;
                tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                brd.Background = Brushes.Black;
                brd.Width = sto.Sirina / 5;
                brd.Height = sto.Visina / 5;
                canvasSematskiPrikaz.Children.Add(brd);
                Canvas.SetLeft(brd, sto.PozicijaLeft / 11 - 100);
                Canvas.SetTop(brd, sto.PozicijaTop / 11);
                if (zauzetost)
                {
                    Image znakuzvika = new Image();
                    BitmapImage bi2 = new BitmapImage();
                    znakuzvika.Source = bi2;
                    bi2.BeginInit();
                    bi2.UriSource = new Uri("/Resursi/Slike/ZnakUzvika.PNG", UriKind.RelativeOrAbsolute);
                    bi2.EndInit();
                    grd.Children.Add(znakuzvika);
                    znakuzvika.Height = brd.Height / 3;
                    znakuzvika.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    znakuzvika.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    znakuzvika.Margin = new Thickness(0, 0, -brd.Width + 20, 0);
                }
            }
        }
        private void PopuniVremena()
        {
            Canvas.SetLeft(dpVremena, sirinaStola - sirinaVremena / 2);
            DateTime dt = DateTime.Today;
            DateTime dtprovera = DateTime.Today;
            dt = dt.AddHours(7); //odakle krece dan
            do
            {
                //pravljenje i dodavanje vremena
                Border brd = new Border();
                brd.Background = Brushes.WhiteSmoke;
                brd.Width = sirinaVremena;
                brd.Height = visinaVremena - 20;
                brd.VerticalAlignment = VerticalAlignment.Top;
                brd.BorderThickness = new System.Windows.Thickness(0,0,0,1);
                brd.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#444444");
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Vertical;
                TextBlock tb = new TextBlock();
                brd.Child = sp;
                sp.Margin = new System.Windows.Thickness(0, 5, 0, 0);
                sp.Children.Add(tb);
                Rectangle rec = new Rectangle();
                rec.Width = 2;
                rec.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                rec.Height = 15;
                rec.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#444444");
                sp.Children.Add(rec);   
                tb.Text = dt.ToString("HH:mm");
                tb.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#444444");
                tb.FontWeight = FontWeights.Bold;
                tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                dt = dt.AddMinutes(30);
                tb.FontSize = 16;
                dpVremena.Children.Add(brd);
                DockPanel.SetDock(tb, Dock.Left);

                //dodavanje linija radi lakseg pregleda
                Line linija = new Line();
                linija.Stroke = Brushes.Black;
                linija.Opacity = 0.1;
                double[] dbl = new double[] { 6, 6 };
                linija.StrokeDashArray = new DoubleCollection(dbl);
                linija.Y1 = 0;
                linija.Y2 = 5000;
                linija.X1 = sirinaVremena*brojodeljka;
                linija.X2 = sirinaVremena * brojodeljka;
                canvasPrikaz.Children.Add(linija);
                Canvas.SetLeft(linija, sirinaStola);
                Canvas.SetTop(linija, visinaVremena);
                brojodeljka++;
            } while (dt.Day == dtprovera.Day);
            //hocu da mi vreme pocinje od 00:00 i zavrsava se sa 00:00
            Border brd1 = new Border();
            brd1.Background = Brushes.WhiteSmoke;
            brd1.Width = sirinaVremena;
            brd1.Height = visinaVremena;
            brd1.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#444444");
            brd1.BorderThickness = new System.Windows.Thickness(0, 0, 0, 1);
            StackPanel sp1 = new StackPanel();
            sp1.Orientation = Orientation.Vertical;
            TextBlock tb1 = new TextBlock();
            brd1.Child = sp1;
            sp1.Margin = new System.Windows.Thickness(0, 5, 0, 0);
            sp1.Children.Add(tb1);
            Rectangle rec1 = new Rectangle();
            rec1.Width = 2;
            rec1.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            rec1.Height = 15;
            rec1.Fill = Brushes.Black;
            sp1.Children.Add(rec1);
            tb1.Text = dt.ToString("HH:mm");
            tb1.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            tb1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            tb1.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#444444");
            tb1.FontWeight = FontWeights.Bold;
            dt = dt.AddMinutes(30);
            tb1.FontSize = 16;
            dpVremena.Children.Add(brd1);
            DockPanel.SetDock(tb1, Dock.Left);


            Line linija2 = new Line();
            linija2.Stroke = Brushes.Black;
            linija2.Opacity = 0.1;
            double[] dbl2 = new double[] { 6, 6 };
            linija2.StrokeDashArray = new DoubleCollection(dbl2);
            linija2.Y1 = 0;
            linija2.Y2 = 5000;
            linija2.X1 = sirinaVremena * brojodeljka;
            linija2.X2 = sirinaVremena * brojodeljka;
            canvasPrikaz.Children.Add(linija2);
            Canvas.SetLeft(linija2, sirinaStola);
            Canvas.SetTop(linija2, visinaVremena);
            brojodeljka++;
            SirinaCanvasa =  brojodeljka  * sirinaVremena + 30;
        }
        static v_Meni meni = new v_Meni("-","-", null);
        //pozicija misa pri kliku
        double mouseX;
        double mouseY;
        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }
        private void OtvoriRadniMeni_Metoda(object sender)
        {
            double mousey = Mouse.GetPosition(canvasPrikaz).X; //treba mi X koordinata da odredim koje je vreme
            double mousex = Mouse.GetPosition(canvasPrikaz).Y; //treba mi Y koordinata da odredim koji je sto

            mouseX = mousex;
            mouseY = mousey;

            //odredjujem vreme
            int vreme = Convert.ToInt32(Math.Floor((mousey - sirinaStola) / sirinaVremena));
            DateTime dt = DateTime.Today;
            dt = dt.AddHours(7);
            dt = dt.AddMinutes(vreme * 30);

            //odredjujem sto
            int pozicijasto = Convert.ToInt32(Math.Floor((mousex - visinaVremena) / visinaStola));
            re_Sto sto = Stolovi[pozicijasto];
            if (IsWindowOpen<v_Meni>())
            {
                meni.Close();
            }
            meni = new v_Meni(dt.ToString("HH:mm"), sto.Sto, sender);
            if (meni.ActionNovaRezervacija == null)
            {
                meni.ActionNovaRezervacija = new Action(NovaRezervacija);
            }
            meni.Left = Mouse.GetPosition(canvasPrikaz).X - 170 - svCanvas.HorizontalOffset;
            meni.Top = Mouse.GetPosition(canvasPrikaz).Y - svCanvas.VerticalOffset;
            if(sender is Border)
            {
                meni.Left = Mouse.GetPosition(canvasPrikaz).X - svCanvas.HorizontalOffset;
            }
            meni.Show();
        }
        public void NovaRezervacija()
        {
            meni.Close();

            int vreme = Convert.ToInt32(Math.Floor((mouseY - sirinaStola) / sirinaVremena));
            int pozicijasto = Convert.ToInt32(Math.Floor((mouseX - visinaVremena) / visinaStola));


            DateTime dt = DateTime.Today;
            dt = dt.AddHours(7);
            dt = dt.AddMinutes(vreme * 30);
            re_Sto sto = Stolovi[pozicijasto];

            ObservableCollection<re_Rezervacija> svapoklapanja = Broker.BrokerSelect.dajSesiju().VratiRezervacijeKojeSePoklapajuSaVremenom(dt, dt.AddMinutes(120), dt.Date);
            ObservableCollection<re_Rezervacija> poklapanjazasto = new ObservableCollection<re_Rezervacija>();

            foreach (re_Rezervacija rez in svapoklapanja)
            {
                string[] rezstolovi = rez.Sto.Split(",");
                foreach (string strsto in rezstolovi)
                {
                    if (strsto.Contains(sto.Sto) && strsto.Length == sto.Sto.Length)
                    {
                        poklapanjazasto.Add(rez);
                    }
                }
            }
            if (poklapanjazasto.Count() > 0)
            {
                v_MessageBox prozor = new v_MessageBox("U datom vremenu već postoji rezervacija. Da li želite da nastavite?");
                prozor.ShowDialog();
                if (!Globalno.Varijable.sacuvanePromene)
                {
                    return;
                }
            }
            re_Rezervacija novarezervacija = new re_Rezervacija();
            novarezervacija.Datum = IzabranDatum;
            novarezervacija.VremeOd = dt;
            novarezervacija.VremeDo = dt.AddMinutes(120);
            novarezervacija.IdGost = "0";
            novarezervacija.Objekat = Globalno.Varijable.Objekat.Objekat;
            novarezervacija.Sema = IzabranaSema.Sema;
            novarezervacija.Sto = sto.Sto;
            novarezervacija.BrojOdraslih = 2;
            novarezervacija.BrojDece = 0;
            bool upsesno = Broker.BrokerInsert.dajSesiju().UpisiRezervaciju(novarezervacija);          

            OsveziRadniProstor();

            //uc_Rezervacija brdRez = new uc_Rezervacija(novarezervacija);
            //brdRez.Width = 240;

            //canvasPrikaz.Children.Add(brdRez);
            //BrdRezervacije.Add(brdRez);

            //Canvas.SetLeft(brdRez, vreme * sirinaVremena + sirinaStola);
            //Canvas.SetTop(brdRez, pozicijasto * visinaStola + visinaVremena);
        }
        private void Zatvori_Metoda(object obj)
        {
            if (meni != null)
            {
                meni.Close();
            }
            v_Login prozor = new v_Login();
            prozor.Show();
            this.ZatvoriFormu();
        }
        public void UpdateNotifikacije()
        {
            spNotifikacije.Children.Clear();
            Notifikacije.Clear();
            ObservableCollection<re_Rezervacija> reNotifikacije = Broker.BrokerSelect.dajSesiju().VratiNotifikacije(DateTime.Now, DateTime.Today);
            foreach(re_Rezervacija rezervacija in reNotifikacije)
            {
                uc_Notifikacija notifikacija = new uc_Notifikacija(rezervacija);
                notifikacija.MouseLeftButtonDown += Notifikacija_OtvoriMeni;
                Notifikacije.Add(notifikacija);
                spNotifikacije.Children.Add(notifikacija);
            }
        }
        private void Notifikacija_OtvoriMeni(object sender, MouseButtonEventArgs e)
        {
            uc_Notifikacija notifikacija = sender as uc_Notifikacija;
            re_Rezervacija rezervacija = notifikacija.rezervacija;
            //rezervacijaKojaSePomera = new uc_Rezervacija(rezervacija);
            if (IsWindowOpen<v_Meni>())
            {
                meni.Close();
            }
            meni = new v_Meni(rezervacija.VremeOd.ToString("HH:mm"), rezervacija.Sto, sender);
            if (meni.ActionNovaRezervacija == null)
            {
                meni.ActionNovaRezervacija = new Action(NovaRezervacija);
                meni.ActionAktivirajRezervaciju = new Action(AktivirajRezervaciju);
                meni.ActionOtvoriDetaljeRezervacije = new Action(OtvoriDetaljeRezervacije);
                meni.ActionDeaktivirajRezervaciju = new Action(DeaktivirajRezervaciju);
            }
            meni.Left = Mouse.GetPosition(canvasPrikaz).X - 170 - svCanvas.HorizontalOffset;
            meni.Top = Mouse.GetPosition(canvasPrikaz).Y - svCanvas.VerticalOffset;
            meni.Show();
        }
    }
}
