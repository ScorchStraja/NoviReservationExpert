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
    }
}
