using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NoviReservationExpert.Model
{
    public class re_Sto : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public override string ToString()
        {
            return Sto;
        }

        int _obj;
        string _sema;
        string _sto;
        int _brosoba;
        int _ptop;
        int _pleft;
        int _width;
        int _height;
        int _oblik;

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
        public string Sto
        {
            get
            {
                return _sto;
            }
            set
            {
                _sto = value;
                NotifyPropertyChanged(nameof(Sto));
            }
        }
        public int BrojOsoba
        {
            get
            {
                return _brosoba;
            }
            set
            {
                _brosoba = value;
                NotifyPropertyChanged(nameof(BrojOsoba));
            }
        }
        public int PozicijaTop
        {
            get
            {
                return _ptop;
            }
            set
            {
                _ptop = value;
                NotifyPropertyChanged(nameof(PozicijaTop));
            }
        }
        public int PozicijaLeft
        {
            get
            {
                return _pleft;
            }
            set
            {
                _pleft = value;
                NotifyPropertyChanged(nameof(PozicijaLeft));
            }
        }
        public int Sirina
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                NotifyPropertyChanged(nameof(Sirina));
            }
        }
        public int Visina
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                NotifyPropertyChanged(nameof(Visina));
            }
        }
        public int Oblik
        {
            get
            {
                return _oblik;
            }
            set
            {
                _oblik = value;
                NotifyPropertyChanged(nameof(Oblik));
            }
        }
    }
}
