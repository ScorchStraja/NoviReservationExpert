using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoviReservationExpert.Model
{
    public class re_Korisnik : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        int _korisnikId;
        int _korisnikIdGrupa;
        string? _korisnikSid;
        string? _korisnikLogOnIme;
        string? _korisnikIme;
        string? _korisnikPrezime;

        public int Id
        {
            get
            {
                return _korisnikId;
            }
            set
            {
                _korisnikId = value;
                NotifyPropertyChanged(nameof(Id));
            }
        }

        public int IdGrupa
        {
            get
            {
                return _korisnikIdGrupa;
            }
            set
            {
                _korisnikIdGrupa = value;
                NotifyPropertyChanged(nameof(IdGrupa));
            }
        }
        public string SID
        {
            get
            {
                return _korisnikSid ?? "-";
            }
            set
            {
                _korisnikSid = value;
                NotifyPropertyChanged(nameof(SID));
            }
        }
        public string LogOnIme
        {
            get
            {
                return _korisnikLogOnIme ?? "-";
            }
            set
            {
                _korisnikLogOnIme = value;
                NotifyPropertyChanged(nameof(LogOnIme));
            }
        }
        public string Ime
        {
            get
            {
                return _korisnikIme ?? "-";
            }
            set
            {
                _korisnikIme = value;
                NotifyPropertyChanged(nameof(Ime));
            }
        }
        public string Prezime
        {
            get
            {
                return _korisnikPrezime ?? "-";
            }
            set
            {
                _korisnikPrezime = value;
                NotifyPropertyChanged(nameof(Prezime));
            }
        }
    }
}
