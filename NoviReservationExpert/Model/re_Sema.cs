using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoviReservationExpert.Model
{
    public class re_Sema : INotifyPropertyChanged
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
            return Sema;
        }

        int _obj;
        string _sema;
        int _odabran;
        string _napomena;
        string _slika;

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
        public int Odabran
        {
            get
            {
                return _odabran;
            }
            set
            {
                _odabran = value;
                NotifyPropertyChanged(nameof(Odabran));
            }
        }
        public string Slika
        {
            get
            {
                return _slika;
            }
            set
            {
                _slika = value;
                NotifyPropertyChanged(nameof(Slika));
            }
        }
        public string Napomena
        {
            get
            {
                return _napomena;
            }
            set
            {
                _napomena = value;
                NotifyPropertyChanged(nameof(Napomena));
            }
        }

    }
}
