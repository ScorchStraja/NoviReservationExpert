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
    /// Interaction logic for v_PronalazakGosta.xaml
    /// </summary>
    public partial class v_IstorijaDogadjaja: Window
    {
        vm_IstorijaDogadjaja vm;
        public v_IstorijaDogadjaja(string filter = "-")
        {
            InitializeComponent();

            vm = new vm_IstorijaDogadjaja(filter);
            this.DataContext = vm;
            
            btnKalendarOD.Content = DateTime.Today.ToString("dd.MM.yyyy");

            if (vm.ZatvoriFormu == null)
                vm.ZatvoriFormu = new Action(this.Close);
        }
        private void CalendarOD_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (KalendarOD.SelectedDate == new DateTime())
            {
                KalendarOD.SelectedDate = DateTime.Today;
            }
            KalendarOD.Visibility = Visibility.Collapsed;
            DateTime dt = (DateTime)KalendarOD.SelectedDate;
            btnKalendarOD.Content = dt.ToString("dd.MM.yyyy");
        }

        private void PrikaziSakrijKalendarOD(object sender, RoutedEventArgs e)
        {
            if (KalendarOD.Visibility == Visibility.Visible)
            {
                KalendarOD.Visibility = Visibility.Collapsed;
            }
            else
            {
                KalendarOD.Visibility = Visibility.Visible;
            }
        }
    }
}
