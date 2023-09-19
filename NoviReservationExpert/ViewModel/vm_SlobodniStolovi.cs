using NoviReservationExpert.Model;
using NoviReservationExpert.View;
using NoviReservationExpert.View.UserKontrole;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NoviReservationExpert.ViewModel
{
    internal class vm_SlobodniStolovi : INotifyPropertyChanged
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
        public RelayCommand Zatvori_Command { get; set; }
        public RelayCommand NapraviRezervaciju_Command { get; set; }
        public RelayCommand TasterKlik_Command { get; private set; }
        public RelayCommand TastaturaObrisi_Command { get; private set; }
        public RelayCommand PrikaziTastaturuBrojevi_Command { get; private set; }
        public RelayCommand PrikaziTastaturuSlova_Command { get; private set; }
        public RelayCommand UgasiTastaturu_Command { get; private set; }
        public RelayCommand OtvoriTastaturu_Command { get; private set; }
        public RelayCommand TastaturaEnter_Command { get; private set; }
        public RelayCommand IzabranTB_Command { get; private set; }
        public RelayCommand PronadjiGosta_Command { get; private set; }
        public RelayCommand IzabranStoUListi_Command { get; private set; }
        #endregion

        #region PROPERTIJI
        ObservableCollection<re_Sema> _Seme;
        public ObservableCollection<re_Sema> Seme
        {
            get
            {
                return _Seme;
            }
            set
            {
                _Seme = value;
            }
        }
        ObservableCollection<re_Sto> _SlobodniStolovi;
        public ObservableCollection<re_Sto> SlobodniStolovi
        {
            get
            {
                return _SlobodniStolovi;
            }
            set
            {
                _SlobodniStolovi = value;
            }
        }
        ObservableCollection<re_Sto> _IzabraniStolovi;
        public ObservableCollection<re_Sto> IzabraniStolovi
        {
            get
            {
                return _IzabraniStolovi;
            }
            set
            {
                _IzabraniStolovi = value;
                NotifyPropertyChanged(nameof(IzabraniStolovi));
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
                if (DateTime.TryParse(value, out Vreme))
                {
                    PronadjiSlobodneStolove();                
                }
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
                int broj = 0;
                if (_BrojGostiju != "")
                {
                    if (int.TryParse(_BrojGostiju, out broj))
                    {
                        PronadjiSlobodneStolove();
                    }
                }
                NotifyPropertyChanged(nameof(BrojGostiju));
            }
        }
        string _GostIme;
        public string GostIme
        {
            get
            {
                return _GostIme;
            }
            set
            {
                _GostIme = value;
                NotifyPropertyChanged(nameof(GostIme));
            }
        }

        string _GostPrezime;
        public string GostPrezime
        {
            get
            {
                return _GostPrezime;
            }
            set
            {
                _GostPrezime = value;
                NotifyPropertyChanged(nameof(GostPrezime));
            }
        }
        string _Telefon;
        public string Telefon
        {
            get
            {
                return _Telefon;
            }
            set
            {
                _Telefon = value;
                NotifyPropertyChanged(nameof(Telefon));
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
                NotifyPropertyChanged(nameof(IzabranDatum));
            }
        }
        DateTime _VremeOD;
        public DateTime VremeOd
        {
            get
            {
                return _VremeOD;
            }
            set
            {
                _VremeOD = value;
                PronadjiSlobodneStolove();
                NotifyPropertyChanged(nameof(VremeOd));
            }
        }
        DateTime _VremeDo;
        public DateTime VremeDo
        {
            get
            {
                return _VremeDo;
            }
            set
            {
                _VremeDo = value;
                PronadjiSlobodneStolove();
                NotifyPropertyChanged(nameof(VremeDo));
            }
        }
        List<string> _listaVremena;
        public List<string> listaVremena
        {
            get
            {
                return _listaVremena;
            }
            set
            {
                _listaVremena = value;
            }
        }
        #region TASTATURA
        string _texttastature;
        public string Texttastature
        {
            get
            {
                return _texttastature;
            }
            set
            {
                _texttastature = value;
                NotifyPropertyChanged(nameof(Texttastature));
            }
        }
        Visibility _prikazTastatura = Visibility.Hidden;
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
        Visibility _Vis_tasteriVelikaSlova = Visibility.Visible;
        public Visibility Vis_tasteriVelikaSlova
        {
            get
            {
                return _Vis_tasteriVelikaSlova;
            }
            set
            {
                _Vis_tasteriVelikaSlova = value;
                NotifyPropertyChanged(nameof(Vis_tasteriVelikaSlova));
            }
        }
        Visibility _Vis_tasteriMalaSlova = Visibility.Hidden;
        public Visibility Vis_tasteriMalaSlova
        {
            get
            {
                return _Vis_tasteriMalaSlova;
            }
            set
            {
                _Vis_tasteriMalaSlova = value;
                NotifyPropertyChanged(nameof(Vis_tasteriMalaSlova));
            }
        }
        Visibility _Vis_tasteriBrojevi = Visibility.Visible;
        public Visibility Vis_tasteriBrojevi
        {
            get
            {
                return _Vis_tasteriBrojevi;
            }
            set
            {
                _Vis_tasteriBrojevi = value;
                NotifyPropertyChanged(nameof(Vis_tasteriBrojevi));
            }
        }
        #endregion
        #endregion

        DockPanel dpSlobodniStolovi;

        ObservableCollection<re_Sto> SviStolovi;
        List<Border> brdstolovi = new List<Border>();
        Canvas canvasStolovi;
        re_Sto izabranSto;
        DateTime Vreme;
        string izabranTB;
        public class canvasSto : Border
        {
            public bool izabransto = false;
            public bool slobodansto = true;
            public re_Sto sto;

            public canvasSto(re_Sto sto)
            {
                this.sto = sto;
                this.CornerRadius = new System.Windows.CornerRadius(3);
                if (sto.Oblik != 0)
                {
                    this.CornerRadius = new System.Windows.CornerRadius(100);
                }
                TextBlock tb = new TextBlock();
                tb.Foreground = Brushes.White;
                this.Child = tb;
                tb.Text = sto.Sto;
                tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                this.Width = sto.Sirina / 12;
                this.Height = sto.Visina / 12;
            }
        }
        public vm_SlobodniStolovi(Canvas canvasStolovi, DockPanel dpSlobodniStolovi) 
        {
            Zatvori_Command = new RelayCommand(ZatvoriProzor_Metoda);
            NapraviRezervaciju_Command = new RelayCommand(NapraviRezervaciju_Metoda);
            Seme = Broker.BrokerSelect.dajSesiju().vratiSeme();
            TasterKlik_Command = new RelayCommand(TasterKlik_Metoda);
            TastaturaObrisi_Command = new RelayCommand(TastaturaObrisi_Metoda);
            PrikaziTastaturuBrojevi_Command = new RelayCommand(PrikaziTastaturuBrojevi_Metoda);
            PrikaziTastaturuSlova_Command = new RelayCommand(PrikaziTastaturuSlova_Metoda);
            UgasiTastaturu_Command = new RelayCommand(UgasiTastaturu_Metoda);
            OtvoriTastaturu_Command = new RelayCommand(OtvoriTastaturu_Metoda);
            TastaturaEnter_Command = new RelayCommand(TastaturaEnter_Metoda);
            IzabranTB_Command = new RelayCommand(IzabranTB_Metoda);
            PronadjiGosta_Command = new RelayCommand(PronadjiGosta_Metoda);
            IzabranStoUListi_Command = new RelayCommand(IzabranStoUListi_Metoda);
            SviStolovi = new ObservableCollection<re_Sto>();

            this.dpSlobodniStolovi = dpSlobodniStolovi;

            IzabranDatum = DateTime.Today;
            Vreme = new DateTime();
            Vreme = DateTime.Now;

            DateTime temp = DateTime.Now;
            TimeSpan span = new TimeSpan(0, 30, 0);
            long ticks = temp.Ticks / span.Ticks;
            temp = new DateTime(ticks * span.Ticks,temp.Kind);
            VremeOd = temp;
            VremeDo = temp.AddMinutes(120);

            SviStolovi = Broker.BrokerSelect.dajSesiju().vratiStolove(Globalno.Varijable.IzabranaSema.Sema);

            this.canvasStolovi = canvasStolovi;
            IzabraniStolovi = new ObservableCollection<re_Sto>();

            listaVremena = new List<string>();
            temp = DateTime.Today;
            TimeSpan temp1 = new TimeSpan(7, 0, 0);
            temp = temp.Date + temp1;
            while (temp.Date == DateTime.Today)
            {
                listaVremena.Add(temp.ToString("HH:mm"));
                temp = temp.AddMinutes(30);
            }
            listaVremena.Add(temp.ToString("HH:mm"));

            BrojGostiju = 2.ToString();

            PronadjiSlobodneStolove();
        }

        private void IzabranStoUListi_Metoda(object obj)
        {
            re_Sto sto = obj as re_Sto;
            if (sto != null)
            {
                IzabraniStolovi.Remove(sto);
                foreach (canvasSto border in canvasStolovi.Children.OfType<canvasSto>())
                {
                    if (border.sto.Sto == sto.Sto)
                    {
                        if (!border.slobodansto)
                        {
                            border.Background = (Brush)Application.Current.FindResource("Crvena");

                        }
                        else
                        {
                            border.Background = (Brush)Application.Current.FindResource("Plava");
                        }
                        border.izabransto = false;
                    }
                }
                foreach (uc_PrikazSto prikaz in dpSlobodniStolovi.Children.OfType<uc_PrikazSto>())
                {
                    if (prikaz.sto == sto)
                    {
                        prikaz.IzabranSto();
                    }
                }
            }
        }

        private void PronadjiGosta_Metoda(object obj)
        {

            v_PronalazakGosta prozor = new v_PronalazakGosta();
            prozor.ShowDialog();
            if (Globalno.Varijable.IzabranGost != null)
            {
                GostIme = Globalno.Varijable.IzabranGost.Ime;
                GostPrezime = Globalno.Varijable.IzabranGost.Prezime;
                Telefon = Globalno.Varijable.IzabranGost.Telefon;
            }
            return;
        }

        #region TASTATURA
        private void IzabranTB_Metoda(object obj)
        {
            izabranTB = obj.ToString();
            Texttastature = "";
            prikazTastatura = Visibility.Visible;
        }
        private void TastaturaEnter_Metoda(object obj)
        {
            if(izabranTB != null)
            {
                switch (izabranTB)
                {
                    case "Vreme":
                        IzabranoVreme = Texttastature;
                        break;
                    case "Telefon":
                        Telefon = Texttastature;
                        break;
                    case "BrojGostiju":
                        BrojGostiju = Texttastature;
                        break;
                }
            }
            prikazTastatura = Visibility.Hidden;
            Texttastature = "";
        }
        private void UgasiTastaturu_Metoda(object obj)
        {
            Texttastature = "";
            prikazTastatura = Visibility.Hidden;
        }
        private void OtvoriTastaturu_Metoda(object obj)
        {
            Texttastature = "";
            prikazTastatura = Visibility.Visible;
        }
        private void TastaturaObrisi_Metoda(object obj)
        {
            if (Texttastature.Length != 0)
            {
                Texttastature = Texttastature.Substring(0, Texttastature.Length - 1);
            }
        }
        private void TasterKlik_Metoda(object obj)
        {
            Texttastature += obj.ToString();
        }
        private void PrikaziTastaturuSlova_Metoda(object obj)
        {
            if (Vis_tasteriVelikaSlova == Visibility.Visible)
            {
                Vis_tasteriVelikaSlova = Visibility.Hidden;
                Vis_tasteriMalaSlova = Visibility.Visible;
                Vis_tasteriBrojevi = Visibility.Hidden;
            }
            else if (Vis_tasteriMalaSlova == Visibility.Visible)
            {
                Vis_tasteriVelikaSlova = Visibility.Visible;
                Vis_tasteriMalaSlova = Visibility.Hidden;
                Vis_tasteriBrojevi = Visibility.Hidden;
            }
        }
        private void PrikaziTastaturuBrojevi_Metoda(object obj)
        {
            if (Vis_tasteriVelikaSlova == Visibility.Visible || Vis_tasteriMalaSlova == Visibility.Visible)
            {
                Vis_tasteriVelikaSlova = Visibility.Hidden;
                Vis_tasteriMalaSlova = Visibility.Hidden;
                Vis_tasteriBrojevi = Visibility.Visible;
            }
            else
            {
                Vis_tasteriVelikaSlova = Visibility.Hidden;
                Vis_tasteriMalaSlova = Visibility.Visible;
                Vis_tasteriBrojevi = Visibility.Hidden;
            }
        }
        #endregion
        private void PronadjiSlobodneStolove()
        {
            if (SlobodniStolovi != null) SlobodniStolovi.Clear();
            if (canvasStolovi != null) canvasStolovi.Children.Clear();
            
            SlobodniStolovi = new ObservableCollection<re_Sto>(SviStolovi.Where(x => x.BrojOsoba >= Convert.ToInt32(BrojGostiju)));

            ObservableCollection<re_Rezervacija> poklapanja = Broker.BrokerSelect.dajSesiju().VratiRezervacijeKojeSePoklapajuSaVremenom(VremeOd, VremeDo, IzabranDatum);

            
            foreach (re_Rezervacija rezervacija in poklapanja)
            {
                string[] stolovi = rezervacija.Sto.Split(",");
                foreach (string sto in stolovi)
                {
                    if (SlobodniStolovi.Contains(SlobodniStolovi.FirstOrDefault(x => x.Sto == sto)))
                        SlobodniStolovi.Remove(SlobodniStolovi.FirstOrDefault(x => x.Sto == sto));
                }
            }

            this.dpSlobodniStolovi.Children.Clear();

            foreach (re_Sto sto in SviStolovi)
            {
                uc_PrikazSto prikazsto = new uc_PrikazSto(60, 100, sto);
                prikazsto.MouseLeftButtonDown += PrikazSto_DodajSto;
                this.dpSlobodniStolovi.Children.Add(prikazsto);
                DockPanel.SetDock(prikazsto, Dock.Top);
                prikazsto.ProveriBrojLjudi(Convert.ToInt32(BrojGostiju));

                canvasSto brd = new canvasSto(sto);
                brd.MouseDown += IzaberiSto_Canvas;               
                if (SlobodniStolovi.Contains(sto))
                {
                    brd.Background = (Brush)Application.Current.FindResource("Plava");
                }
                else
                {
                    brd.slobodansto = false;
                    brd.Background = (Brush)Application.Current.FindResource("Crvena");
                    prikazsto.StoNeispunjavaUslove();
                }                
                canvasStolovi.Children.Add(brd);
                Canvas.SetLeft(brd, sto.PozicijaLeft / 19 - 100);
                Canvas.SetTop(brd, sto.PozicijaTop / 19 );
                brdstolovi.Add(brd);
            }
        }
        private void PrikazSto_DodajSto(object sender, MouseButtonEventArgs e)
        {
            uc_PrikazSto ucsto = sender as uc_PrikazSto;
            re_Sto sto = ucsto.sto;
            if (sto != null)
            {
                foreach (canvasSto border in canvasStolovi.Children.OfType<canvasSto>())
                {
                    if (border.sto.Sto == sto.Sto)
                    {
                        if (border.izabransto)
                        {
                            if (!border.slobodansto)
                            {
                                border.Background = (Brush)Application.Current.FindResource("Crvena");
                            }
                            else
                            {
                                border.Background = (Brush)Application.Current.FindResource("Plava");

                            }
                            border.izabransto = false;
                            goto Nastavak;
                        }
                        else
                        {
                            if (!border.slobodansto)
                            {
                                v_MessageBox prozor = new v_MessageBox("Ovaj sto ne ispunjava date kriterijume. Da li želite da nastavite?");
                                prozor.ShowDialog();
                                if (!Globalno.Varijable.sacuvanePromene)
                                {
                                    return;
                                }
                            }
                            border.Background = (Brush)Application.Current.FindResource("Plava_SV");
                            border.izabransto = true;
                            goto Nastavak;
                        }
                    }
                }
            Nastavak:
                izabranSto = SviStolovi.First(x => x.Sto == sto.Sto);
                if (IzabraniStolovi.Contains(sto))
                {
                    IzabraniStolovi.Remove(sto);
                    ucsto.IzabranSto();
                }
                else
                {
                    IzabraniStolovi.Add(sto);
                    ucsto.IzabranSto();
                }
            }
        }
        private void IzaberiSto_Canvas(object sender, MouseButtonEventArgs e)
        {
            canvasSto brd = sender as canvasSto;
            if (brd.izabransto)
            {
                brd.izabransto = false;
                TextBlock brdtb = brd.Child as TextBlock;
                foreach (uc_PrikazSto prikaz in dpSlobodniStolovi.Children.OfType<uc_PrikazSto>())
                {
                    if (prikaz.sto.Sto == brdtb.Text)
                    {
                        prikaz.IzabranSto();
                    }
                }
                IzabraniStolovi.Remove(brd.sto);
                if (!brd.slobodansto)
                {
                    brd.Background = (Brush)Application.Current.FindResource("Crvena");

                }
                else
                {
                    brd.Background = (Brush)Application.Current.FindResource("Plava");
                }
            }
            else
            {
                if (!brd.slobodansto)
                {
                    v_MessageBox prozor = new v_MessageBox("Ovaj sto ne ispunjava date kriterijume. Da li želite da nastavite?");
                    prozor.ShowDialog();
                    if (!Globalno.Varijable.sacuvanePromene)
                    {
                        return;
                    }
                }
                brd.izabransto = true;
                foreach (canvasSto border in brdstolovi)
                {
                    if (!border.izabransto)
                    {
                        if (!brd.slobodansto)
                        {
                            brd.Background = (Brush)Application.Current.FindResource("Crvena");

                        }
                        else
                        {
                            brd.Background = (Brush)Application.Current.FindResource("Plava");
                        }
                    }
                }
                brd.Background = (Brush)Application.Current.FindResource("Plava_SV");
                TextBlock brdtb = brd.Child as TextBlock;
                foreach (uc_PrikazSto prikaz in dpSlobodniStolovi.Children.OfType<uc_PrikazSto>())
                {
                    if (prikaz.sto.Sto == brdtb.Text)
                    {
                        prikaz.IzabranSto();
                    }
                }
                izabranSto = SviStolovi.First(x => x.Sto == brdtb.Text);
                IzabraniStolovi.Add(izabranSto);
            }

        }
        private void NapraviRezervaciju_Metoda(object obj)
        {
            if (izabranSto == null)
            {
                v_WarningBox prozor = new v_WarningBox("Nije izabran nijedan slobodan sto.");
                prozor.ShowDialog();
                return;
            }

            if(VremeOd > VremeDo)
            {
                v_WarningBox prozor = new v_WarningBox("Kraj rezervacije mora biti posle početka.");
                prozor.ShowDialog();
                return;
            }
            

            re_Rezervacija novarezervacija = new re_Rezervacija();
            novarezervacija.Datum = IzabranDatum;
            novarezervacija.VremeOd = VremeOd;
            novarezervacija.VremeDo = VremeDo;
            novarezervacija.IdGost = "0";
            novarezervacija.ImeGosta = GostIme;
            novarezervacija.BrojTelefona = Telefon;
            novarezervacija.PrezimeGosta = GostPrezime;
            novarezervacija.Objekat = Globalno.Varijable.Objekat.Objekat;
            novarezervacija.Sema = Globalno.Varijable.IzabranaSema.Sema;
            string stolovi = "";
            foreach(re_Sto sto in IzabraniStolovi)
            {
                stolovi += sto.Sto;
                stolovi += ",";
            }
            stolovi = stolovi.Remove(stolovi.Length - 1);
            novarezervacija.Sto = stolovi;
            novarezervacija.BrojOdraslih = Convert.ToInt32(BrojGostiju);
            novarezervacija.BrojDece = 0;
            Broker.BrokerInsert.dajSesiju().UpisiRezervaciju(novarezervacija);

            //Broker.BrokerInsert.dajSesiju().ZapisiLog();

            Globalno.Varijable.PoslednjaNovaRezervacija = novarezervacija;

            Globalno.Varijable.RadniProstor.OsveziRadniProstor();
            this.ZatvoriFormu();
        }       
        private void ZatvoriProzor_Metoda(object obj)
        {
            Globalno.Varijable.PoslednjaNovaRezervacija = null;
            this.ZatvoriFormu();
        }
    }
}
