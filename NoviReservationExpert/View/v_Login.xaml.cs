using NoviReservationExpert.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
using System.Windows.Threading;

namespace NoviReservationExpert.View
{

    public partial class v_Login : Window
    {
        public v_Login()
        {
            InitializeComponent();
            Globalno.Varijable.LoginProzor = this;
            vm_Login vm = new vm_Login(btnPin, btnUser, brdPadajuciMeni, tasteriBrojevi, tasteriMalaSlova, tasteriVelikaSlova, pb, pbPin);
            this.DataContext = vm;


            if (vm.ZatvoriFormu == null)
                vm.ZatvoriFormu = new Action(this.Close);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, (object s, EventArgs ev) =>
            {
                this.datumVreme.Text = DateTime.Now.ToString("dd/MM/yyyy   H:mm");
            }, this.Dispatcher);
            timer.Start();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            pb.Focus();
        }
    }
}
