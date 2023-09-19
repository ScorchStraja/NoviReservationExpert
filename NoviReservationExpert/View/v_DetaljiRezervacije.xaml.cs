using NoviReservationExpert.Model;
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
    /// Interaction logic for v_DetaljiRezervacije.xaml
    /// </summary>
    public partial class v_DetaljiRezervacije : Window
    {
        vm_DetaljiRezervacije vm;
        public v_DetaljiRezervacije(re_Rezervacija rezervacija)
        {
            InitializeComponent();

            vm = new vm_DetaljiRezervacije(rezervacija, canvasStolovi);
            this.DataContext = vm;

            if (vm.ZatvoriFormu == null)
                vm.ZatvoriFormu = new Action(this.Close);

        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Kalendar.Visibility = Visibility.Visible;
        }

        private void Kalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Kalendar.Visibility = Visibility.Hidden;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Kalendar.Visibility = Visibility.Hidden;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lvVremenaPocetak.Visibility = Visibility.Hidden;
        }

        private void TextBox_GotFocus_1(object sender, RoutedEventArgs e)
        {
            lvVremenaPocetak.Visibility = Visibility.Visible;
            TimeSpan span = new TimeSpan(0, 30, 0);
            long ticks = DateTime.Now.Ticks / span.Ticks;
            lvVremenaPocetak.ScrollIntoView(new DateTime(ticks * span.Ticks, DateTime.Now.Kind).ToString("HH:mm"));
        }

        private void TextBox_LostFocus_1(object sender, RoutedEventArgs e)
        {
            lvVremenaPocetak.Visibility = Visibility.Hidden;
        }

        private void TextBox_GotFocus_2(object sender, RoutedEventArgs e)
        {
            lvVremenaKraj.Visibility = Visibility.Visible;
            TimeSpan span = new TimeSpan(0, 30, 0);
            long ticks = DateTime.Now.Ticks / span.Ticks;
            lvVremenaKraj.ScrollIntoView(new DateTime(ticks * span.Ticks, DateTime.Now.Kind).ToString("HH:mm"));
        }

        private void TextBox_LostFocus_2(object sender, RoutedEventArgs e)
        {
            lvVremenaKraj.Visibility = Visibility.Hidden;
        }

        private void lvVremenaKraj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lvVremenaKraj.Visibility = Visibility.Hidden;
        }

        private void lvVremenaKraj_LostFocus(object sender, RoutedEventArgs e)
        {
            lvVremenaKraj.Visibility = Visibility.Hidden;
            lvVremenaPocetak.Visibility = Visibility.Hidden;
        }
    }
}
