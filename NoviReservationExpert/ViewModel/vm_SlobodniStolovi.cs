using NoviReservationExpert.Model;
using NoviReservationExpert.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                PronadjiSlobodneStolove();
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
        #endregion

        DockPanel dpSlobodniStolovi;
        ObservableCollection<re_Sto> SviStolovi;
        List<Border> brdstolovi = new List<Border>();
        Canvas canvasStolovi;
        re_Sto izabranSto;
        DateTime vreme;
        public vm_SlobodniStolovi(int brojgostiju, DateTime vreme, string Gost, DockPanel dpSlobodniStolovi, Canvas canvasStolovi) 
        {
            Zatvori_Command = new RelayCommand(ZatvoriProzor_Metoda);
            NapraviRezervaciju_Command = new RelayCommand(NapraviRezervaciju_Metoda);
            Seme = Broker.BrokerSelect.dajSesiju().vratiSeme();
            SviStolovi = new ObservableCollection<re_Sto>();
            //foreach(re_Sema sema in Seme)
            //{
            //    ObservableCollection<re_Sto> temp = new ObservableCollection<re_Sto>();
            //    temp = Broker.BrokerSelect.dajSesiju().vratiStolove(sema.Sema);
            //    foreach(re_Sto sto in temp)
            //    {
            //        SviStolovi.Add(sto);
            //    }
            //}

            SviStolovi = Broker.BrokerSelect.dajSesiju().vratiStolove(Globalno.Varijable.IzabranaSema.Sema);

            this.dpSlobodniStolovi = dpSlobodniStolovi;
            this.canvasStolovi = canvasStolovi;
            _BrojGostiju = brojgostiju.ToString();
            _IzabranoVreme = vreme.ToString("HH:mm dd.MM.yyyy");
            this.Gost = Gost;
            this.vreme = vreme;

            PronadjiSlobodneStolove();
        }

        private void PronadjiSlobodneStolove()
        {
            if(SlobodniStolovi != null) SlobodniStolovi.Clear();
            if (canvasStolovi != null) canvasStolovi.Children.Clear();
            if (dpSlobodniStolovi != null) dpSlobodniStolovi.Children.Clear();
            
            SlobodniStolovi = new ObservableCollection<re_Sto>(SviStolovi.Where(x => x.BrojOsoba >= Convert.ToInt32(BrojGostiju)));

            ObservableCollection<re_Rezervacija> poklapanja = Broker.BrokerSelect.dajSesiju().VratiRezervacijeKojeSePoklapajuSaVremenom(vreme, vreme.AddHours(2));

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

            foreach (re_Sto sto in SlobodniStolovi)
            {
                Border brd = new Border();
                brd.MouseDown += IzaberiSto_Canvas;
                brd.CornerRadius = new System.Windows.CornerRadius(3);
                if (sto.Oblik != 0)
                {
                    brd.CornerRadius = new System.Windows.CornerRadius(100);
                }
                TextBlock tb = new TextBlock();
                tb.Foreground = Brushes.White;
                brd.Child = tb;
                tb.Text = sto.Sto;
                tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                brd.Background = Brushes.Black;
                brd.Opacity = 0.2;
                brd.Width = sto.Sirina / 12;
                brd.Height = sto.Visina / 12;
                canvasStolovi.Children.Add(brd);
                Canvas.SetLeft(brd, sto.PozicijaLeft / 19 - 100);
                Canvas.SetTop(brd, sto.PozicijaTop / 19 );
                brdstolovi.Add(brd);
            }
        }

        private void IzaberiSto_Canvas(object sender, MouseButtonEventArgs e)
        {
            Border brd = sender as Border;
            foreach (Border border in brdstolovi)
            {
                border.Opacity = 0.2;
            }
            brd.Opacity = 1;
            TextBlock brdtb = brd.Child as TextBlock;
            foreach (Border stobrd in dpSlobodniStolovi.Children)
            {
                TextBlock tb = stobrd.Child as TextBlock;
                string sto = tb.Text.Substring(6);
                if(sto == brdtb.Text)
                {
                    stobrd.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#427aa1");
                } 
                else
                {
                    stobrd.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#284b63");
                }
            }
            izabranSto = SlobodniStolovi.First(x => x.Sto == brdtb.Text);
        }

        private void NapraviRezervaciju_Metoda(object obj)
        {
            if (izabranSto == null)
            {
                v_WarningBox prozor = new v_WarningBox("Nije izabran nijedan slobodan sto.");
                return;
            }

            re_Sto sto = izabranSto;
            re_Rezervacija novarezervacija = new re_Rezervacija();
            novarezervacija.Datum = vreme.Date;
            novarezervacija.VremeOd = vreme;
            novarezervacija.VremeDo = vreme.AddMinutes(120);
            novarezervacija.IdGost = "0";
            novarezervacija.Objekat = Globalno.Varijable.Objekat.Objekat;
            novarezervacija.Sema = Globalno.Varijable.IzabranaSema.Sema;
            novarezervacija.Sto = sto.Sto;
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
