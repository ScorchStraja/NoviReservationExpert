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

        string _ime;
        string _prezime;
        string _telefon;


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
        public string Telefon
        {
            get
            {
                return _telefon;
            }
            set
            {
                _telefon = value;
                NotifyPropertyChanged(nameof(Telefon));
            }
        }
    }
}
