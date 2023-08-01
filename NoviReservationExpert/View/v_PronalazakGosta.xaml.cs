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
    public partial class v_PronalazakGosta : Window
    {
        vm_PronalazakGosta vm;
        public v_PronalazakGosta(string filter = "-")
        {
            InitializeComponent();

            vm = new vm_PronalazakGosta(filter);
            this.DataContext = vm;

            if (vm.ZatvoriFormu == null)
                vm.ZatvoriFormu = new Action(this.Close);
        }
    }
}
