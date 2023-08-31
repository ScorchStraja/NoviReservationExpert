using NoviReservationExpert.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NoviReservationExpert.View
{
    /// <summary>
    /// Interaction logic for v_PregledRezervacija.xaml
    /// </summary>
    public partial class v_PregledRezervacija : Window
    {
        vm_PregledRezervacija vm;
        public v_PregledRezervacija()
        {
            InitializeComponent();
            vm = new vm_PregledRezervacija();
            this.DataContext = vm;

            btnSve.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#427aa1");

            if (vm.ZatvoriFormu != null ) 
            {
                vm.ZatvoriFormu = new Action(this.Close);
            }
        }

        private void TrenutnoPrikazivanje(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            foreach(Button button in dpDugmici.Children.OfType<Button>())
            {
                button.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#284b63");
            }
            btn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#427aa1");
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Kalendar.SelectedDate == new DateTime())
            {
                Kalendar.SelectedDate = DateTime.Today;
            }
            Kalendar.Visibility = Visibility.Collapsed;
            DateTime dt = (DateTime)Kalendar.SelectedDate;
            btnKalendar.Content = dt.ToString("dd.MM.yyyy");
        }

        private void PrikaziSakrijKalendar(object sender, RoutedEventArgs e)
        {
            if(Kalendar.Visibility == Visibility.Visible)
            {
                Kalendar.Visibility = Visibility.Collapsed;
            }
            else
            {
                Kalendar.Visibility = Visibility.Visible;
            }
        }
    }
}
