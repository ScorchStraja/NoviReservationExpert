using NoviReservationExpert.Model;
using NoviReservationExpert.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
        public RelayCommand PrikaziOtkazane_Command { get; private set; }
        public RelayCommand PrikaziZavrsene_Command { get; private set; }
        public RelayCommand Pretraga_Command { get; private set; }
        public RelayCommand OtvoriDetaljeRezervacije_Command { get; private set; }
        public RelayCommand PrikaziUToku_Command { get; private set; }
        public RelayCommand PrikaziRezervisane_Command { get; private set; }
        public RelayCommand Detalji_Command { get; set; }
        public RelayCommand UkloniPretraga_Command { get; set; }
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
        DateTime _izabranDatumOD;
        public DateTime izabranDatumOD
        {
            get
            {
                return _izabranDatumOD;
            }
            set
            {
                _izabranDatumOD = value;
                AzurirajSQLListu();
                NotifyPropertyChanged(nameof(izabranDatumDO));
            }
        }

        DateTime _izabranDatumDO;
        public DateTime izabranDatumDO
        {
            get
            {
                return _izabranDatumDO;
            }
            set
            {
                _izabranDatumDO = value;
                AzurirajSQLListu();
                NotifyPropertyChanged(nameof(izabranDatumDO));
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
        DockPanel dpDugmici;
        public vm_PregledRezervacija(DockPanel dpDugmici)
        {
            Zatvori_Command = new RelayCommand(Zatvori_Metoda);
            PrikaziSve_Command = new RelayCommand(PrikaziSve_Metoda);
            PrikaziOtkazane_Command = new RelayCommand(PrikaziOtkazane_Metoda);
            PrikaziZavrsene_Command = new RelayCommand(PrikaziZavrsene_Metoda);
            Pretraga_Command = new RelayCommand(Pretraga_Metoda);
            OtvoriDetaljeRezervacije_Command = new RelayCommand(OtvoriDetaljeRezervacije_Metoda);
            PrikaziUToku_Command = new RelayCommand(PrikaziUToku_Metoda);
            PrikaziRezervisane_Command = new RelayCommand(PrikaziRezervisane_Metoda);
            Detalji_Command = new RelayCommand(Detalji_Metoda);
            UkloniPretraga_Command = new RelayCommand(UkloniPretraga_Metoda);

            this.dpDugmici = dpDugmici;

            _izabranDatumOD = DateTime.Today;
            _izabranDatumDO = DateTime.Today;

            Pretraga = "";

            SQLListaRezervacija = new ObservableCollection<re_Rezervacija>();
            SQLListaRezervacija = Broker.BrokerSelect.dajSesiju().VratiRezervacijeOdDoDatuma(izabranDatumOD, izabranDatumDO, Globalno.Varijable.Objekat.Objekat, Globalno.Varijable.IzabranaSema.Sema);
            PrikaziUToku_Metoda(null);
        }

        public void UkloniPretraga_Metoda(object obj)
        {
            Pretraga = "";
            PodesiIzgled("btnSve");
            PrikaziSve_Metoda(null);
        }

        private void PrikaziUToku_Metoda(object obj)
        {
            SQLListaRezervacija = Broker.BrokerSelect.dajSesiju().VratiRezervacijeOdDoDatuma(izabranDatumOD, izabranDatumDO, Globalno.Varijable.Objekat.Objekat, Globalno.Varijable.IzabranaSema.Sema);
            ObservableCollection<re_Rezervacija> utoku = Broker.BrokerSelect.dajSesiju().VratiRezervacijeOdDoDatuma(izabranDatumOD, izabranDatumDO, Globalno.Varijable.Objekat.Objekat, Globalno.Varijable.IzabranaSema.Sema);
            for (int i = utoku.Count() - 1; i >= 0; i--)
            {
                re_Rezervacija rez = utoku[i];
                if (rez.Datum != DateTime.Today)
                {
                    utoku.RemoveAt(i);
                    continue;
                }
                if (rez.Status != 1)
                {
                    utoku.RemoveAt(i);
                }
            }
            if (Pretraga == "")
            {
                listaRezervacija = new ObservableCollection<re_Rezervacija>(utoku);
            }
            else
            {
                Pretraga = Pretraga.ToLower();
                ObservableCollection<re_Rezervacija> ime = new ObservableCollection<re_Rezervacija>(utoku.Where(x => x.ImeGosta.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> prezime = new ObservableCollection<re_Rezervacija>(utoku.Where(x => x.PrezimeGosta.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> broj = new ObservableCollection<re_Rezervacija>(utoku.Where(x => x.BrojTelefona.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> sto = new ObservableCollection<re_Rezervacija>(utoku.Where(x => x.Sto.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> ime_prezime = new ObservableCollection<re_Rezervacija>(ime.Union(prezime));
                ObservableCollection<re_Rezervacija> ime_prezime_broj = new ObservableCollection<re_Rezervacija>(ime_prezime.Union(broj));
                ObservableCollection<re_Rezervacija> ime_prezime_broj_sto = new ObservableCollection<re_Rezervacija>(ime_prezime_broj.Union(sto));
                listaRezervacija = ime_prezime_broj_sto;
            }                  
        }
        private void AzurirajSQLListu()
        {   
            PodesiIzgled("btnSve");
            PrikaziSve_Metoda(null);
        }
        private void PodesiIzgled(string filter)
        {
            foreach (Button button in dpDugmici.Children.OfType<Button>())
            {
                button.Background = (Brush)Application.Current.FindResource("Plava");
                
                if (button.Name == filter)
                {
                    button.Background = (Brush)Application.Current.FindResource("Plava_SV");
                }
            }
        }
        private void Detalji_Metoda(object obj)
        {
            OtvoriDetaljeRezervacije_Metoda(obj);
        }
        private void PrikaziRezervisane_Metoda(object obj)
        {
            SQLListaRezervacija = Broker.BrokerSelect.dajSesiju().VratiRezervacijeOdDoDatuma(izabranDatumOD, izabranDatumDO, Globalno.Varijable.Objekat.Objekat, Globalno.Varijable.IzabranaSema.Sema);
            if (Pretraga == "")
            {
                listaRezervacija = new ObservableCollection<re_Rezervacija>(SQLListaRezervacija.Where(x => x.Status == 0));

            }
            else
            {
                ObservableCollection<re_Rezervacija> rezervisane = new ObservableCollection<re_Rezervacija>(SQLListaRezervacija.Where(x => x.Status == 0));

                ObservableCollection<re_Rezervacija> ime = new ObservableCollection<re_Rezervacija>(rezervisane.Where(x => x.ImeGosta.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> prezime = new ObservableCollection<re_Rezervacija>(rezervisane.Where(x => x.PrezimeGosta.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> broj = new ObservableCollection<re_Rezervacija>(rezervisane.Where(x => x.BrojTelefona.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> sto = new ObservableCollection<re_Rezervacija>(rezervisane.Where(x => x.Sto.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> ime_prezime = new ObservableCollection<re_Rezervacija>(ime.Union(prezime));
                ObservableCollection<re_Rezervacija> ime_prezime_broj = new ObservableCollection<re_Rezervacija>(ime_prezime.Union(broj));
                ObservableCollection<re_Rezervacija> ime_prezime_broj_sto = new ObservableCollection<re_Rezervacija>(ime_prezime_broj.Union(sto));
                listaRezervacija = ime_prezime_broj;
            }
        }
        private void PrikaziOtkazane_Metoda(object obj)
        {
            SQLListaRezervacija = Broker.BrokerSelect.dajSesiju().VratiRezervacijeOdDoDatuma(izabranDatumOD, izabranDatumDO, Globalno.Varijable.Objekat.Objekat, Globalno.Varijable.IzabranaSema.Sema);
            if (Pretraga == "")
            {
                listaRezervacija = new ObservableCollection<re_Rezervacija>(SQLListaRezervacija.Where(x => x.Status == -1));

            }
            else
            {
                ObservableCollection<re_Rezervacija> otkazane = new ObservableCollection<re_Rezervacija>(SQLListaRezervacija.Where(x => x.Status == -1));

                ObservableCollection<re_Rezervacija> ime = new ObservableCollection<re_Rezervacija>(otkazane.Where(x => x.ImeGosta.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> prezime = new ObservableCollection<re_Rezervacija>(otkazane.Where(x => x.PrezimeGosta.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> broj = new ObservableCollection<re_Rezervacija>(otkazane.Where(x => x.BrojTelefona.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> sto = new ObservableCollection<re_Rezervacija>(otkazane.Where(x => x.Sto.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> ime_prezime = new ObservableCollection<re_Rezervacija>(ime.Union(prezime));
                ObservableCollection<re_Rezervacija> ime_prezime_broj = new ObservableCollection<re_Rezervacija>(ime_prezime.Union(broj));
                ObservableCollection<re_Rezervacija> ime_prezime_broj_sto = new ObservableCollection<re_Rezervacija>(ime_prezime_broj.Union(sto));
                listaRezervacija = ime_prezime_broj_sto;
            }
        }
        private void PrikaziZavrsene_Metoda(object obj)
        {
            SQLListaRezervacija = Broker.BrokerSelect.dajSesiju().VratiRezervacijeOdDoDatuma(izabranDatumOD, izabranDatumDO, Globalno.Varijable.Objekat.Objekat, Globalno.Varijable.IzabranaSema.Sema);
            if (Pretraga == "")
            {
                listaRezervacija = new ObservableCollection<re_Rezervacija>(SQLListaRezervacija.Where(x => x.Status == 2));

            }
            else
            {
                ObservableCollection<re_Rezervacija> zavrene = new ObservableCollection<re_Rezervacija>(SQLListaRezervacija.Where(x => x.Status == 2));

                ObservableCollection<re_Rezervacija> ime = new ObservableCollection<re_Rezervacija>(zavrene.Where(x => x.ImeGosta.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> prezime = new ObservableCollection<re_Rezervacija>(zavrene.Where(x => x.PrezimeGosta.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> broj = new ObservableCollection<re_Rezervacija>(zavrene.Where(x => x.BrojTelefona.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> sto = new ObservableCollection<re_Rezervacija>(zavrene.Where(x => x.Sto.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> ime_prezime = new ObservableCollection<re_Rezervacija>(ime.Union(prezime));
                ObservableCollection<re_Rezervacija> ime_prezime_broj = new ObservableCollection<re_Rezervacija>(ime_prezime.Union(broj));
                ObservableCollection<re_Rezervacija> ime_prezime_broj_sto = new ObservableCollection<re_Rezervacija>(ime_prezime_broj.Union(sto));
                listaRezervacija = ime_prezime_broj_sto;
            }
        }
        private void OtvoriDetaljeRezervacije_Metoda(object obj)
        {
            if (izabranaRezervacija == null)
            {
                v_MessageBox warprozor = new v_MessageBox("Nije izabrana nijedna rezervacija.");
                warprozor.ShowDialog();
                return;
            }
            v_DetaljiRezervacije prozor = new v_DetaljiRezervacije(izabranaRezervacija);
            prozor.ShowDialog();
        }
        public void Pretraga_Metoda(object obj)
        {
            SQLListaRezervacija = Broker.BrokerSelect.dajSesiju().VratiRezervacijeOdDoDatuma(izabranDatumOD, izabranDatumDO, Globalno.Varijable.Objekat.Objekat, Globalno.Varijable.IzabranaSema.Sema);
            if (!string.IsNullOrEmpty(Pretraga)) 
            {
                Pretraga = Pretraga.ToLower();
                ObservableCollection<re_Rezervacija> ime = new ObservableCollection<re_Rezervacija>(SQLListaRezervacija.Where(x => x.ImeGosta.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> prezime = new ObservableCollection<re_Rezervacija>(SQLListaRezervacija.Where(x => x.PrezimeGosta.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> broj = new ObservableCollection<re_Rezervacija>(SQLListaRezervacija.Where(x => x.BrojTelefona.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> sto = new ObservableCollection<re_Rezervacija>(SQLListaRezervacija.Where(x => x.Sto.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> ime_prezime = new ObservableCollection<re_Rezervacija>(ime.Union(prezime));
                ObservableCollection<re_Rezervacija> ime_prezime_broj = new ObservableCollection<re_Rezervacija>(ime_prezime.Union(broj));
                ObservableCollection<re_Rezervacija> ime_prezime_broj_sto = new ObservableCollection<re_Rezervacija>(ime_prezime_broj.Union(sto));
                listaRezervacija = ime_prezime_broj_sto;
            }
            PodesiIzgled("btnSve");
        }
        private void PrikaziSve_Metoda(object obj)
        {
            SQLListaRezervacija = Broker.BrokerSelect.dajSesiju().VratiRezervacijeOdDoDatuma(izabranDatumOD, izabranDatumDO, Globalno.Varijable.Objekat.Objekat, Globalno.Varijable.IzabranaSema.Sema);
            if (Pretraga == "")
            {
                listaRezervacija = SQLListaRezervacija;
            } else
            {
                Pretraga = Pretraga.ToLower();
                ObservableCollection<re_Rezervacija> ime = new ObservableCollection<re_Rezervacija>(SQLListaRezervacija.Where(x => x.ImeGosta.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> prezime = new ObservableCollection<re_Rezervacija>(SQLListaRezervacija.Where(x => x.PrezimeGosta.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> broj = new ObservableCollection<re_Rezervacija>(SQLListaRezervacija.Where(x => x.BrojTelefona.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> sto = new ObservableCollection<re_Rezervacija>(SQLListaRezervacija.Where(x => x.Sto.ToLower().Contains(Pretraga)));
                ObservableCollection<re_Rezervacija> ime_prezime = new ObservableCollection<re_Rezervacija>(ime.Union(prezime));
                ObservableCollection<re_Rezervacija> ime_prezime_broj = new ObservableCollection<re_Rezervacija>(ime_prezime.Union(broj));
                ObservableCollection<re_Rezervacija> ime_prezime_broj_sto = new ObservableCollection<re_Rezervacija>(ime_prezime_broj.Union(sto));
                listaRezervacija = ime_prezime_broj_sto;
            }       
        }
        private void Zatvori_Metoda(object obj)
        {
            this.ZatvoriFormu();
        }
    }
}
