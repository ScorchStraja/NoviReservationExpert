using NoviReservationExpert.Model;
using NoviReservationExpert.View;
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

        string _Datum;
        public string Datum
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
                NotifyPropertyChanged(nameof(VremeDo));
            }
        }

        string _Sto;
        public string Sto 
        {
            get
            {
                return _Sto;
            }
            set
            {
                _Sto = value;
                NotifyPropertyChanged(nameof(Sto));
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
        #endregion

        Canvas canvasStolovi;
        ObservableCollection<re_Sto> sviStolovi = new ObservableCollection<re_Sto>();
        re_Sto prikazanSto = new re_Sto();
        public vm_DetaljiRezervacije(re_Rezervacija rezervacija, Canvas canvasStolovi)
        {
            Odustani_Command = new RelayCommand(Odustani_Metoda);
            PrimeniPromene_Command = new RelayCommand(PrimeniPromene_Metoda);
            PronadjiGosta_Command = new RelayCommand(PronadjiGosta_Metoda);

            this.Rezervacija = rezervacija;
            this.canvasStolovi = canvasStolovi;
            this.prikazanSto = Broker.BrokerSelect.dajSesiju().VratiSto(rezervacija.Sema, rezervacija.Sto);
            this.sviStolovi = Broker.BrokerSelect.dajSesiju().vratiStolove(rezervacija.Sema);

            Naslov = "Rezervacija - " + rezervacija.Id;
            Gost = rezervacija.ImeGosta + " " + rezervacija.PrezimeGosta;
            Telefon = rezervacija.BrojTelefona;
            Datum = rezervacija.Datum.ToString("dd.MM.yyyy");
            VremeOd = rezervacija.VremeOd.ToString("HH:mm");
            VremeDo = rezervacija.VremeDo.ToString("HH:mm");
            Sto = rezervacija.Sto;
            BrojOdraslih = rezervacija.BrojOdraslih.ToString();
            BrojDece = rezervacija.BrojDece.ToString();
            Napomena = rezervacija.Napomena;

            foreach(re_Sto sto in sviStolovi)
            {
                
                Border brd = new Border();
                brd.MouseLeftButtonDown += Brd_MouseLeftButtonDown;
                brd.CornerRadius = new System.Windows.CornerRadius(3);
                if(sto.Oblik != 0)
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
                if (prikazanSto.Sto != sto.Sto)
                {
                    brd.Opacity = 0.2;
                } else
                {
                    brd.Opacity = 1;
                }
                brd.Width = sto.Sirina/ 13;
                brd.Height = sto.Visina/ 13;
                canvasStolovi.Children.Add(brd);
                Canvas.SetLeft(brd, sto.PozicijaLeft/ 19 - 100);
                Canvas.SetTop(brd, sto.PozicijaTop/ 19);
            }
        }

        private void Brd_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Border brd = sender as Border;
            if (brd != null)
            {


                TextBlock tb = brd.Child as TextBlock;
                ;
                DateTime vremeOd = DateTime.Now;
                DateTime vremeDo = DateTime.Now;
                DateTime.TryParse(VremeOd, out vremeOd);
                DateTime.TryParse(VremeDo, out vremeDo);
                ObservableCollection<re_Rezervacija> poklapanja = Broker.BrokerSelect.dajSesiju().VratiRezervacijeKojeSePoklapajuSaVremenom_ZaSto(vremeOd, vremeDo, tb.Text);
                bool poklapanje = false;
                if(poklapanja.Any(x => x.Id == Rezervacija.Id))
                {
                    poklapanja.Remove(poklapanja.First(x => x.Id == Rezervacija.Id));
                }
                if (poklapanja.Count > 0)
                {
                    poklapanje = true;
                }
                if (poklapanje)
                {
                    v_MessageBox prozor = new v_MessageBox("Za ovaj sto već postoji rezervacija u datom vremenu. Želite li da nastavite?");
                    prozor.ShowDialog();
                    if (!Globalno.Varijable.sacuvanePromene)
                    {
                        return;
                    }
                }
                foreach (Border brdsto in canvasStolovi.Children)
                {
                    brdsto.Opacity = 0.2;
                }
                brd.Opacity = 1;
                Sto = tb.Text;
            }
        }

        private void PronadjiGosta_Metoda(object obj)
        {
            string filtertext = "";
            if (!string.IsNullOrEmpty(Telefon))
            {
                filtertext = Telefon;
            }
            if (!string.IsNullOrEmpty(Gost))
            {
                filtertext = Gost;
            }
            v_PronalazakGosta prozor = new v_PronalazakGosta(filtertext);
            prozor.ShowDialog();
            if(Globalno.Varijable.IzabranGost != null)
            {
                Rezervacija.IdGost = Globalno.Varijable.IzabranGost.ID;
                Rezervacija.BrojTelefona = Globalno.Varijable.IzabranGost.Telefon;
                Gost = Globalno.Varijable.IzabranGost.Ime + " " + Globalno.Varijable.IzabranGost.Prezime;
                Telefon = Globalno.Varijable.IzabranGost.Telefon;
            }
            return;
        }

        private void PrimeniPromene_Metoda(object obj)
        {
            DateTime vreme = DateTime.Now;
            DateTime.TryParse(Datum, out vreme);
            Rezervacija.Datum = vreme;
            DateTime.TryParse(VremeOd, out vreme);
            Rezervacija.VremeOd = vreme;
            DateTime.TryParse(VremeDo, out vreme);
            Rezervacija.VremeDo = vreme;
            Rezervacija.Sto = Sto;
            Rezervacija.BrojOdraslih = Convert.ToInt32(BrojOdraslih);
            Rezervacija.BrojDece = Convert.ToInt32(BrojDece);
            Rezervacija.Napomena = Napomena;
            bool test = Broker.BrokerUpdate.dajSesiju().UpdateRezervaciju(Rezervacija);
            if (test)
            {

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
