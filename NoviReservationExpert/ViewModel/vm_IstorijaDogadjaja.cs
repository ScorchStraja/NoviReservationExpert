using NoviReservationExpert.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoviReservationExpert.ViewModel
{
    internal class vm_IstorijaDogadjaja : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #region KOMANDE
        public Action ZatvoriFormu { get; set; }
        public RelayCommand Odustani_Command { get; set; }
        public RelayCommand Filter_Command { get; private set; }
        public RelayCommand IzaberiGosta_Command { get; private set; }
        public RelayCommand IzbrisanFilter_Command { get; private set; }
        public RelayCommand PromenaDatuma_Command { get; private set; }
        #endregion

        #region PROPERTIJI
        ObservableCollection<re_Log> SQLListaLog;

        ObservableCollection<re_Log> _ListaLog;
        public ObservableCollection<re_Log> ListaLog
        {
            get
            {
                return _ListaLog;
            }
            set
            {
                _ListaLog = value;
                NotifyPropertyChanged(nameof(ListaLog));
            }
        }
        
        string _filterText;
        public string filterText
        {
            get
            {
                return _filterText;
            }
            set
            {
                _filterText = value;
                NotifyPropertyChanged(nameof(filterText));
            }
        }

        DateTime _IzabranDatum;
        public DateTime IzabranDatum
        {
            get
            {
                return _IzabranDatum;
            }
            set
            {
                _IzabranDatum = value;
                NotifyPropertyChanged(nameof(IzabranDatum));
            }
        }
        #endregion
        public vm_IstorijaDogadjaja(string pretraga="") 
        {
            Odustani_Command = new RelayCommand(Odustani_Metoda);
            Filter_Command = new RelayCommand(Filter_Metoda);
            IzbrisanFilter_Command = new RelayCommand(IzbrisanFilter_Metoda);
            PromenaDatuma_Command = new RelayCommand(PromenaDatuma_Metoda);

            IzabranDatum = DateTime.Today;

            SQLListaLog = Broker.BrokerSelect.dajSesiju().VratiLog(IzabranDatum);
            ListaLog = SQLListaLog;
            if ( !string.IsNullOrEmpty(pretraga) || pretraga!= "-")
            {
                filterText = pretraga;
            }
            if (pretraga != "-")
            {
                Filter_Metoda(null);
            }
        }

        private void PromenaDatuma_Metoda(object obj)
        {
            filterText = "";
            SQLListaLog = Broker.BrokerSelect.dajSesiju().VratiLog(IzabranDatum);
            ListaLog = SQLListaLog;
        }

        private void IzbrisanFilter_Metoda(object obj)
        {
            if (filterText.Length == 0)
            {
                filterText = "";
                SQLListaLog = Broker.BrokerSelect.dajSesiju().VratiLog(IzabranDatum);
                ListaLog = SQLListaLog;
            }
        }

        private void Odustani_Metoda(object obj)
        {
            Globalno.Varijable.IzabranGost = null;
            this.ZatvoriFormu();
        }

        private void Filter_Metoda(object obj)
        {
            if (!string.IsNullOrEmpty(filterText))
            {
                ObservableCollection<re_Log> listaOpis = new ObservableCollection<re_Log>(ListaLog.Where(x => x.Opis.Contains(filterText, System.StringComparison.CurrentCultureIgnoreCase)));
                ListaLog = new ObservableCollection<re_Log>(listaOpis);
            }
            else
            {
                SQLListaLog = Broker.BrokerSelect.dajSesiju().VratiLog(IzabranDatum);
                ListaLog = SQLListaLog;
            }
        }
    }
}
