using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoviReservationExpert.Model
{
    public class re_Log : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        int _tip;
        int _korisnikid;
        DateTime _datum;
        DateTime _vreme;
        string? _opis;
        string? _modul;
        int _brerror;
        string? _tipapp;
        string? _logon_masime;
        int _id;
        int _tipizm;
        string? _operacija;
        string? _imeforme;
        int _tipobj;

        public int Tip
        {
            get
            {
                return _tip;
            }
            set
            {
                _tip = value;
                NotifyPropertyChanged(nameof(Tip));
            }
        }
        public int KorisnikId
        {
            get
            {
                return _korisnikid;
            }
            set
            {
                _korisnikid = value;
                NotifyPropertyChanged(nameof(KorisnikId));
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
        public DateTime Vreme
        {
            get
            {
                return _vreme;
            }
            set
            {
                _vreme = value;
                NotifyPropertyChanged(nameof(Vreme));
            }
        }
        public string Opis
        {
            get
            {
                return _opis ?? "-";
            }
            set
            {
                _opis = value;
                NotifyPropertyChanged(nameof(Opis));
            }
        }
        public string Modul
        {
            get
            {
                return _modul ?? "-";
            }
            set
            {
                _modul = value;
                NotifyPropertyChanged(nameof(Modul));
            }
        }
        public int BrError
        {
            get
            {
                return _brerror;
            }
            set
            {
                _brerror = value;
                NotifyPropertyChanged(nameof(BrError));
            }
        }
        public string TipApp
        {
            get
            {
                return _tipapp ?? "-";
            }
            set
            {
                _tipapp = value;
                NotifyPropertyChanged(nameof(TipApp));
            }
        }
        public string LogOnMasinskoIme
        {
            get
            {
                return _logon_masime ?? "-";
            }
            set
            {
                _logon_masime = value;
                NotifyPropertyChanged(nameof(LogOnMasinskoIme));
            }
        }
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
        public int TipIzmene
        {
            get
            {
                return _tipizm;
            }
            set
            {
                _tipizm = value;
                NotifyPropertyChanged(nameof(TipIzmene));
            }
        }
        public string Operacija
        {
            get
            {
                return _operacija ?? "-";
            }
            set
            {
                _operacija = value;
                NotifyPropertyChanged(nameof(Operacija));
            }
        }
        public string ImeForme
        {
            get
            {
                return _imeforme ?? "-";
            }
            set
            {
                _imeforme = value;
                NotifyPropertyChanged(nameof(ImeForme));
            }
        }
        public int TipObj
        {
            get
            {
                return _tipobj;
            }
            set
            {
                _tipobj = value;
                NotifyPropertyChanged(nameof(TipObj));
            }
        }
    }
}
