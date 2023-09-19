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
            vm = new vm_PregledRezervacija(dpDugmici);
            this.DataContext = vm;

            btnUToku.Background = (Brush)Application.Current.FindResource("Plava_SV");

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
                button.Background = (Brush)Application.Current.FindResource("Plava");
            }
            btn.Background = (Brush)Application.Current.FindResource("Plava_SV");
        }

        private void CalendarOD_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if(KalendarOD.SelectedDate == new DateTime())
            {
                KalendarOD.SelectedDate = DateTime.Today;
            }
            KalendarOD.Visibility = Visibility.Collapsed;
            KalendarDO.Visibility = Visibility.Collapsed;
            DateTime dt = (DateTime)KalendarOD.SelectedDate;
            btnKalendarOD.Content = "OD: " + dt.ToString("dd.MM.yyyy");
        }

        private void PrikaziSakrijKalendarOD(object sender, RoutedEventArgs e)
        {
            if(KalendarOD.Visibility == Visibility.Visible)
            {
                KalendarOD.Visibility = Visibility.Collapsed;
                KalendarDO.Visibility = Visibility.Collapsed;
            }
            else
            {
                KalendarOD.Visibility = Visibility.Visible;
                KalendarDO.Visibility = Visibility.Collapsed;
            }
        }

        private void CalendarDO_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (KalendarDO.SelectedDate == new DateTime())
            {
                KalendarDO.SelectedDate = DateTime.Today;
            }
            KalendarOD.Visibility = Visibility.Collapsed;
            KalendarDO.Visibility = Visibility.Collapsed;
            DateTime dt = (DateTime)KalendarDO.SelectedDate;
            btnKalendarDO.Content = "DO: " + dt.ToString("dd.MM.yyyy");
        }

        private void PrikaziSakrijKalendarDO(object sender, RoutedEventArgs e)
        {
            if (KalendarDO.Visibility == Visibility.Visible)
            {
                KalendarOD.Visibility = Visibility.Collapsed;
                KalendarDO.Visibility = Visibility.Collapsed;
            }
            else
            {
                KalendarOD.Visibility = Visibility.Collapsed;
                KalendarDO.Visibility = Visibility.Visible;
            }
        }

        private void ResetPretragu(object sender, TextChangedEventArgs e)
        {
            if(tbPretraga.Text.Length == 0)
            {
                this.vm.UkloniPretraga_Metoda(null);
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                this.vm.Pretraga_Metoda(null);
            }
        }
    }
}
