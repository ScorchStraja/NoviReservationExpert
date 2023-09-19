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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NoviReservationExpert.View
{
    /// <summary>
    /// Interaction logic for v_SlobodniStolovi.xaml
    /// </summary>
    public partial class v_SlobodniStolovi : Window
    {
        //Grid width i height animacija
        #region Animacija
        internal class GridLengthAnimation : AnimationTimeline
        {
            public override Type TargetPropertyType
            {
                get
                {
                    return typeof(GridLength);
                }
            }
            protected override System.Windows.Freezable CreateInstanceCore()
            {
                return new GridLengthAnimation();
            }
            static GridLengthAnimation()
            {
                FromProperty = DependencyProperty.Register("From", typeof(GridLength),
                    typeof(GridLengthAnimation));

                ToProperty = DependencyProperty.Register("To", typeof(GridLength),
                    typeof(GridLengthAnimation));
            }
            public static readonly DependencyProperty FromProperty;
            public GridLength From
            {
                get
                {
                    return (GridLength)GetValue(GridLengthAnimation.FromProperty);
                }
                set
                {
                    SetValue(GridLengthAnimation.FromProperty, value);
                }
            }
            public static readonly DependencyProperty ToProperty;
            public GridLength To
            {
                get
                {
                    return (GridLength)GetValue(GridLengthAnimation.ToProperty);
                }
                set
                {
                    SetValue(GridLengthAnimation.ToProperty, value);
                }
            }
            public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
            {
                double fromVal = ((GridLength)GetValue(GridLengthAnimation.FromProperty)).Value;
                double toVal = ((GridLength)GetValue(GridLengthAnimation.ToProperty)).Value;

                if (fromVal > toVal)
                {
                    return new GridLength((1 - animationClock.CurrentProgress.Value) *
                        (fromVal - toVal) + toVal, GridUnitType.Pixel);
                }
                else
                {
                    return new GridLength(animationClock.CurrentProgress.Value *
                        (toVal - fromVal) + fromVal, GridUnitType.Pixel);
                }
            }
        }
        #endregion


        vm_SlobodniStolovi vm;
        public v_SlobodniStolovi()
        {
            InitializeComponent();

            vm = new vm_SlobodniStolovi(canvasStolovi, dpSlobodniStolovi);
            this.DataContext = vm;

            if (vm.ZatvoriFormu == null)
                vm.ZatvoriFormu = new Action(this.Close);
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            lvVremenaPocetak.Visibility = Visibility.Visible;
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            lvVremenaPocetak.Visibility = Visibility.Hidden;
        }
        private void lvVremenaPocetak_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lvVremenaPocetak.Visibility = Visibility.Hidden;
        }
        private void TextBox_GotFocus_1(object sender, RoutedEventArgs e)
        {
            lvVremenaKraj.Visibility = Visibility.Visible;
        }
        private void TextBox_LostFocus_1(object sender, RoutedEventArgs e)
        {
            lvVremenaKraj.Visibility = Visibility.Hidden;
        }
        private void lvVremenaKraj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lvVremenaKraj.Visibility = Visibility.Hidden;
        }
        private bool otvorenBocniMeni = false;
        private void OtvoriBocniMeni(object sender, MouseButtonEventArgs e)
        {
            GridLengthAnimation bocnimenianimacija = new GridLengthAnimation();
            bocnimenianimacija.From = new GridLength(otvorenBocniMeni ? 100 : 0, GridUnitType.Pixel);
            bocnimenianimacija.To = new GridLength(otvorenBocniMeni ? 0 : 100, GridUnitType.Pixel); ;
            bocnimenianimacija.Duration = new TimeSpan(0, 0, 0, 0, 300);
            gridAnimacija.ColumnDefinitions[1].BeginAnimation(ColumnDefinition.WidthProperty, bocnimenianimacija);

            otvorenBocniMeni = !otvorenBocniMeni;
        }
    }
}
