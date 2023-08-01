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
    public partial class v_WarningBox : Window
    {
        public v_WarningBox(string poruka)
        {
            InitializeComponent();

            ViewModel.vm_WarningBox vm = new ViewModel.vm_WarningBox(poruka);
            this.DataContext = vm;

            if (vm.ZatvoriFormu == null)
            {
                vm.ZatvoriFormu = new Action(this.Close);
            }
        }
    }
}
