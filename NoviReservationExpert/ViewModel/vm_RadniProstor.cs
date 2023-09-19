using NoviReservationExpert.Broker;
using NoviReservationExpert.Model;
using NoviReservationExpert.View;
using NoviReservationExpert.View.UserKontrole;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
        public RelayCommand Log_Command { get; private set; }
        public RelayCommand Konfiguracija_Command { get; private set; }
        #endregion

        #region PROPERTIJI
        int _RadniProstor_BlurRadious;
        public int RadniProstor_BlurRadious
        {
            get
            {
                return _RadniProstor_BlurRadious;
            }
            set
            {
                _RadniProstor_BlurRadious = value;
                NotifyPropertyChanged(nameof(RadniProstor_BlurRadious));
            }
        }

        int _crvenaLinijaSematskiLeft;
        public int crvenaLinijaSematskiLeft
        {
            get
            {
                return _crvenaLinijaSematskiLeft;
            }
            set
            {
                _crvenaLinijaSematskiLeft = value;
                NotifyPropertyChanged(nameof(crvenaLinijaSematskiLeft));
            }
        }
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
        int _SematskaSirinaCanvasa;
        public int SematskaSirinaCanvasa
        {
            get
            {
                return _SematskaSirinaCanvasa;
            }
            set
            {
                _SematskaSirinaCanvasa = value;
                NotifyPropertyChanged(nameof(SematskaSirinaCanvasa));
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
        Visibility _PrikazBrojaNotifikacija;
        public Visibility PrikazBrojaNotifikacija
        {
            get
            {
                return _PrikazBrojaNotifikacija;
            }
            set
            {
                _PrikazBrojaNotifikacija = value;
                NotifyPropertyChanged(nameof(PrikazBrojaNotifikacija));
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
        int _brojNotifikacija;
        public int BrojNotifikacija
        {
            get
            {
                return _brojNotifikacija;
            }
            set
            {
                _brojNotifikacija = value;
                NotifyPropertyChanged(nameof(BrojNotifikacija));
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
                this.CornerRadius = new System.Windows.CornerRadius(0);
                this.Background = (Brush)Application.Current.FindResource("Plava");
                StackPanel sp = new StackPanel();
                this.Child = sp;
                sp.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                sp.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                TextBlock tb = new TextBlock();
                if(sema.ToString().Count() > 9)
                {
                    tb.Text = sema.ToString().Substring(0, 10);
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
                slika.Background = (Brush)Application.Current.FindResource("Siva");
                slika.CornerRadius = new System.Windows.CornerRadius(3);
                slika.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                sp.Children.Add(slika);
            }
        }
        #endregion
        Popup BiranjeIkoniceStola;
        List<Line> listaHorizontalnihLinija = new List<Line>();
        List<Line> listaHorizontalnihLinijaSematski = new List<Line>();
        List<Line> listaVertikalnihLinijaSematski = new List<Line>();
        ObservableCollection<re_Sema> Seme = new ObservableCollection<re_Sema>();
        ObservableCollection<re_Sto> Stolovi = new ObservableCollection<re_Sto>();
        ObservableCollection<re_Sto> StoloviSematski = new ObservableCollection<re_Sto>();
        ObservableCollection<re_Rezervacija> Rezervacije = new ObservableCollection<re_Rezervacija>();
        ObservableCollection<re_Rezervacija> RezervacijeSematski = new ObservableCollection<re_Rezervacija>();
        List<uc_Notifikacija> Notifikacije = new List<uc_Notifikacija>();
        uc_Notifikacija IzabranaNotifikacija;
        List<uc_Rezervacija> BrdRezervacije = new List<uc_Rezervacija>(); //rezervacije koje se prikazuju, za svaku rezervaciju u Rezervacije, ovde postoji border
        List<uc_Rezervacija> BrdSematskeRezervacije = new List<uc_Rezervacija>(); //rezervacije koje se prikazuju, za svaku rezervaciju u Rezervacije, ovde postoji border
        DockPanel dpSemeIDugmad;
        DockPanel dpVremena;
        DockPanel dpStolovi;
        DockPanel dpVremenaSematski;
        DockPanel dpStoloviSematski;
        StackPanel spNotifikacije;
        ScrollViewer svCanvas;
        Canvas canvasPrikaz;
        Canvas canvasSematskiPrikaz;
        public int visinaProstoraZaBookmark = 20;
        public int sirinaVremena = 60;
        public int visinaVremena = 60;
        public int visinaStola = 60;
        public int sirinaStola = 120;
        public int brojodeljka = 0;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public vm_RadniProstor(DockPanel dpSemeIDugmad, DockPanel dpStolovi, DockPanel dpVremena, Canvas prikazTabelarni, Canvas prikazSematski, ScrollViewer svCanvas, StackPanel spNotifikacije, DockPanel dpStoloviSematski, DockPanel dpVremenaSematski)
        {
            Zatvori_Command = new RelayCommand(Zatvori_Metoda);
            OtvoriRadniMeni_Command = new RelayCommand(OtvoriRadniMeni_Metoda);
            DodajNovuRezervaciju_Command = new RelayCommand(DodajNovuRezervaciju_Metoda);
            PromenaEkrana_Command = new RelayCommand(PromenaEkrana_Metoda);
            PregledRezervacija_Command = new RelayCommand(PregledRezervacija_Metoda);
            ZatvoriMeni_Command = new RelayCommand(ZatvoriMeni_Metoda);
            PromenaDatuma_Command = new RelayCommand(PromenaDatuma_Metoda);
            Log_Command = new RelayCommand(Log_Metoda);
            Konfiguracija_Command = new RelayCommand(Konfiguracija_Metoda);
            //----------PARAMETRI
            string automatskoMenjanjeStatusa = Broker.BrokerSelect.dajSesiju().VratiParametar_AutomatskoMenjanjeStatusa();
            if(automatskoMenjanjeStatusa == "Da")
            {
                Globalno.Varijable.AutomatskoMenjanjeStatusa = true;
            }
            else
            {
                Globalno.Varijable.AutomatskoMenjanjeStatusa = false;
            }
            string prikazivanjeotkazanihstolova = Broker.BrokerSelect.dajSesiju().VratiParametar_PrikazivanjeOtkazanihStolova();
            if (prikazivanjeotkazanihstolova == "Da")
            {
                Globalno.Varijable.PrikazujOtkazane = true;
            }
            else
            {
                Globalno.Varijable.PrikazujOtkazane = false;
            }

            BrokerUpdate.dajSesiju().UpdateSveRezervacijePreDanasnjegDana();
            Seme = Broker.BrokerSelect.dajSesiju().vratiSeme(); // automatski vraca seme za izabran objekat sa login-a

            PrikazBrojaNotifikacija = Visibility.Collapsed;

            RadniProstor_BlurRadious = 0;
           
            canvasPrikaz = prikazTabelarni;
            canvasSematskiPrikaz = prikazSematski;
            canvasPrikaz.MouseMove += Pomeranje;
            this.dpStolovi = dpStolovi;
            this.dpVremena = dpVremena;
            this.dpStoloviSematski = dpStoloviSematski;
            this.dpVremenaSematski = dpVremenaSematski;
            this.svCanvas = svCanvas;
            this.spNotifikacije = spNotifikacije;
            this.dpSemeIDugmad = dpSemeIDugmad;
       
            v_PrikazSematski = Visibility.Hidden;
            v_PrikazTabelarni = Visibility.Visible;

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();

            this.IzabranDatum = DateTime.Today;

            foreach (re_Sema sema in Seme)
            {
                if (sema.Odabran == 1)
                {
                    IzabranaSema = sema;
                }
            }
            if(IzabranaSema == null)
            {
                IzabranaSema = Seme[0];
            }
            Stolovi = BrokerSelect.dajSesiju().vratiStolove(IzabranaSema.Sema);

            PopuniVremena();
            PopuniStolove();
            PopuniPrikazSema();

            foreach (MojBorderSema brd1 in dpSemeIDugmad.Children.OfType<MojBorderSema>())
            {
                if (brd1.Sema.Odabran == 1)
                {
                    brd1.Background = (Brush)Application.Current.FindResource("Plava_SV");
                }
            }

            OsveziRadniProstor();
        }
        private void Konfiguracija_Metoda(object obj)
        {
            RadniProstor_BlurRadious = 10;
            v_Konfiguracija prozor = new v_Konfiguracija();
            prozor.ShowDialog();
            RadniProstor_BlurRadious = 0;
        }
        private void Log_Metoda(object obj)
        {
            RadniProstor_BlurRadious = 10;
            v_IstorijaDogadjaja prozor = new v_IstorijaDogadjaja();
            prozor.ShowDialog();
            RadniProstor_BlurRadious = 0;
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
            RadniProstor_BlurRadious = 10;
            v_PregledRezervacija prozor = new v_PregledRezervacija();
            prozor.ShowDialog();
            RadniProstor_BlurRadious = 0;
        }
        private void PromenaEkrana_Metoda(object obj)
        {
            this.v_PrikazSematski = v_PrikazSematski == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            this.v_PrikazTabelarni = v_PrikazTabelarni == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            if(this.v_PrikazSematski == Visibility.Visible)
            {
                PopuniSematskiPrikaz();
            }
        }
        private void DodajNovuRezervaciju_Metoda(object obj)
        {
            RadniProstor_BlurRadious = 10;
            v_SlobodniStolovi prozor = new v_SlobodniStolovi();
            prozor.ShowDialog();
            RadniProstor_BlurRadious = 0;

            if (Globalno.Varijable.PoslednjaNovaRezervacija == null)
            {
                return;
            }
            if (Globalno.Varijable.PoslednjaNovaRezervacija.Datum.Date != IzabranDatum.Date)
            {
                return;
            }
            string[] stolovi = Globalno.Varijable.PoslednjaNovaRezervacija.Sto.Split(",");
            int index = Stolovi.IndexOf(Stolovi.FirstOrDefault(x => x.Sto == stolovi[0]));
            svCanvas.ScrollToVerticalOffset(index * visinaStola);
        }
        private void dispatcherTimer_Tick(object? sender, EventArgs e)
        {
            UpdateStatusRezervacija();
            NapraviNotifikacije();
            UpdateNotifikacije();
        }
        private void UpdateNotifikacije()
        {
            foreach (re_Rezervacija rezervacija in Rezervacije)
            {
                UpdateNotifikaciju(rezervacija);
            }
        }
        private void UpdateStatusRezervacija()
        {
            if (!Globalno.Varijable.AutomatskoMenjanjeStatusa)
            {
                return;
            }
            if (IzabranDatum != DateTime.Today) return; 
            foreach(re_Rezervacija rezervacija in Rezervacije)
            {
                if (rezervacija.Status != -1 && rezervacija.VremeDo < DateTime.Now)
                {
                    Broker.BrokerUpdate.dajSesiju().UpdateRezervaciju(rezervacija.Id, 2);
                    rezervacija.Status = 2;
                    UpdateNotifikaciju(rezervacija);
                    continue;
                }
                if (rezervacija.Status == 0 && rezervacija.VremeOd < DateTime.Now && rezervacija.VremeDo > DateTime.Now)
                {
                    Broker.BrokerUpdate.dajSesiju().UpdateRezervaciju(rezervacija.Id, 1);
                    rezervacija.Status = 1;
                    UpdateNotifikaciju(rezervacija);
                    continue;
                }
            }
            foreach(uc_Rezervacija ucrez in BrdRezervacije)
            {
                ucrez.UpdateIzgledUOdnosuNaStatus();
            }

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
                if (!Globalno.Varijable.PrikazujOtkazane)
                {
                    if (rezervacija.Status == -1) continue;
                }
                int vremeUMin = (rezervacija.VremeOd.Hour - 7) * sirinaVremena + rezervacija.VremeOd.Minute;
                int ypozicija = sirinaStola + vremeUMin * 2;

                string[] stolovi = rezervacija.Sto.Split(",");
                foreach(string sto in stolovi)
                {
                    uc_Rezervacija brdRez = new uc_Rezervacija(rezervacija, sto);
                    int xpozicija = Stolovi.IndexOf(Stolovi.FirstOrDefault(x => x.Sto == sto));
                    Panel.SetZIndex(brdRez, xpozicija * 100);
                    brdRez.MouseLeftButtonDown += PocetakPomeranja;
                    brdRez.MouseLeftButtonUp += ZavrsetakPomeranja;

                    canvasPrikaz.Children.Add(brdRez);
                    BrdRezervacije.Add(brdRez);
                    ProveraPreklapanja(brdRez,BrdRezervacije);
                    Canvas.SetLeft(brdRez, ypozicija);
                    Canvas.SetTop(brdRez, visinaVremena + xpozicija * visinaStola);

                    brdRez.ProveraKapacitetaStola(Stolovi.FirstOrDefault(x => x.Sto == sto));
                }
            }      
        }
        private void ProveraPreklapanja(uc_Rezervacija ucrez, List<uc_Rezervacija> BrdRezervacije)
        {
            foreach(uc_Rezervacija rezervacija in BrdRezervacije)
            {
                if (rezervacija.ucrez_sto != ucrez.ucrez_sto) continue;
                if(rezervacija != ucrez)
                {
                    if((ucrez.Rezervacija.VremeOd < rezervacija.Rezervacija.VremeDo) && (ucrez.Rezervacija.VremeDo > rezervacija.Rezervacija.VremeOd))
                    {
                        rezervacija.OtkrijObelezivac();
                        if (rezervacija.zamracen) continue;
                        ucrez.Preklapanje();
                        Panel.SetZIndex(rezervacija, Stolovi.IndexOf(Stolovi.FirstOrDefault(x => x.Sto == rezervacija.ucrez_sto)) * 100);
                        if (ucrez.Rezervacija.VremeOd == rezervacija.Rezervacija.VremeOd)
                        {
                            rezervacija.Preklapanje();
                            ucrez.PomeriObelezivac();
                        }
                        return;
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
            if (IsWindowOpen<v_Meni>())
            {
                meni.Close();
            }
            if (pomeranje == false)
            {
                if (rezervacijaKojaSePomera == null) return;

                if (Panel.GetZIndex(rezervacijaKojaSePomera) == Stolovi.IndexOf(Stolovi.FirstOrDefault(x => x.Sto == rezervacijaKojaSePomera.ucrez_sto)) * 100 + 1)
                {
                    meni = new v_Meni(rezervacija.Rezervacija.VremeOd.ToString("HH:mm"), rezervacija.Rezervacija.Sto, sender);
                    if (meni.ActionNovaRezervacija == null)
                    {
                        meni.ActionNovaRezervacija = new Action(NovaRezervacija);
                        meni.ActionAktivirajRezervaciju = new Action(AktivirajRezervaciju);
                        meni.ActionOtvoriDetaljeRezervacije = new Action(OtvoriDetaljeRezervacije);
                        meni.ActionDeaktivirajRezervaciju = new Action(DeaktivirajRezervaciju);
                        meni.ActionZavrsiRezervaciju = new Action(ZavrsiRezervaciju);
                        meni.ActionVratiURezervisano = new Action(VratiURezervisano);
                    }
                    meni.Left = Mouse.GetPosition(canvasPrikaz).X - 170 - svCanvas.HorizontalOffset;
                    meni.Top = Mouse.GetPosition(canvasPrikaz).Y - svCanvas.VerticalOffset + visinaProstoraZaBookmark;
                    meni.Show();
                }
                else
                {
                    Panel.SetZIndex(rezervacijaKojaSePomera, Stolovi.IndexOf(Stolovi.FirstOrDefault(x => x.Sto == rezervacijaKojaSePomera.ucrez_sto)) * 100 + 1);
                    foreach (uc_Rezervacija prerezervacija in BrdRezervacije)
                    {
                        if (prerezervacija.ucrez_sto != rezervacijaKojaSePomera.ucrez_sto) continue;
                        if (prerezervacija != rezervacijaKojaSePomera)
                        {
                            if ((rezervacijaKojaSePomera.Rezervacija.VremeOd < prerezervacija.Rezervacija.VremeDo) && (rezervacijaKojaSePomera.Rezervacija.VremeDo > prerezervacija.Rezervacija.VremeOd))
                            {
                                Panel.SetZIndex(prerezervacija, Stolovi.IndexOf(Stolovi.FirstOrDefault(x => x.Sto == rezervacija.ucrez_sto)) * 100);
                            }
                        }
                    }
                }

                pocetakPomeranja = false;
                return;
            }
            if (pocetakPomeranja)
            {
                pocetakPomeranja = false;
                pomeranje = false;

                rezervacijaKojaSePomera.RezervacijaJePrestalaDaSePomera();

                int vremeUMin = (rezervacijaKojaSePomera.Rezervacija.VremeOd.Hour - 7) * sirinaVremena + rezervacijaKojaSePomera.Rezervacija.VremeOd.Minute;
                double pocetnaY = sirinaStola + vremeUMin * 2;
                double pocetnaX = Stolovi.IndexOf(Stolovi.First(x => x.Sto == rezervacijaKojaSePomera.ucrez_sto));

                double koordinataX = Canvas.GetLeft(rezervacijaKojaSePomera) - sirinaStola - sirinaVremena;
                double koordinataY = Canvas.GetTop(rezervacijaKojaSePomera) - visinaVremena;

                int xbrojCelina = Convert.ToInt32(Math.Floor(koordinataX / sirinaVremena + 1)); //-1
                if (xbrojCelina < 0) xbrojCelina = 0;
                
                int ybrojCelina = Convert.ToInt32(Math.Floor(koordinataY / visinaStola )); //-1
                if (ybrojCelina < 0) ybrojCelina = 0;

                TimeSpan trajanje = rezervacijaKojaSePomera.Rezervacija.VremeDo - rezervacijaKojaSePomera.Rezervacija.VremeOd;

                DateTime osnova = IzabranDatum.Date;

                DateTime pocetak = osnova.AddHours(7).AddMinutes(xbrojCelina * 30);
                DateTime kraj = pocetak.AddMinutes(trajanje.TotalMinutes);
                re_Sto sto = Stolovi[ybrojCelina];

                if (DaLiRezervacijaVecZauzimaSto(rezervacijaKojaSePomera, sto.Sto)) //da li rezervacija vec zauzizma sto, tj ako prevacim jednu borduru u drugi red
                {
                    RadniProstor_BlurRadious = 10;
                    v_WarningBox prozor = new v_WarningBox($"Ova rezervacija već zauzima ovaj sto.");
                    prozor.ShowDialog();
                    RadniProstor_BlurRadious = 0;
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
                
                if (sto.BrojOsoba < rezervacijaKojaSePomera.Rezervacija.BrojOdraslih) // da li sto na koji sam prevukao borduru podrzava broj osoba rezervacija
                {
                    RadniProstor_BlurRadious = 10;
                    v_MessageBox prozor = new v_MessageBox($"Sto {sto.Sto} ne podržava broj osoba rezervacije. Želite li da nastavite?");
                    prozor.ShowDialog();
                    RadniProstor_BlurRadious = 0;
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

                rezervacijaKojaSePomera.ProveraKapacitetaStola(sto); //ako ne podrzava, pocrvenim deo sa brojem na prikazu

                //DA LI POSTOJI POKLAPANJE
                ObservableCollection<re_Rezervacija> preklapanja = Broker.BrokerSelect.dajSesiju().VratiRezervacijeKojeSePoklapajuSaVremenom(pocetak, kraj, pocetak.Date, rezervacijaKojaSePomera.Rezervacija.Id);
                ObservableCollection<uc_Rezervacija> UCpreklapanja = DaLiPostojiPreklapanje(preklapanja,sto);
                if (UCpreklapanja.Any())
                {
                    RadniProstor_BlurRadious = 10;
                    v_MessageBox poruka = new v_MessageBox("U datom terminu već postoji rezervacija. Da li želite da nastavite?");
                    poruka.ShowDialog();
                    RadniProstor_BlurRadious = 0;
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

                string promene = $"Korisnik {Globalno.Varijable.Korisnik.LogOnIme} menja rezervaciju {rezervacijaKojaSePomera.Rezervacija.Id}.";
                string staristolovi = rezervacijaKojaSePomera.Rezervacija.Sto;
                rezervacijaKojaSePomera.Rezervacija.Sto = "";
                rezervacijaKojaSePomera.ucrez_sto = sto.Sto;
                foreach (uc_Rezervacija rez in BrdRezervacije)
                {
                    if (rez.Rezervacija.Id == rezervacijaKojaSePomera.Rezervacija.Id)
                    {
                        rezervacijaKojaSePomera.Rezervacija.Sto += rez.ucrez_sto;
                        rezervacijaKojaSePomera.Rezervacija.Sto += ",";
                        continue;
                    }
                }

                rezervacijaKojaSePomera.Rezervacija.Sto = rezervacijaKojaSePomera.Rezervacija.Sto.Remove(rezervacijaKojaSePomera.Rezervacija.Sto.Length - 1);
                if (rezervacijaKojaSePomera.Rezervacija.Sto != staristolovi) promene += $"Promena stola sa '{staristolovi}' na '{rezervacijaKojaSePomera.Rezervacija.Sto}'. ";


                if (rezervacijaKojaSePomera.Rezervacija.VremeOd != pocetak) promene += $"Promena početka rezervacije sa " + rezervacijaKojaSePomera.Rezervacija.VremeOd.ToString("HH:mm") + " na " + pocetak.ToString("HH:mm") + ". ";
                rezervacijaKojaSePomera.Rezervacija.VremeOd = pocetak;
                if (rezervacijaKojaSePomera.Rezervacija.VremeDo != kraj) promene += $"Promena kraja rezervacije sa " + rezervacijaKojaSePomera.Rezervacija.VremeDo.ToString("HH:mm") + " na " + kraj.ToString("HH:mm") + ". ";
                rezervacijaKojaSePomera.Rezervacija.VremeDo = kraj;
                rezervacijaKojaSePomera.PromeniVreme(pocetak, kraj);
                BrokerUpdate.dajSesiju().UpdateRezervaciju(rezervacijaKojaSePomera.Rezervacija);
                BrokerInsert.dajSesiju().ZapisiLog(9100, promene, "Radni prostor");
            }
            this.OsveziRadniProstor();
        }
        private ObservableCollection<uc_Rezervacija> DaLiPostojiPreklapanje(ObservableCollection<re_Rezervacija> preklapanja, re_Sto sto)
        {
            ObservableCollection<uc_Rezervacija> UCpreklapanja = new ObservableCollection<uc_Rezervacija>();
            foreach (re_Rezervacija rez in preklapanja)
            {
                foreach (uc_Rezervacija ucrez in BrdRezervacije)
                {
                    if (ucrez.Rezervacija.Id == rez.Id)
                    {
                        UCpreklapanja.Add(ucrez);
                    }
                }
            }
            for (int i = UCpreklapanja.Count - 1; i >= 0; i--)
            {
                if (UCpreklapanja[i].ucrez_sto != sto.Sto)
                {
                    UCpreklapanja.RemoveAt(i);
                }
            }
            return UCpreklapanja;
        }
        private bool DaLiRezervacijaVecZauzimaSto(uc_Rezervacija rezervacijaKojaSePomera, string sto)
        {
            string[] stolovi = rezervacijaKojaSePomera.Rezervacija.Sto.Split(",");
            foreach (string strsto in stolovi)
            {
                if (strsto.Contains(sto) && strsto.Length == sto.Length)
                {
                    if (rezervacijaKojaSePomera.ucrez_sto != sto)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private void ZavrsiRezervaciju()
        {
            meni.Close();
            Broker.BrokerUpdate.dajSesiju().UpdateRezervaciju(rezervacijaKojaSePomera.Rezervacija.Id, 2);
            rezervacijaKojaSePomera.Rezervacija.Status = 2;
            rezervacijaKojaSePomera.UpdateIzgledUOdnosuNaStatus();
        }
        private void VratiURezervisano()
        {
            meni.Close();
            Broker.BrokerUpdate.dajSesiju().UpdateRezervaciju(rezervacijaKojaSePomera.Rezervacija.Id, 0);
            rezervacijaKojaSePomera.Rezervacija.Status = 0;
            rezervacijaKojaSePomera.UpdateIzgledUOdnosuNaStatus();
        }
        private void DeaktivirajRezervaciju()
        {
            meni.Close();
            Broker.BrokerUpdate.dajSesiju().UpdateRezervaciju(rezervacijaKojaSePomera.Rezervacija.Id, -1);
            rezervacijaKojaSePomera.Rezervacija.Status = -1;
            rezervacijaKojaSePomera.UpdateIzgledUOdnosuNaStatus();
        }
        private void OtvoriDetaljeRezervacije()
        {
            meni.Close();
            RadniProstor_BlurRadious = 10;

            v_DetaljiRezervacije prozor = new v_DetaljiRezervacije(rezervacijaKojaSePomera.Rezervacija);
            prozor.ShowDialog();
            RadniProstor_BlurRadious = 0;

        }
        private void AktivirajRezervaciju()
        {
            meni.Close();
            Broker.BrokerUpdate.dajSesiju().UpdateRezervaciju(rezervacijaKojaSePomera.Rezervacija.Id, 1);
            rezervacijaKojaSePomera.Rezervacija.Status = 1;
            rezervacijaKojaSePomera.UpdateIzgledUOdnosuNaStatus();
        }
        #endregion
        public void OsveziRadniProstor() //<-------------------------
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
                brd.Background = (Brush)Application.Current.FindResource("RadniProstor");
                brd.Width = sirinaVremena;
                brd.Height = visinaVremena - visinaProstoraZaBookmark;
                brd.VerticalAlignment = VerticalAlignment.Top;
                brd.BorderThickness = new System.Windows.Thickness(0, 0, 0, 1);
                brd.BorderBrush = (Brush)Application.Current.FindResource("Siva");
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
                rec.Fill = (Brush)Application.Current.FindResource("Siva");
                sp.Children.Add(rec);
                tb.Text = dt.ToString("HH:mm");
                tb.Foreground = (Brush)Application.Current.FindResource("Siva");
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
                linija.X1 = sirinaVremena * brojodeljka;
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
            brd1.BorderBrush = (Brush)Application.Current.FindResource("Siva");
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
            tb1.Foreground = (Brush)Application.Current.FindResource("Siva");
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
            SirinaCanvasa = brojodeljka * sirinaVremena + 30;
        }
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
                uc_PrikazSto brd = new uc_PrikazSto(visinaStola, sirinaStola,sto);
                brd.ProveriZauzetost();
                dpStolovi.Children.Add(brd);
                DockPanel.SetDock(brd, Dock.Top);

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
            canvasPrikaz.Height = visinaStola * (Stolovi.Count + 1) + 18 - 20; //18 je visina scrollbar-a
        }
        private void PromeniSemu(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MojBorderSema brd = sender as MojBorderSema;
            foreach(MojBorderSema brd1 in dpSemeIDugmad.Children.OfType<MojBorderSema>())
            {
                brd1.Background = (Brush)Application.Current.FindResource("Plava");
            }
            brd.Background = (Brush)Application.Current.FindResource("Plava_SV");
            if (brd != null)
            {
                IzabranaSema = brd.Sema;               
            }
            Stolovi = Broker.BrokerSelect.dajSesiju().vratiStolove(IzabranaSema.Sema);
            PopuniStolove();
            OsveziRadniProstor();
            PopuniSematskiPrikaz();
        }
        private void PopuniSematskiPrikaz() // <---------------------
        {
            RezervacijeSematski = BrokerSelect.dajSesiju().VratiRezervacijeZaDatum(IzabranDatum, Globalno.Varijable.Objekat.Objekat, IzabranaSema.Sema);
            for(int i = RezervacijeSematski.Count - 1; i >= 0; i--)
            {
                re_Rezervacija rez = RezervacijeSematski[i];
                if(rez.VremeDo < DateTime.Now)
                {
                    RezervacijeSematski.RemoveAt(i);
                }
            }
            RezervacijeSematski = new ObservableCollection<re_Rezervacija>(RezervacijeSematski.OrderBy(x => x.VremeOd));
            if (RezervacijeSematski.Count > 0)
            {
                PopuniStoloveSematski(RezervacijeSematski);
                PopuniVremenaSematski(RezervacijeSematski);
                PopuniSematskiPrikazRezervacijama(RezervacijeSematski);
            }
        }
        private void PopuniStoloveSematski(ObservableCollection<re_Rezervacija> RezervacijeSematski) // saljem zato sto mi trebaju samo stolovi iz ove liste
        {
            dpStoloviSematski.Children.Clear();
            Canvas.SetTop(dpStoloviSematski, visinaVremena - 1);
            foreach (Line linija in listaHorizontalnihLinijaSematski)
            {
                canvasSematskiPrikaz.Children.Remove(linija);
            }
            dpStoloviSematski.Children.Clear();
            int stoporedu = 0;
            StoloviSematski.Clear();
            foreach (re_Sto sto1 in Stolovi)
            {
                foreach (re_Rezervacija rez in RezervacijeSematski)
                {
                    if(rez.Status == -1)
                    {
                        if (Globalno.Varijable.PrikazujOtkazane)
                        {
                            goto Nastavak;
                        } else
                        {
                            continue;
                        }
                    }
                    Nastavak:
                    string[] stolovi = rez.Sto.Split(",");
                    foreach (string sto in stolovi)
                    {
                        if (sto1.Sto == sto)
                        {
                            if (!StoloviSematski.Contains(sto1))
                            {
                                StoloviSematski.Add(sto1);
                            }
                        }
                    }
                        
                }
            }
            foreach (re_Sto sto in StoloviSematski)
            {
                uc_PrikazSto brd = new uc_PrikazSto(visinaStola, sirinaStola, sto);
                brd.ProveriZauzetost();
                dpStoloviSematski.Children.Add(brd);
                DockPanel.SetDock(brd, Dock.Top);

                //dodavanje linija radi lakseg pregleda
                Line linija = new Line();
                Canvas.SetLeft(linija, sirinaStola);
                Canvas.SetTop(linija, visinaVremena);
                linija.StrokeThickness = 2;
                linija.Stroke = Brushes.Black;
                linija.Opacity = 0.05;
                double[] dbl = new double[] { 4, 0 };
                linija.StrokeDashArray = new DoubleCollection(dbl);
                linija.Y1 = visinaStola * stoporedu + 1; //+1 da bi izravnao sa borderom
                linija.Y2 = visinaStola * stoporedu + 1;
                stoporedu++;
                linija.X1 = 0;
                linija.X2 = 5000;
                listaHorizontalnihLinijaSematski.Add(linija);
                canvasSematskiPrikaz.Children.Add(linija);
            }
            Line linija1 = new Line();
            Canvas.SetLeft(linija1, sirinaStola);
            Canvas.SetTop(linija1, visinaVremena);
            linija1.StrokeThickness = 2;
            linija1.Stroke = Brushes.Black;
            linija1.Opacity = 0.05;
            double[] db2 = new double[] { 4, 0 };
            linija1.StrokeDashArray = new DoubleCollection(db2);
            linija1.Y1 = visinaStola * stoporedu + 1; //+1 da bi izravnao sa borderom
            linija1.Y2 = visinaStola * stoporedu + 1;

            linija1.X1 = 0;
            linija1.X2 = 5000;
            listaHorizontalnihLinijaSematski.Add(linija1);
            canvasSematskiPrikaz.Children.Add(linija1);

            canvasSematskiPrikaz.Height = visinaStola * (StoloviSematski.Count + 1) + 18 - 20; //18 je visina scrollbar-a
        }
        private void PopuniVremenaSematski(ObservableCollection<re_Rezervacija> RezervacijeSematski)
        {
            int brojodeljkasematski = 0;
            foreach (Line linija in listaVertikalnihLinijaSematski)
            {
                canvasSematskiPrikaz.Children.Remove(linija);
            }
            dpVremenaSematski.Children.Clear();
            Canvas.SetLeft(dpVremenaSematski, sirinaStola - sirinaVremena / 2);
            DateTime dt = DateTime.Today;
            DateTime dtprovera = DateTime.Today;
            dt = dt.AddHours(RezervacijeSematski[0].VremeOd.Hour); //odakle krece dan
            dt = dt.AddMinutes(RezervacijeSematski[0].VremeOd.Minute); //odakle krece dan
            do
            {
                //pravljenje i dodavanje vremena
                Border brd = new Border();
                brd.Background = Brushes.WhiteSmoke;
                brd.Width = sirinaVremena;
                brd.Height = visinaVremena - visinaProstoraZaBookmark;
                brd.VerticalAlignment = VerticalAlignment.Top;
                brd.BorderThickness = new Thickness(0, 0, 0, 1);
                brd.BorderBrush = (Brush)Application.Current.FindResource("Siva");
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Vertical;
                TextBlock tb = new TextBlock();
                brd.Child = sp;
                sp.Margin = new Thickness(0, 5, 0, 0);
                sp.Children.Add(tb);
                Rectangle rec = new Rectangle();
                rec.Width = 2;
                rec.HorizontalAlignment = HorizontalAlignment.Center;
                rec.Height = 15;
                rec.Fill = (Brush)Application.Current.FindResource("Siva");
                sp.Children.Add(rec);
                tb.Text = dt.ToString("HH:mm");
                tb.Foreground = (Brush)Application.Current.FindResource("Siva");
                tb.FontWeight = FontWeights.Bold;
                tb.HorizontalAlignment = HorizontalAlignment.Center;
                tb.VerticalAlignment = VerticalAlignment.Center;
                dt = dt.AddMinutes(30);
                tb.FontSize = 16;
                dpVremenaSematski.Children.Add(brd);
                DockPanel.SetDock(tb, Dock.Left);

                //dodavanje linija radi lakseg pregleda
                Line linija = new Line();
                listaVertikalnihLinijaSematski.Add(linija);
                linija.Stroke = Brushes.Black;
                linija.Opacity = 0.1;
                double[] dbl = new double[] { 6, 6 };
                linija.StrokeDashArray = new DoubleCollection(dbl);
                linija.Y1 = 0;
                linija.Y2 = (StoloviSematski.Count - 1) * visinaStola + visinaVremena;
                linija.X1 = sirinaVremena * brojodeljkasematski;
                linija.X2 = sirinaVremena * brojodeljkasematski;
                canvasSematskiPrikaz.Children.Add(linija);
                Canvas.SetLeft(linija, sirinaStola);
                Canvas.SetTop(linija, visinaVremena);
                brojodeljkasematski++;
            } while (dt.Day == dtprovera.Day);
            //hocu da mi vreme pocinje od 00:00 i zavrsava se sa 00:00
            Border brd1 = new Border();
            brd1.Background = Brushes.WhiteSmoke;
            brd1.Width = sirinaVremena;
            brd1.Height = visinaVremena - visinaProstoraZaBookmark;
            brd1.BorderBrush = (Brush)Application.Current.FindResource("Siva");
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
            tb1.Foreground = (Brush)Application.Current.FindResource("Siva");
            tb1.FontWeight = FontWeights.Bold;
            dt = dt.AddMinutes(30);
            tb1.FontSize = 16;
            dpVremenaSematski.Children.Add(brd1);
            DockPanel.SetDock(tb1, Dock.Left);


            Line linija2 = new Line();
            listaVertikalnihLinijaSematski.Add(linija2);
            linija2.Stroke = Brushes.Black;
            linija2.Opacity = 0.1;
            double[] dbl2 = new double[] { 6, 6 };
            linija2.StrokeDashArray = new DoubleCollection(dbl2);
            linija2.Y1 = 0;
            linija2.Y2 = (StoloviSematski.Count - 1) * visinaStola + visinaVremena;
            linija2.X1 = sirinaVremena * brojodeljkasematski;
            linija2.X2 = sirinaVremena * brojodeljkasematski;
            canvasSematskiPrikaz.Children.Add(linija2);
            Canvas.SetLeft(linija2, sirinaStola);
            Canvas.SetTop(linija2, visinaVremena);
            brojodeljkasematski++;
            SematskaSirinaCanvasa = brojodeljkasematski * sirinaVremena + 30;

            int totalnominuta = (DateTime.Now.Hour - RezervacijeSematski[0].VremeOd.Hour) * 60 + DateTime.Now.Minute;
            crvenaLinijaSematskiLeft = totalnominuta * 2 + sirinaStola - 20;
        }
        private void PopuniSematskiPrikazRezervacijama(ObservableCollection<re_Rezervacija> RezervacijeSematski)
        {
            for (int i = canvasSematskiPrikaz.Children.Count - 1; i >= 0; i--)
            {
                if (canvasSematskiPrikaz.Children[i].GetType() == typeof(uc_Rezervacija))
                {
                    canvasSematskiPrikaz.Children.RemoveAt(i);
                }
            }
            BrdSematskeRezervacije.Clear();
            foreach (re_Rezervacija rezervacija in RezervacijeSematski)
            {
                int vremeUMin = (rezervacija.VremeOd.Hour - RezervacijeSematski[0].VremeOd.Hour) * sirinaVremena + rezervacija.VremeOd.Minute - RezervacijeSematski[0].VremeOd.Minute;
                int ypozicija = sirinaStola + vremeUMin * 2;

                string[] stolovi = rezervacija.Sto.Split(",");
                foreach (string sto in stolovi)
                {
                    uc_Rezervacija brdRez = new uc_Rezervacija(rezervacija, sto);
                    int xpozicija = StoloviSematski.IndexOf(StoloviSematski.FirstOrDefault(x => x.Sto == sto));
                    Panel.SetZIndex(brdRez, xpozicija * 100);

                    canvasSematskiPrikaz.Children.Add(brdRez);
                    BrdSematskeRezervacije.Add(brdRez);
                    ProveraPreklapanja(brdRez, BrdSematskeRezervacije);
                    Canvas.SetLeft(brdRez, ypozicija);
                    Canvas.SetTop(brdRez, visinaVremena + xpozicija * visinaStola);

                    brdRez.ProveraKapacitetaStola(StoloviSematski.FirstOrDefault(x => x.Sto == sto));
                }
            }
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

            if(mouseY < sirinaStola)
            {
                return;
            }

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
            meni.Top = Mouse.GetPosition(canvasPrikaz).Y - svCanvas.VerticalOffset + visinaProstoraZaBookmark;
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
                RadniProstor_BlurRadious = 10;
                v_MessageBox prozor = new v_MessageBox("U datom vremenu već postoji rezervacija. Da li želite da nastavite?");
                prozor.ShowDialog();
                RadniProstor_BlurRadious = 0;
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
        private void UpdateNotifikaciju(re_Rezervacija rezervacija)
        {
            if (IzabranDatum != DateTime.Today) return;
            foreach (uc_Notifikacija notifikacija in Notifikacije)
            {
                if(notifikacija.rezervacija.Id == rezervacija.Id)
                {
                    notifikacija.Update(rezervacija);
                }
            }           
        }
        private void NapraviNotifikacije()
        {
            ObservableCollection<re_Rezervacija> novenotifikacije = Broker.BrokerSelect.dajSesiju().VratiNotifikacije(DateTime.Today);
            foreach(uc_Notifikacija notifikacija in Notifikacije)
            {
                for(int i = novenotifikacije.Count - 1; i >= 0; i--)
                {
                    if(notifikacija.rezervacija.Id == novenotifikacije[i].Id)
                    {
                        novenotifikacije.RemoveAt(i);
                    }
                }
            }
            foreach(re_Rezervacija rezervacija in novenotifikacije)
            {
                uc_Notifikacija notifikacija = new uc_Notifikacija(rezervacija);
                notifikacija.MouseEnter += Notifikacija_MouseEnter;
                notifikacija.MouseLeftButtonDown += Notifikacija_OtvoriMeni;
                Notifikacije.Add(notifikacija);
                spNotifikacije.Children.Add(notifikacija);
                BrojNotifikacija++;
            }
            if(BrojNotifikacija > 0)
            {
                PrikazBrojaNotifikacija = Visibility.Visible;
            }

        }
        private void Notifikacija_MouseEnter(object sender, MouseEventArgs e)
        {
            uc_Notifikacija notifikacija = sender as uc_Notifikacija;
            notifikacija.VidjenaNotifikacija();
            if(BrojNotifikacija != 0)
            {
                BrojNotifikacija--;
            } 
            if(BrojNotifikacija == 0)
            {
                PrikazBrojaNotifikacija = Visibility.Collapsed;
            }
        }
        private void Notifikacija_OtvoriMeni(object sender, MouseButtonEventArgs e)
        {
            uc_Notifikacija notifikacija = sender as uc_Notifikacija;
            IzabranaNotifikacija = notifikacija;
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
                meni.ActionZavrsiRezervaciju = new Action(ZavrsiRezervaciju);
                meni.ActionSkloniNotifikaciju = new Action(SkloniNotifikaciju);
                meni.ActionVratiURezervisano = new Action(VratiURezervisano);
            }
            meni.Left = Mouse.GetPosition(canvasPrikaz).X - 170 - svCanvas.HorizontalOffset;
            meni.Top = Mouse.GetPosition(canvasPrikaz).Y - svCanvas.VerticalOffset;
            meni.Show();
        }
        private void SkloniNotifikaciju()
        {
            if (IsWindowOpen<v_Meni>())
            {
                meni.Close();
            }
            if (IzabranaNotifikacija != null)
            {
                IzabranaNotifikacija.Visibility = Visibility.Collapsed;
            }
        }
    }
}
