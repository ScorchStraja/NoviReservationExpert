using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoviReservationExpert.ViewModel
{
    public class vm_WarningBox : INotifyPropertyChanged
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
        #endregion

        #region parametri
        string _poruka;
        public string poruka
        {
            get
            {
                return _poruka;
            }
            set
            {
                _poruka = value;
                NotifyPropertyChanged(nameof(poruka));
            }
        }
        #endregion

        public vm_WarningBox(string poruka)
        {
            Zatvori_Command = new RelayCommand(Zatvori_Metoda);
            Potvrdi_Command = new RelayCommand(Potvrdi_Metoda);

            this.poruka = poruka;
        }

        private void Potvrdi_Metoda(object obj)
        {
            //Sistem.GlobalneVarijable.primljenaPoruka = true;
            ZatvoriFormu();
        }

        private void Zatvori_Metoda(object obj)
        {
            ZatvoriFormu();
        }
    }
}
