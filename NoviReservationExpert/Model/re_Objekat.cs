using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoviReservationExpert.Model
{
    public class re_Objekat : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        int _mag;
        string _naziv;
        int _indMag;
        string _korisnik;
        int _zakljucano;
        int _mar;
        string _mestoObj;
        string _adresaObj;

        public int Objekat
        {
            get
            {
                return _mag;
            }
            set
            {
                _mag = value;
                NotifyPropertyChanged(nameof(Objekat));
            }
        }

        public string Naziv
        {
            get
            {
                return _naziv;
            }
            set
            {
                _naziv = value;
                NotifyPropertyChanged(nameof(Naziv));
            }
        }

        public int IndMag
        {
            get
            {
                return _indMag;
            }
            set
            {
                _indMag = value;
                NotifyPropertyChanged(nameof(IndMag));
            }
        }

        public string Korisnik
        {
            get
            {
                return _korisnik;
            }
            set
            {
                _korisnik = value;
                NotifyPropertyChanged(nameof(Korisnik));
            }
        }

        public int Zakljucano
        {
            get
            {
                return _zakljucano;
            }
            set
            {
                _zakljucano = value;
                NotifyPropertyChanged(nameof(Zakljucano));
            }
        }

        public int Mar
        {
            get
            {
                return _mar;
            }
            set
            {
                _mar = value;
                NotifyPropertyChanged(nameof(Mar));
            }
        }

        public string Mesto
        {
            get
            {
                return _mestoObj;
            }
            set
            {
                _mestoObj = value;
                NotifyPropertyChanged(nameof(Mesto));
            }
        }

        public string Adresa
        {
            get
            {
                return _adresaObj;
            }
            set
            {
                _adresaObj = value;
                NotifyPropertyChanged(nameof(Adresa));
            }
        }
    }
}
