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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NoviReservationExpert.View.UserKontrole
{
    /// <summary>
    /// Interaction logic for uc_Kalendar.xaml
    /// </summary>
    public partial class uc_Kalendar : UserControl
    {
        DateTime _izabranDatum;
        public DateTime IzabranDatum
        {
            get
            {
                return _izabranDatum;
            }
            set
            {
                _izabranDatum = value;
            }
        }
        DateTime _prikazanDatum;
        public DateTime PrikazanDatum
        {
            get
            {
                return _prikazanDatum;
            }
            set
            {
                _prikazanDatum = value;
            }
        }
        public uc_Kalendar()
        {
            InitializeComponent();

            IzabranDatum = DateTime.Now;
            PrikazanDatum = DateTime.Now;

            tbMesec.Text = PrikazanDatum.ToString("yyyy. MMMM");

            PopuniDatume();
        }
        public class KalendarBtn : Button
        {
            public KalendarBtn()
            {

            }
        }
        private void PopuniDatume()
        {
            ugDatumi.Children.Clear();
            DateTime prvidatum = new DateTime(PrikazanDatum.Year, PrikazanDatum.Month, 1);
            int prvidan = (int)prvidatum.DayOfWeek;
            prvidan = prvidan - 1;// 0 je nedelja
            if (prvidan == -1)  prvidan = 6; 
            for (int i = 0; i < prvidan; i++)
            {
                Button btn = new Button();              
                btn.Background = Brushes.LightGray;
                btn.BorderBrush = Brushes.Transparent;
                btn.BorderThickness = new Thickness(0);
                btn.IsHitTestVisible = false;
                ugDatumi.Children.Add(btn);
            }
            for (int j = 0; j < DateTime.DaysInMonth(PrikazanDatum.Year, PrikazanDatum.Month); j++)
            {
                Button btn = new Button();
                btn.Background = Brushes.LightGray;
                if ((int)DateTime.Today.Day == j && DateTime.Today.Month == PrikazanDatum.Month) btn.Background = Brushes.Gray;
                if ((int)IzabranDatum.Day == j && DateTime.Today.Month == IzabranDatum.Month) btn.Background = Brushes.Aqua;
                btn.BorderBrush = Brushes.Transparent;
                btn.BorderThickness = new Thickness(0);
                btn.Content = j + 1;
                btn.Foreground = Brushes.Black;
                ugDatumi.Children.Add(btn);
            }
            int temp = ugDatumi.Children.Count;
            for(int k= 0; k < 42 - temp; k++)
            {
                Button btn = new Button();
                btn.Background = Brushes.LightGray;
                btn.BorderBrush = Brushes.Transparent;
                btn.BorderThickness = new Thickness(0);
                btn.IsHitTestVisible = false;
                ugDatumi.Children.Add(btn);
            }
        }

        private void PromeniMesec_Napred(object sender, RoutedEventArgs e)
        {
            PrikazanDatum = PrikazanDatum.AddMonths(1);
            tbMesec.Text = PrikazanDatum.ToString("yyyy. MMMM");
            PopuniDatume();
        }

        private void PromeniMesec_Nazad(object sender, RoutedEventArgs e)
        {
            PrikazanDatum = PrikazanDatum.AddMonths(-1);
            tbMesec.Text = PrikazanDatum.ToString("yyyy. MMMM");
            PopuniDatume();
        }
    }
}
