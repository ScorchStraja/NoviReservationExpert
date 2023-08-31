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
    /// Interaction logic for v_SlobodniStolovi.xaml
    /// </summary>
    public partial class v_SlobodniStolovi : Window
    {
        vm_SlobodniStolovi vm;
        public v_SlobodniStolovi()
        {
            InitializeComponent();

            vm = new vm_SlobodniStolovi(dpSlobodniStolovi, canvasStolovi);
            this.DataContext = vm;

            if (vm.ZatvoriFormu == null)
                vm.ZatvoriFormu = new Action(this.Close);
        }
    }
}
