using NoviReservationExpert.Broker;
using NoviReservationExpert.Model;
using NoviReservationExpert.View;
using NoviReservationExpert.View.UserKontrole;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xceed.Wpf.AvalonDock.Themes;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace NoviReservationExpert.ViewModel
{
    internal class vm_DetaljiRezervacije : INotifyPropertyChanged
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
        public RelayCommand PrimeniPromene_Command { get; set; }
        public RelayCommand Odustani_Command { get; set; }
        public RelayCommand PronadjiGosta_Command { get; set; }
        public RelayCommand IzabranStoUListi_Command { get; private set; }
        #endregion

        #region PROPERTIJI
        re_Rezervacija _Rezervacija;
        public re_Rezervacija Rezervacija
        {
            get
            {
                return _Rezervacija;
            }
            set
            {
                _Rezervacija = value;
                NotifyPropertyChanged(nameof(Rezervacija));
            }
        }

        string _Naslov;
        public string Naslov
        {
            get
            {
                return _Naslov;
            }
            set
            {
                _Naslov = value;
                NotifyPropertyChanged(nameof(Naslov));
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

        DateTime _Datum;
        public DateTime Datum
        {
            get
            {
                return _Datum;
            }
            set 
            { 
                _Datum = value;
                NotifyPropertyChanged(nameof(Datum));
            }
        }

        string _VremeOd;
        public string VremeOd
        {
            get
            {
                return _VremeOd;
            }
            set
            {
                _VremeOd = value;
                if (DateTime.TryParse(value, out vremeOd))
                {
                    PronadjiSlobodneStolove();
                }
                NotifyPropertyChanged(nameof(VremeOd));
            }
        }

        string _VremeDo;
        public string VremeDo
        {
            get
            {
                return _VremeDo;
            }
            set
            {
                _VremeDo = value;
                if (DateTime.TryParse(value, out vremeDo))
                {
                    PronadjiSlobodneStolove();
                }
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
        string _BrojOdraslih;
        public string BrojOdraslih
        {
            get
            {
                return _BrojOdraslih;
            }
            set
            {
                _BrojOdraslih = value;
                if (_BrojOdraslih != "") PronadjiSlobodneStolove();
                NotifyPropertyChanged(nameof(BrojOdraslih));
            }
        }
        string _BrojDece;
        public string BrojDece
        {
            get
            {
                return _BrojDece;
            }
            set
            {
                _BrojDece = value;
                NotifyPropertyChanged(nameof(BrojDece));
            }
        }
        string _Napomena;
        public string Napomena
        {
            get
            {
                return _Napomena;
            }
            set
            {
                _Napomena = value;
                NotifyPropertyChanged(nameof(Napomena));
            }
        }
        Visibility _vis_Kalendar;
        public Visibility vis_Kalendar
        {
            get
            {
                return _vis_Kalendar;
            }
            set
            {
                _vis_Kalendar = value;
                NotifyPropertyChanged(nameof(vis_Kalendar));
            }
        }
        #endregion

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
                this.Background = Brushes.Black;
                this.CornerRadius = new CornerRadius(3);
                this.Child = tb;
                tb.Text = sto.Sto;
                tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                this.Width = sto.Sirina / 12;
                this.Height = sto.Visina / 12;
            }
        }
        DateTime vremeDo;
        DateTime vremeOd;
        string staristolovi = "";
        Canvas canvasStolovi;
        ObservableCollection<re_Sto> sviStolovi = new ObservableCollection<re_Sto>();
        re_Sto prikazanSto = new re_Sto();
        List<Border> brdstolovi = new List<Border>();
        public vm_DetaljiRezervacije(re_Rezervacija rezervacija, Canvas canvasStolovi)
        {
            Odustani_Command = new RelayCommand(Odustani_Metoda);
            PrimeniPromene_Command = new RelayCommand(PrimeniPromene_Metoda);
            PronadjiGosta_Command = new RelayCommand(PronadjiGosta_Metoda);
            IzabranStoUListi_Command = new RelayCommand(IzabranStoUListi_Metoda);
            IzabraniStolovi = new ObservableCollection<re_Sto>();
            this.Rezervacija = rezervacija;
            this.canvasStolovi = canvasStolovi;
            this.prikazanSto = Broker.BrokerSelect.dajSesiju().VratiSto(rezervacija.Sema, rezervacija.Sto);
            this.sviStolovi = Broker.BrokerSelect.dajSesiju().vratiStolove(rezervacija.Sema);

            vis_Kalendar = Visibility.Hidden;
            staristolovi = rezervacija.Sto;

            string[] stolovi = rezervacija.Sto.Split(",");
            foreach (string sto in stolovi)
            {
                re_Sto izabransto = sviStolovi.FirstOrDefault(x => x.Sto == sto);
                IzabraniStolovi.Add(izabransto);
            }
            Naslov = "Rezervacija - " + rezervacija.Id;
            GostIme = rezervacija.ImeGosta;
            GostPrezime = rezervacija.PrezimeGosta;
            Telefon = rezervacija.BrojTelefona;
            Datum = rezervacija.Datum;
            VremeOd = rezervacija.VremeOd.ToString("HH:mm");
            VremeDo = rezervacija.VremeDo.ToString("HH:mm");
            BrojOdraslih = rezervacija.BrojOdraslih.ToString();
            BrojDece = rezervacija.BrojDece.ToString();
            Napomena = rezervacija.Napomena;

            listaVremena = new List<string>();
            DateTime temp = DateTime.Today;
            TimeSpan temp1 = new TimeSpan(7, 0, 0);
            temp = temp.Date + temp1;
            while (temp.Date == DateTime.Today)
            {
                listaVremena.Add(temp.ToString("HH:mm"));
                temp = temp.AddMinutes(30);
            }
            listaVremena.Add(temp.ToString("HH:mm"));

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
            }
        }

        private void PronadjiSlobodneStolove()
        {
            if (SlobodniStolovi != null) SlobodniStolovi.Clear();
            if (canvasStolovi != null) canvasStolovi.Children.Clear();

            vremeOd = Rezervacija.VremeOd;
            vremeDo = Rezervacija.VremeDo;
            SlobodniStolovi = new ObservableCollection<re_Sto>(sviStolovi.Where(x => x.BrojOsoba >= Convert.ToInt32(BrojOdraslih)));

            ObservableCollection<re_Rezervacija> poklapanja = Broker.BrokerSelect.dajSesiju().VratiRezervacijeKojeSePoklapajuSaVremenom(vremeOd, vremeDo, Datum.Date);

            foreach (re_Rezervacija rezervacija in poklapanja)
            {
                if (SlobodniStolovi.Contains(SlobodniStolovi.FirstOrDefault(x => x.Sto == rezervacija.Sto)))
                    SlobodniStolovi.Remove(SlobodniStolovi.FirstOrDefault(x => x.Sto == rezervacija.Sto));
            }


            foreach (re_Sto sto in sviStolovi)
            {
                canvasSto brd = new canvasSto(sto);
                brd.MouseDown += Brd_MouseLeftButtonDown;
                if (SlobodniStolovi.Contains(sto))
                {
                    brd.Background = (Brush)Application.Current.FindResource("Plava");
                }
                else
                {
                    brd.slobodansto = false;
                    brd.Background = (Brush)Application.Current.FindResource("Crvena");
                }
                if (IzabraniStolovi.Contains(sto))
                {
                    brd.izabransto = true;
                    brd.Background = (Brush)Application.Current.FindResource("Plava_SV");
                }
                canvasStolovi.Children.Add(brd);
                Canvas.SetLeft(brd, sto.PozicijaLeft / 19 - 100);
                Canvas.SetTop(brd, sto.PozicijaTop / 19);
                brdstolovi.Add(brd);
            }
        }
        private void Brd_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            canvasSto brd = sender as canvasSto;
            if (brd.izabransto)
            {
                brd.izabransto = false;
                IzabraniStolovi.Remove(brd.sto);
                if (!brd.slobodansto)
                {
                    brd.Background = (Brush)Application.Current.FindResource("Crvena");

                }
                else
                {
                    brd.Background = (Brush)Application.Current.FindResource("Plava");
                }
                TextBlock brdtb = brd.Child as TextBlock;              
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
                foreach (canvasSto canvasborder in canvasStolovi.Children.OfType<canvasSto>())
                {
                    if (!canvasborder.izabransto && !IzabraniStolovi.Contains(canvasborder.sto))
                    {
                        if (!canvasborder.slobodansto)
                        {
                            canvasborder.Background = (Brush)Application.Current.FindResource("Crvena");

                        }
                        else
                        {
                            canvasborder.Background = (Brush)Application.Current.FindResource("Plava");
                        }
                    }
                }
                brd.Background = (Brush)Application.Current.FindResource("Plava_SV");
                TextBlock brdtb = brd.Child as TextBlock;               
                IzabraniStolovi.Add(sviStolovi.First(x => x.Sto == brdtb.Text));
            }
        }
        private void PronadjiGosta_Metoda(object obj)
        {
            v_PronalazakGosta prozor = new v_PronalazakGosta();
            prozor.ShowDialog();
            if(Globalno.Varijable.IzabranGost != null)
            {
                Rezervacija.BrojTelefona = Globalno.Varijable.IzabranGost.Telefon;
                GostIme = Globalno.Varijable.IzabranGost.Ime;
                GostPrezime = Globalno.Varijable.IzabranGost.Prezime;
                Telefon = Globalno.Varijable.IzabranGost.Telefon;
            }
            return;
        }
        private void PrimeniPromene_Metoda(object obj)
        {
            string promene = "";
            DateTime vreme = DateTime.Now;
            Rezervacija.Datum = Datum.Date;
            DateTime.TryParse(VremeOd, out vreme);
            if (Rezervacija.VremeOd != vreme) promene += $"Početak rezervacije promenjen, sa " + Rezervacija.VremeOd.ToString("HH:mm") + " na " +  vreme.ToString("HH:mm") + ". ";
            Rezervacija.VremeOd = Rezervacija.Datum.Date + vreme.TimeOfDay;
            DateTime.TryParse(VremeDo, out vreme);
            if (Rezervacija.VremeDo != vreme) promene += $"Kraj rezervacije promenjen, sa " + Rezervacija.VremeDo.ToString("HH:mm") + " na " + vreme.ToString("HH:mm") + ". ";
            Rezervacija.VremeDo = Rezervacija.Datum.Date + vreme.TimeOfDay;
            if (Rezervacija.BrojOdraslih != Convert.ToInt32(BrojOdraslih)) promene += $"Broj osoba rezervacije promenjeno sa '{Rezervacija.BrojOdraslih}' na '{Convert.ToInt32(BrojOdraslih)}'. ";
            Rezervacija.BrojOdraslih = Convert.ToInt32(BrojOdraslih);
            Rezervacija.BrojDece = Convert.ToInt32(BrojDece);
            if (Rezervacija.Napomena != Napomena) promene += $"Promenjena napomena sa '{Rezervacija.Napomena}' na '{Napomena}'. ";
            Rezervacija.Napomena = Napomena;
            if (Rezervacija.BrojTelefona != Telefon) promene += $"Promenjen broj telefona sa '{Rezervacija.BrojTelefona}' na '{Telefon}'. ";
            Rezervacija.BrojTelefona = Telefon;
            string stolovi = "";
            if (IzabraniStolovi.Count > 0)
            {
                foreach (re_Sto sto in IzabraniStolovi)
                {
                    stolovi += sto.Sto;
                    stolovi += ",";
                }
                stolovi = stolovi.Remove(stolovi.Length - 1);
            } else
            {
                v_MessageBox prozor = new v_MessageBox("Nije izabran nijedan sto!");
                prozor.ShowDialog();
                return;
            }
            if (Rezervacija.Sto != stolovi) promene += $"Promenjen izbor stolova sa '{Rezervacija.Sto}' na '{stolovi}'. ";
            Rezervacija.Sto = stolovi;
            if (Rezervacija.ImeGosta != GostIme) promene += $"Promenjeno ime nosioca rezervacije sa '{Rezervacija.ImeGosta}' na '{GostIme}'. ";
            Rezervacija.ImeGosta = GostIme;
            if (Rezervacija.PrezimeGosta != GostPrezime) promene += $"Promenjeno prezime nosioca rezervacije sa '{Rezervacija.ImeGosta}' na '{GostIme}'. ";
            Rezervacija.PrezimeGosta = GostPrezime;
            if (Rezervacija.BrojTelefona != Telefon) promene += $"Promenjen telefon nosioca rezervacije sa '{Rezervacija.BrojTelefona}' na '{Telefon}'. ";
            Rezervacija.BrojTelefona = Telefon;

            bool test = Broker.BrokerUpdate.dajSesiju().UpdateRezervaciju(Rezervacija);
            if (test)
            {
                BrokerInsert.dajSesiju().ZapisiLog(9100, $"Promena rezervacije - {Rezervacija.Id} od strane koristnika {Globalno.Varijable.Korisnik.LogOnIme}. {promene}");
            } else
            {
                v_WarningBox prozor = new v_WarningBox("Rezervacija nije uspešno ažurirana!");
                prozor.ShowDialog();
            }
            Globalno.Varijable.RadniProstor.OsveziRadniProstor();
            this.ZatvoriFormu();
        }
        private void Odustani_Metoda(object obj)
        {
            this.ZatvoriFormu();
        }
    }
}
