using NoviReservationExpert.Model;
using NoviReservationExpert.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
                if(_BrojGostiju != "") PronadjiSlobodneStolove();
                NotifyPropertyChanged(nameof(BrojGostiju));
            }
        }
        string _Gost;
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
                PronadjiSlobodneStolove();
                NotifyPropertyChanged(nameof(IzabranDatum));
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
                this.Opacity = 0.2;
                this.Width = sto.Sirina / 12;
                this.Height = sto.Visina / 12;
            }
        }

        public vm_SlobodniStolovi(DockPanel dpSlobodniStolovi, Canvas canvasStolovi) 
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
            SviStolovi = new ObservableCollection<re_Sto>();

            Vreme = new DateTime();
            Vreme = DateTime.Now;
            IzabranoVreme = Vreme.ToString("HH:mm");

            SviStolovi = Broker.BrokerSelect.dajSesiju().vratiStolove(Globalno.Varijable.IzabranaSema.Sema);

            this.dpSlobodniStolovi = dpSlobodniStolovi;
            this.canvasStolovi = canvasStolovi;
            this.Gost = Gost;
            IzabraniStolovi = new ObservableCollection<re_Sto>();

            BrojGostiju = 2.ToString();

            PronadjiSlobodneStolove();
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
                    case "Gost":
                        Gost = Texttastature;
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
            if (dpSlobodniStolovi != null) dpSlobodniStolovi.Children.Clear();
            
            SlobodniStolovi = new ObservableCollection<re_Sto>(SviStolovi.Where(x => x.BrojOsoba >= Convert.ToInt32(BrojGostiju)));

            ObservableCollection<re_Rezervacija> poklapanja = Broker.BrokerSelect.dajSesiju().VratiRezervacijeKojeSePoklapajuSaVremenom(Vreme, Vreme.AddHours(2), IzabranDatum);

            foreach (re_Rezervacija rezervacija in poklapanja)
            {
                if (SlobodniStolovi.Contains(SlobodniStolovi.FirstOrDefault(x => x.Sto == rezervacija.Sto)))
                    SlobodniStolovi.Remove(SlobodniStolovi.FirstOrDefault(x => x.Sto == rezervacija.Sto));
            }

            foreach (re_Sto sto in SlobodniStolovi)
            {
                Border brd = new Border();
                brd.Height = 35;
                brd.Margin = new System.Windows.Thickness(3);
                brd.CornerRadius = new System.Windows.CornerRadius(3);
                brd.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#284b63");
                TextBlock tb = new TextBlock();
                brd.Child = tb;
                tb.Foreground = Brushes.White;
                tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                tb.Text = "Sto - " + sto.Sto;
                brd.MouseLeftButtonDown += IzaberiSto_Dockpanel;
                tb.Margin = new System.Windows.Thickness(5, 0, 0, 0);
                dpSlobodniStolovi.Children.Add(brd);
                DockPanel.SetDock(brd, Dock.Top);
            }

            foreach (re_Sto sto in SviStolovi)
            {
                canvasSto brd = new canvasSto(sto);
                brd.MouseDown += IzaberiSto_Canvas;               
                if (SlobodniStolovi.Contains(sto))
                {
                    brd.Background = Brushes.Black;
                }
                else
                {
                    brd.slobodansto = false;
                    brd.Background = Brushes.Red;
                }                
                canvasStolovi.Children.Add(brd);
                Canvas.SetLeft(brd, sto.PozicijaLeft / 19 - 100);
                Canvas.SetTop(brd, sto.PozicijaTop / 19 );
                brdstolovi.Add(brd);
            }
        }
        private void IzaberiSto_Canvas(object sender, MouseButtonEventArgs e)
        {
            canvasSto brd = sender as canvasSto;
            if (brd.izabransto)
            {
                brd.izabransto = false;
                IzabraniStolovi.Remove(brd.sto);
                brd.Opacity = 0.2;
                TextBlock brdtb = brd.Child as TextBlock;
                foreach (Border stobrd in dpSlobodniStolovi.Children)
                {
                    TextBlock tb = stobrd.Child as TextBlock;
                    string sto = tb.Text.Substring(6);
                    if (sto == brdtb.Text)
                    {
                        stobrd.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#284b63");
                    }
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
                        border.Opacity = 0.2;
                    }
                }
                brd.Opacity = 1;               
                TextBlock brdtb = brd.Child as TextBlock;
                foreach (Border stobrd in dpSlobodniStolovi.Children)
                {
                    TextBlock tb = stobrd.Child as TextBlock;
                    string sto = tb.Text.Substring(6);
                    if (sto == brdtb.Text)
                    {
                        stobrd.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#427aa1");
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
                return;
            }

            re_Rezervacija novarezervacija = new re_Rezervacija();
            novarezervacija.Datum = Vreme.Date;
            novarezervacija.VremeOd = Vreme;
            novarezervacija.VremeDo = Vreme.AddMinutes(120);
            novarezervacija.IdGost = "0";
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

            Globalno.Varijable.RadniProstor.OsveziRadniProstor();
            this.ZatvoriFormu();
        }
        private void IzaberiSto_Dockpanel(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            foreach (Border border in brdstolovi)
            {
                border.Opacity = 0.2;
            }
            foreach (Border stobrd in dpSlobodniStolovi.Children)
            {
                stobrd.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#284b63");
            }
            Border brd = sender as Border;
            brd.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#427aa1");
            TextBlock tb = brd.Child as TextBlock;
            string sto = tb.Text.Substring(6);
            foreach(Border border in brdstolovi)
            {
                TextBlock bordertb = border.Child as TextBlock;
                if (bordertb.Text == sto)
                {
                    border.Opacity = 1;
                }
            }
            izabranSto = SlobodniStolovi.First(x => x.Sto == sto);
        }
        private void ZatvoriProzor_Metoda(object obj)
        {
            this.ZatvoriFormu();
        }
    }
}
