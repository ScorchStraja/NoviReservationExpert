using NoviReservationExpert.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Interaction logic for v_MenjanjeBoja.xaml
    /// </summary>
    public partial class v_Konfiguracija : Window
    {
        vm_Konfiguracija vm;
        public v_Konfiguracija()
        {
            InitializeComponent();
            vm = new vm_Konfiguracija();
            this.DataContext = vm;

            cp_primarnaBoja.SelectedColor = BrushConverter((SolidColorBrush)Application.Current.FindResource("Plava"));
            cp_sekundarnaBoja.SelectedColor = BrushConverter((SolidColorBrush)Application.Current.FindResource("Plava_SV"));
            cp_RadniProstor.SelectedColor = BrushConverter((SolidColorBrush)Application.Current.FindResource("RadniProstor"));
            cp_UToku.SelectedColor = BrushConverter((SolidColorBrush)Application.Current.FindResource("UToku"));
            cp_Otkazano.SelectedColor = BrushConverter((SolidColorBrush)Application.Current.FindResource("Otkazano"));
            cp_Rezervisano.SelectedColor = BrushConverter((SolidColorBrush)Application.Current.FindResource("Rezervisano"));
            cp_Zavrseno.SelectedColor = BrushConverter((SolidColorBrush)Application.Current.FindResource("Zavrseno"));
        }
        //promenaBoje
        private void cp_primarnaBoja_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            Application.Current.Resources["Plava"] = new SolidColorBrush((System.Windows.Media.Color)e.NewValue);
        }
        private void cp_sekundarnaBoja_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            Application.Current.Resources["Plava_SV"] = new SolidColorBrush((System.Windows.Media.Color)e.NewValue);
        }
        private void cp_RadniProstor_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            Application.Current.Resources["RadniProstor"] = new SolidColorBrush((System.Windows.Media.Color)e.NewValue);
        }
        private void cp_UToku_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            Application.Current.Resources["UToku"] = new SolidColorBrush((System.Windows.Media.Color)e.NewValue);
        }
        private void cp_Otkazano_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            Application.Current.Resources["Otkazano"] = new SolidColorBrush((System.Windows.Media.Color)e.NewValue);
        }
        private void cp_Rezervisano_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            Application.Current.Resources["Rezervisano"] = new SolidColorBrush((System.Windows.Media.Color)e.NewValue);

        }
        private void cp_Zavrseno_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            Application.Current.Resources["Zavrseno"] = new SolidColorBrush((System.Windows.Media.Color)e.NewValue);
        }
        private System.Windows.Media.Color BrushConverter(SolidColorBrush brush)
        {
            return System.Windows.Media.Color.FromArgb(brush.Color.A,brush.Color.R,brush.Color.G,brush.Color.B);
        }

        //UNDO
        private void cp_primarnaBoja_Reset(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources["Plava"] = (SolidColorBrush)new BrushConverter().ConvertFrom("#0380BF");
            cp_primarnaBoja.SelectedColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#0380BF");
        }
        private void cp_sekundarnaBoja_Reset(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources["Plava_SV"] = (SolidColorBrush)new BrushConverter().ConvertFrom("#0380BF");
            cp_sekundarnaBoja.SelectedColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#0380BF");
        }
        private void cp_UToku_Reset(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources["UToku"] = (SolidColorBrush)new BrushConverter().ConvertFrom("#65E04D");
            cp_UToku.SelectedColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#65E04D");
        }
        private void cp_Otkazano_Reset(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources["Otkazano"] = (SolidColorBrush)new BrushConverter().ConvertFrom("#DA3A3A");
            cp_Otkazano.SelectedColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#DA3A3A");
        }
        private void cp_Rezervisano_Reset(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources["Zavrseno"] = (SolidColorBrush)new BrushConverter().ConvertFrom("#696969");
            cp_Zavrseno.SelectedColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#696969");
        }
        private void cp_Zavrseno_Reset(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources["Rezervisano"] = (SolidColorBrush)new BrushConverter().ConvertFrom("#0380BF");
            cp_Rezervisano.SelectedColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#0380BF");
        }
        private void cp_RadniProstor_Reset(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources["RadniProstor"] = (SolidColorBrush)new BrushConverter().ConvertFrom("#f5f5f5");
            cp_RadniProstor.SelectedColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#f5f5f5");
        }
    }
}
