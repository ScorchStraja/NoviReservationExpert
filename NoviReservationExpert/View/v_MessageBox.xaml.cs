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
    /// Interaction logic for v_MessageBox.xaml
    /// </summary>
    public partial class v_MessageBox : Window
    {
        public v_MessageBox(string pitanje)
        {
            InitializeComponent();
            
            vm_MessageBox vm = new vm_MessageBox(pitanje);
            this.DataContext = vm;


            if (vm.ZatvoriFormu == null)
                vm.ZatvoriFormu = new Action(this.Close);           
        }            
    }
}
