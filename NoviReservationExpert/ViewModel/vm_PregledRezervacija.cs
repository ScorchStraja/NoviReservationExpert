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

namespace NoviReservationExpert.ViewModel
{
    public class vm_PregledRezervacija : INotifyPropertyChanged
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
        public RelayCommand PrikaziSve_Command { get; private set; }
        public RelayCommand PrikaziSamoOtkazane_Command { get; private set; }
        public RelayCommand PrikaziZavrsene_Command { get; private set; }
        public RelayCommand Pretraga_Command { get; private set; }
        public RelayCommand OtvoriDetaljeRezervacije_Command { get; private set; }
        public RelayCommand PrikaziTrenutneRezervacija_Command { get; private set; }
        #endregion

        #region PROPERTIJI
        ObservableCollection<re_Rezervacija> SQLListaRezervacija;
        ObservableCollection<re_Rezervacija> _listaRezervacija;
        public ObservableCollection<re_Rezervacija> listaRezervacija
        {
            get
            {
                return _listaRezervacija;
            }
            set
            {
                _listaRezervacija = value;
                NotifyPropertyChanged(nameof(listaRezervacija));
            }
        }
        re_Rezervacija _izabranaRezervacija;
        public re_Rezervacija izabranaRezervacija
        {
            get
            {
                return _izabranaRezervacija;
            }
            set
            {
                _izabranaRezervacija = value;
                NotifyPropertyChanged(nameof(izabranaRezervacija));
            }
        }
        DateTime _izabranDatum;
        public DateTime izabranDatum
        {
            get
            {
                return _izabranDatum;
            }
            set
            {
                _izabranDatum = value;
                Pretraga = "";
                SQLListaRezervacija = Broker.BrokerSelect.dajSesiju().VratiRezervacijeZaDatum(_izabranDatum, Globalno.Varijable.Objekat.Objekat, Globalno.Varijable.IzabranaSema.Sema);
                listaRezervacija = SQLListaRezervacija;
                if(statusprikaza == 1)
                {
                    listaRezervacija = new ObservableCollection<re_Rezervacija>(SQLListaRezervacija.Where(x => x.Status == 1));
                }
                if(statusprikaza == -1)
                {
                    listaRezervacija = new ObservableCollection<re_Rezervacija>(SQLListaRezervacija.Where(x => x.Status == -1));
                }
                NotifyPropertyChanged(nameof(izabranDatum));
            }
        }
        string _Pretraga;
        public string Pretraga
        {
            get
            {
                return _Pretraga;
            }
            set
            {
                _Pretraga = value;
                NotifyPropertyChanged(nameof(Pretraga));
            }
        }
        #endregion
        int statusprikaza = 0;
        public vm_PregledRezervacija()
        {
            Zatvori_Command = new RelayCommand(Zatvori_Metoda);
            PrikaziSve_Command = new RelayCommand(PrikaziSve_Metoda);
            PrikaziSamoOtkazane_Command = new RelayCommand(PrikaziSamoOtkazane_Metoda);
            PrikaziZavrsene_Command = new RelayCommand(PrikaziZavrsene_Metoda);
            Pretraga_Command = new RelayCommand(Pretraga_Metoda);
            OtvoriDetaljeRezervacije_Command = new RelayCommand(OtvoriDetaljeRezervacije_Metoda);
            PrikaziTrenutneRezervacija_Command = new RelayCommand(PrikaziTrenutneRezervacija_Metoda);

            SQLListaRezervacija = new ObservableCollection<re_Rezervacija>();
            SQLListaRezervacija = Broker.BrokerSelect.dajSesiju().VratiRezervacijeZaDatum(DateTime.Today, Globalno.Varijable.Objekat.Objekat, Globalno.Varijable.IzabranaSema.Sema);
            listaRezervacija = SQLListaRezervacija;
        }

        private void PrikaziTrenutneRezervacija_Metoda(object obj)
        {
            ObservableCollection<re_Rezervacija> trenutnerezervacije = Broker.BrokerSelect.dajSesiju().VratiRezervacijeZaDatum(DateTime.Today, Globalno.Varijable.Objekat.Objekat, Globalno.Varijable.IzabranaSema.Sema);
            listaRezervacija = new ObservableCollection<re_Rezervacija>(trenutnerezervacije.Where(x => x.Status == 0));
        }

        private void OtvoriDetaljeRezervacije_Metoda(object obj)
        {
            if (izabranaRezervacija == null) return;
            v_DetaljiRezervacije prozor = new v_DetaljiRezervacije(izabranaRezervacija);
            prozor.ShowDialog();
        }

        private void Pretraga_Metoda(object obj)
        {
            if(!string.IsNullOrEmpty(Pretraga)) 
            {
                Pretraga = Pretraga.ToLower();
                ObservableCollection<re_Rezervacija> ime = new ObservableCollection<re_Rezervacija>(SQLListaRezervacija.Where(x => x.ImeGosta.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> prezime = new ObservableCollection<re_Rezervacija>(SQLListaRezervacija.Where(x => x.PrezimeGosta.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> broj = new ObservableCollection<re_Rezervacija>(SQLListaRezervacija.Where(x => x.BrojTelefona.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> ime_prezime = new ObservableCollection<re_Rezervacija>(ime.Union(prezime));
                ObservableCollection<re_Rezervacija> ime_prezime_broj = new ObservableCollection<re_Rezervacija>(ime_prezime.Union(broj));
                listaRezervacija = ime_prezime_broj;
            }
            else
            {
                listaRezervacija = SQLListaRezervacija;
            }
            if (statusprikaza == 1)
            {
                listaRezervacija = new ObservableCollection<re_Rezervacija>(listaRezervacija.Where(x => x.Status == 1));
            }
            if (statusprikaza == -1)
            {
                listaRezervacija = new ObservableCollection<re_Rezervacija>(listaRezervacija.Where(x => x.Status == -1));
            }
        }

        private void PrikaziZavrsene_Metoda(object obj)
        {
            statusprikaza = 1;
            Pretraga = "";
            listaRezervacija = new ObservableCollection<re_Rezervacija>(SQLListaRezervacija.Where(x => x.Status == 1));
        }

        private void PrikaziSamoOtkazane_Metoda(object obj)
        {
            statusprikaza = -1;
            Pretraga = "";
            listaRezervacija = new ObservableCollection<re_Rezervacija>(SQLListaRezervacija.Where(x => x.Status == -1));
        }

        private void PrikaziSve_Metoda(object obj)
        {
            statusprikaza = 0;
            Pretraga = "";
            listaRezervacija = SQLListaRezervacija;
        }

        private void Zatvori_Metoda(object obj)
        {
            this.ZatvoriFormu();
        }
    }
}
