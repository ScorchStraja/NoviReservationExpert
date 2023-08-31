using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoviReservationExpert.Model
{
    public class re_Rezervacija : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        int _id;
        DateTime _datum;
        DateTime _vremeod;
        DateTime _vremedo;
        int _obj;
        string? _sema;
        int _grupa;
        string? _napomena;
        string? _kuhinja;
        string? _idgost;
        int _reon;
        string _sto;
        int _status;
        string _ststatus;
        int _brodraslih;
        int _brdece;
        int _korisnik;
        int _obrisano;
        int _preporuka;
        int _tripadvisor;
        int _mreze;
        int _drugo;
        int _ljubimac;
        int _malodete;
        int _notifikacija;
        int _automail;
        int _walkin;
        string? _imegosta;
        string? _prezimegosta;
        string? _brojtelefona;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                NotifyPropertyChanged(nameof(Id));
            }
        }
        public DateTime Datum
        {
            get
            {
                return _datum;
            }
            set
            {
                _datum = value;
                NotifyPropertyChanged(nameof(Datum));
            }
        }
        public DateTime VremeOd
        {
            get
            {
                return _vremeod;
            }
            set
            {
                _vremeod = value;
                NotifyPropertyChanged(nameof(VremeOd));
            }
        }
        public DateTime VremeDo
        {
            get
            {
                return _vremedo;
            }
            set
            {
                _vremedo = value;
                NotifyPropertyChanged(nameof(VremeDo));
            }
        }

        public int Objekat
        {
            get
            {
                return _obj;
            }
            set
            {
                _obj = value;
                NotifyPropertyChanged(nameof(Objekat));
            }
        }

        public string Sema
        {
            get
            {
                return _sema;
            }
            set
            { 
                _sema = value;
                NotifyPropertyChanged(nameof(Sema));
            }
        }
        public int Grupa
        {
            get
            {
                return _grupa;
            }
            set
            {
                _grupa = value;
                NotifyPropertyChanged(nameof(Grupa));
            }
        }
        public string? Napomena
        {
            get
            {
                return _napomena ?? "-";
            }
            set
            {
                _napomena = value;
                NotifyPropertyChanged(nameof(Napomena));
            }
        }
        public string? Kuhinja
        {
            get
            {
                return _kuhinja ?? "-";
            }
            set
            {
                _kuhinja = value;
                NotifyPropertyChanged(nameof(Kuhinja));
            }
        }
        public string? IdGost
        {
            get
            {
                return _idgost ?? "-";
            }
            set
            {
                _idgost = value;
                NotifyPropertyChanged(nameof(IdGost));
            }
        }
        public int Reon
        {
            get
            {
                return _reon;
            }
            set
            {
                _reon = value;
                NotifyPropertyChanged(nameof(Reon));
            }
        }
        public string? Sto
        {
            get
            {
                return _sto ?? "-";
            }
            set
            {
                _sto = value;
                NotifyPropertyChanged(nameof(Sto));
            }
        }
        public int Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                if(_status == -1)
                {
                    StStatus = "Deaktivirana";
                }
                if (_status == 0)
                {
                    StStatus = "Neaktivirana";
                }
                if (_status == 1)
                {
                    StStatus = "Aktivirana";
                }
                NotifyPropertyChanged(nameof(Status));
            }
        }
        public string StStatus
        {
            get
            {
                return _ststatus;
            }
            set
            {
                _ststatus = value; 
                NotifyPropertyChanged(nameof(StStatus));
            }
        }
        public int BrojOdraslih
        {
            get
            {
                return _brodraslih;
            }
            set
            {
                _brodraslih = value;
                NotifyPropertyChanged(nameof(BrojOdraslih));
            }
        }
        public int BrojDece
        {
            get
            {
                return _brdece;
            }
            set
            {
                _brdece = value;
                NotifyPropertyChanged(nameof(BrojDece));
            }
        }
        public int Korisnik
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
        public int Obrisano
        {
            get
            {
                return _obrisano;
            }
            set
            {
                _obrisano = value;
                NotifyPropertyChanged(nameof(Obrisano));
            }
        }
        public int Preporuka
        {
            get
            {
                return _preporuka;
            }
            set
            {
                _preporuka = value;
                NotifyPropertyChanged(nameof(Preporuka));
            }
        }
        public int TripAdvisor
        {
            get
            {
                return _tripadvisor;
            }
            set
            {
                _tripadvisor = value;
                NotifyPropertyChanged(nameof(TripAdvisor));
            }
        }
        public int Mreze
        {
            get
            {
                return _mreze;
            }
            set
            {
                _mreze = value;
                NotifyPropertyChanged(nameof(Mreze));
            }
        }
        public int Drugo
        {
            get
            {
                return _drugo;
            }
            set
            {
                _drugo = value;
                NotifyPropertyChanged(nameof(Drugo));
            }
        }
        public int Ljubimac
        {
            get
            {
                return _ljubimac;
            }
            set
            {
                _ljubimac = value;
                NotifyPropertyChanged(nameof(Ljubimac));
            }
        }
        public int MaloDete
        {
            get
            {
                return _malodete;
            }
            set
            {
                _malodete = value;
                NotifyPropertyChanged(nameof(MaloDete));
            }
        }
        public int Notifikacija
        {
            get
            {
                return _notifikacija;
            }
            set
            {
                _notifikacija = value;
                NotifyPropertyChanged(nameof(Notifikacija));
            }
        }
        public int AutoMail
        {
            get
            {
                return _automail;
            }
            set
            {
                _automail = value;
                NotifyPropertyChanged(nameof(AutoMail));
            }
        }
        public int WalkIn
        {
            get
            {
                return _walkin;
            }
            set
            {
                _walkin = value;
                NotifyPropertyChanged(nameof(WalkIn));
            }
        }
        public string? ImeGosta
        {
            get
            {
                return _imegosta ?? "-";
            }
            set
            {
                _imegosta = value;
                NotifyPropertyChanged(nameof(ImeGosta));
            }
        }
        public string? PrezimeGosta
        {
            get
            {
                return _prezimegosta ?? "-";
            }
            set
            {
                _prezimegosta = value;
                NotifyPropertyChanged(nameof(PrezimeGosta));
            }
        }
        public string? BrojTelefona
        {
            get
            {
                return _brojtelefona ?? "-";
            }
            set
            {
                _brojtelefona = value;
                NotifyPropertyChanged(nameof(BrojTelefona));
            }
        }

    }
}
