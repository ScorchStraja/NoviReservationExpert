using NoviReservationExpert.Model;
using NoviReservationExpert.View.UserKontrole;
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
using System.Windows.Threading;

namespace NoviReservationExpert.View
{
    /// <summary>
    /// Interaction logic for v_RadniProstor.xaml
    /// </summary>
    public partial class v_RadniProstor : Window
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

        vm_RadniProstor vm;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public v_RadniProstor()
        {
            InitializeComponent();
            vm = new vm_RadniProstor(dpSemeIDugmad, dpStolovi, dpVremena, prikazTabelarni, canvasSematski, svCanvas, spNotifikacije);
            Globalno.Varijable.RadniProstor = vm;
            this.DataContext = vm;

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 1, 0);
            dispatcherTimer.Start();

            prikazTrenutnogVremena.Text = DateTime.Now.ToString("HH:mm");
            int totalnominuta = (DateTime.Now.Hour - 7)* 60 + DateTime.Now.Minute;
            Canvas.SetLeft(crvenaLinija, totalnominuta * 2 + vm.sirinaStola - 20);

            if (vm.ZatvoriFormu == null)
                vm.ZatvoriFormu = new Action(this.Close);
        }

        private void dispatcherTimer_Tick(object? sender, EventArgs e)
        {
            prikazTrenutnogVremena.Text = DateTime.Now.ToString("HH:mm");
            Canvas.SetLeft(crvenaLinija, Canvas.GetLeft(crvenaLinija) + 2); // ako izmedju, npr. 14:00 i 14:30 ima 60 px, to znaci da za 30 minuta predje 60px, sto je 2px to 1m
        }
        private void PomeriPrikazLevo(object sender, RoutedEventArgs e)
        {
            svCanvas.ScrollToHorizontalOffset(svCanvas.HorizontalOffset - 60);
            if(Canvas.GetLeft(dpStolovi) == 0)
            {
                return;
            }
            Canvas.SetLeft(dpStolovi, Canvas.GetLeft(dpStolovi) - 60);
            Canvas.SetLeft(brdBlock, Canvas.GetLeft(brdBlock) - 60);
        }
        private void PomeriPrikazDesno(object sender, RoutedEventArgs e)
        {
            svCanvas.ScrollToHorizontalOffset(svCanvas.HorizontalOffset + 60);
            int br = 7 * 60;
            if(Canvas.GetLeft(dpStolovi) == br)
            {
                return;
            }
            Canvas.SetLeft(dpStolovi, Canvas.GetLeft(dpStolovi) + 60);
            Canvas.SetLeft(brdBlock, Canvas.GetLeft(brdBlock) + 60);
        }
        private void PomeriPrikazGore(object sender, RoutedEventArgs e)
        {
            svCanvas.ScrollToVerticalOffset(svCanvas.VerticalOffset - 60);
        }
        private void PomeriPrikazDole(object sender, RoutedEventArgs e)
        {           
            svCanvas.ScrollToVerticalOffset(svCanvas.VerticalOffset + 60);
        }
        private void svCanvas_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if(e.HorizontalChange != 0)
            {
                Canvas.SetLeft(dpStolovi, Canvas.GetLeft(dpStolovi) + e.HorizontalChange);
                Canvas.SetLeft(brdBlock, Canvas.GetLeft(brdBlock) + e.HorizontalChange);
            }
            if(e.VerticalOffset != 0)
            {
                Canvas.SetTop(dpVremena, e.VerticalOffset);
                Canvas.SetTop(crvenaLinija, e.VerticalOffset);
                if(e.VerticalOffset < 35)
                {
                    Canvas.SetTop(dpVremena, 0);
                    Canvas.SetTop(crvenaLinija, 0);
                    
                }
            } else
            {
                Canvas.SetTop(dpVremena, svCanvas.VerticalOffset);
                Canvas.SetTop(crvenaLinija, svCanvas.VerticalOffset);
            }
        }
        private bool otvorenBocniMeni = false;
        private void OtvoriBocniMeni(object sender, MouseButtonEventArgs e)
        {
            GridLengthAnimation bocnimenianimacija = new GridLengthAnimation();
            bocnimenianimacija.From = new GridLength(otvorenBocniMeni ? 250 : 0, GridUnitType.Pixel);
            bocnimenianimacija.To = new GridLength(otvorenBocniMeni ? 0 : 250, GridUnitType.Pixel); ;
            bocnimenianimacija.Duration = new TimeSpan(0, 0,0,0,300);
            gridRoot.ColumnDefinitions[1].BeginAnimation(ColumnDefinition.WidthProperty, bocnimenianimacija);

            otvorenBocniMeni = !otvorenBocniMeni;
        }
        private bool otvorenDonjiMeni = false;
        private void OtvoriDonjiMeni(object sender, MouseButtonEventArgs e)
        {
            GridLengthAnimation donjimenianimacija = new GridLengthAnimation();
            donjimenianimacija.From = new GridLength(otvorenDonjiMeni ? 150 : 0, GridUnitType.Pixel);
            donjimenianimacija.To = new GridLength(otvorenDonjiMeni ? 0 : 150, GridUnitType.Pixel); ;
            donjimenianimacija.Duration = new TimeSpan(0, 0, 0, 0, 300);
            gridDonjiMeni.RowDefinitions[1].BeginAnimation(RowDefinition.HeightProperty, donjimenianimacija);

            otvorenDonjiMeni = !otvorenDonjiMeni;
        }
    }
}
