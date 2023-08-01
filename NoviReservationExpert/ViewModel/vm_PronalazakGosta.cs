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
    internal class vm_PronalazakGosta : INotifyPropertyChanged
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
        #endregion

        #region PROPERTIJI
        ObservableCollection<re_Gost> SQLListaGostiju;
        ObservableCollection<re_Gost> _listaGostiju;
        public ObservableCollection<re_Gost> ListaGostiju
        {
            get
            {
                return _listaGostiju;
            }
            set
            {
                _listaGostiju = value;
                NotifyPropertyChanged(nameof(ListaGostiju));
            }
        }
        re_Gost _izabranGost;
        public re_Gost izabranGost
        {
            get
            {
                return _izabranGost;
            }
            set
            {
                _izabranGost = value;
                NotifyPropertyChanged(nameof(izabranGost));
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
        #endregion
        public vm_PronalazakGosta(string pretraga="-") 
        {
            Odustani_Command = new RelayCommand(Odustani_Metoda);
            Filter_Command = new RelayCommand(Filter_Metoda);
            IzaberiGosta_Command = new RelayCommand(IzaberiGosta_Metoda);

            SQLListaGostiju = Broker.BrokerSelect.dajSesiju().VratiGoste();
            ListaGostiju = SQLListaGostiju;
            if ( !string.IsNullOrEmpty(pretraga) || pretraga!= "-")
            {
                filterText = pretraga;
            }
            if (pretraga != "-")
            {
                Filter_Metoda(null);
            }
        }

        private void IzaberiGosta_Metoda(object obj)
        {
            if(izabranGost != null)
            {
                Globalno.Varijable.IzabranGost = izabranGost;
            }
            this.ZatvoriFormu();
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
                ObservableCollection<re_Gost> listaImena = new ObservableCollection<re_Gost>(ListaGostiju.Where(x => x.Ime.Contains(filterText, System.StringComparison.CurrentCultureIgnoreCase)));
                ObservableCollection<re_Gost> listaPrezimena = new ObservableCollection<re_Gost>(ListaGostiju.Where(x => x.Prezime.Contains(filterText, System.StringComparison.CurrentCultureIgnoreCase)));
                ObservableCollection<re_Gost> listaTelefona = new ObservableCollection<re_Gost>(ListaGostiju.Where(x=> x.Telefon.Contains(filterText,System.StringComparison.CurrentCultureIgnoreCase)));
                ObservableCollection<re_Gost> ImeIPrezime = new ObservableCollection<re_Gost>(listaImena.Union(listaPrezimena));
                ListaGostiju = new ObservableCollection<re_Gost>(ImeIPrezime.Union(listaTelefona));
            }
            else
            {
                SQLListaGostiju = Broker.BrokerSelect.dajSesiju().VratiGoste();
                ListaGostiju = SQLListaGostiju;
            }
        }
    }
}
