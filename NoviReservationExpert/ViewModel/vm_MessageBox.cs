using NoviReservationExpert.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoviReservationExpert.View
{
    public class vm_MessageBox: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #region komande
        public Action ZatvoriFormu { get; set; }
        public RelayCommand Zatvori_Command { get; private set; }
        public RelayCommand Potvrdi_Command { get; private set; }
        public RelayCommand Otkazi_Command { get; private set; }
        #endregion

        #region propertiji

        string _pitanje;
        public string pitanje
        {
            get
            {
                return _pitanje;
            }
            set
            {
                _pitanje = value;
                NotifyPropertyChanged(nameof(pitanje));
            }
        }
        #endregion

        public vm_MessageBox(string pitanje)
        {
            Zatvori_Command = new RelayCommand(Zatvori_Metoda);
            Potvrdi_Command = new RelayCommand(Potvrdi_Metoda);
            Otkazi_Command = new RelayCommand(Otkazi_Metoda);

            this.pitanje = pitanje;
        }

        private void Otkazi_Metoda(object obj)
        {
            Globalno.Varijable.sacuvanePromene = false;
            ZatvoriFormu();
        }

        private void Potvrdi_Metoda(object obj)
        {
            Globalno.Varijable.sacuvanePromene = true;
            ZatvoriFormu();
        }

        private void Zatvori_Metoda(object obj)
        {
            ZatvoriFormu();
        }
    }
}
