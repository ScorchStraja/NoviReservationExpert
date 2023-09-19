using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoviReservationExpert.ViewModel
{
    public class vm_Konfiguracija : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public RelayCommand PrikaziOtkazane_Command { get; set; }
        public RelayCommand NePrikazujOtkazane_Command { get; set; }
        public RelayCommand DaAutomatskoMenjanjeStatusa_Command { get; set; }
        public RelayCommand NeAutomatskoMenjanjeStatusa_Command { get; set; }

        bool _ceker_POR;
        public bool ceker_POR
        {
            get
            {
                return _ceker_POR;
            }
            set
            {
                _ceker_POR = value;
                NotifyPropertyChanged(nameof(ceker_POR));
            }
        }
        bool _ceker_AMS;
        public bool ceker_AMS
        {
            get
            {
                return _ceker_AMS;
            }
            set
            {
                _ceker_AMS = value;
                NotifyPropertyChanged(nameof(ceker_AMS));
            }
        }

        public vm_Konfiguracija() 
        {
            PrikaziOtkazane_Command = new RelayCommand(PrikaziOtkazaneMetoda);
            NePrikazujOtkazane_Command = new RelayCommand(NePrikazujOtkazaneMetoda);
            DaAutomatskoMenjanjeStatusa_Command = new RelayCommand(DaAutomatskoMenjanjeStatusa_Metoda);
            NeAutomatskoMenjanjeStatusa_Command = new RelayCommand(NeAutomatskoMenjanjeStatusa_Metoda);

            ceker_AMS = Globalno.Varijable.AutomatskoMenjanjeStatusa;
            ceker_POR = Globalno.Varijable.PrikazujOtkazane;
        }
        private void DaAutomatskoMenjanjeStatusa_Metoda(object obj)
        {
            Globalno.Varijable.AutomatskoMenjanjeStatusa = true;
            Broker.BrokerInsert.dajSesiju().UpisiParametar_AutomatskoMenjanjeStatusa("Da");
        }
        private void NeAutomatskoMenjanjeStatusa_Metoda(object obj)
        {
            Globalno.Varijable.AutomatskoMenjanjeStatusa = false;
            Broker.BrokerInsert.dajSesiju().UpisiParametar_AutomatskoMenjanjeStatusa("Ne");
        }
        private void PrikaziOtkazaneMetoda(object obj)
        {
            Globalno.Varijable.PrikazujOtkazane = true;
            Broker.BrokerInsert.dajSesiju().UpisiParametar_PrikazivanjeOtkazanihRezervacija("Da");
        }
        private void NePrikazujOtkazaneMetoda(object obj)
        {
            Globalno.Varijable.PrikazujOtkazane = false;
            Broker.BrokerInsert.dajSesiju().UpisiParametar_PrikazivanjeOtkazanihRezervacija("Ne");
        }


    }
}
