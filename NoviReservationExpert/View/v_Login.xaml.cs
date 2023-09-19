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

            NapraviLinije();
        }

        private void NapraviLinije()
        {
            for(int i = 0; i <= 59; i++)
            {
                Line hlinija = new Line();
                Panel.SetZIndex(hlinija, 0);
                hlinija.Stroke = Brushes.Black;
                hlinija.Opacity = 0.1;
                double[] dbl = new double[] { 6, 6 };
                hlinija.StrokeDashArray = new DoubleCollection(dbl);
                hlinija.Y1 = 0;
                hlinija.Y2 = 5000;
                hlinija.X1 = 60 * i;
                hlinija.X2 = 60 * i;
                loginGrid.Children.Add(hlinija);

                Line vlinija = new Line();
                vlinija.StrokeThickness = 2;
                vlinija.Stroke = Brushes.Black;
                vlinija.Opacity = 0.05;
                double[] vdbl = new double[] { 4, 0 };
                vlinija.StrokeDashArray = new DoubleCollection(vdbl);
                vlinija.Y1 = 60 * i + 1; //+1 da bi izravnao sa borderom
                vlinija.Y2 = 60 * i + 1;
                vlinija.X1 = 0;
                vlinija.X2 = 5000;
                loginGrid.Children.Add(vlinija);
            }

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
