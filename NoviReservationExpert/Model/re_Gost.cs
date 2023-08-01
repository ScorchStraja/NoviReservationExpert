using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoviReservationExpert.Model
{
    public class re_Gost : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        string _idgst;
        string _nazivint;
        string _nazivext;
        int _sifkd;
        int _aktivna;
        string _tel;
        string _email;
        string _ime;
        string _prezime;
        string _grad;

        public string ID
        {
            get 
            { 
                return _idgst; 
            }
            set 
            { 
                _idgst = value;
                NotifyPropertyChanged(nameof(ID));
            }
        }
        public string NazivInt
        {
            get
            {
                return _nazivint;
            }
            set
            {
                _nazivint = value;
                NotifyPropertyChanged(nameof(NazivInt));
            }
        }
        public string NazivExt
        {
            get
            {
                return _nazivext;
            }
            set
            {
                _nazivext = value;
                NotifyPropertyChanged(nameof(NazivExt));
            }
        }
        public int SifKD
        {
            get
            {
                return _sifkd;
            }
            set
            {
                _sifkd = value;
                NotifyPropertyChanged(nameof(SifKD));
            }
        }
        public int Aktivna
        {
            get
            {
                return _aktivna;
            }
            set
            {
                _aktivna = value;
                NotifyPropertyChanged(nameof(Aktivna));
            }
        }
        public string Telefon
        {
            get
            {
                return _tel;
            }
            set
            {
                _tel = value;
                NotifyPropertyChanged(nameof(Telefon));
            }
        }
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                NotifyPropertyChanged(nameof(Email));
            }
        }
        public string Ime
        {
            get
            {
                return _ime;
            }
            set
            {
                _ime = value;
                NotifyPropertyChanged(nameof(Ime));
            }
        }
        public string Prezime
        {
            get
            {
                return _prezime;
            }
            set
            {
                _prezime = value;
                NotifyPropertyChanged(nameof(Prezime));
            }
        }
        public string Grad
        {
            get
            {
                return _grad;
            }
            set
            {
                _grad = value;
                NotifyPropertyChanged(nameof(Grad));
            }
        }

    }
}
