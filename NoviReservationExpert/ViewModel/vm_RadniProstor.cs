using NoviReservationExpert.Model;
using NoviReservationExpert.View;
using NoviReservationExpert.View.UserKontrole;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NoviReservationExpert.ViewModel
{
    public class vm_RadniProstor : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #region KOMANDE
        public Action ZatvoriFormu { get; set; }
        public RelayCommand Zatvori_Command { get; private set; }
        public RelayCommand OtvoriRadniMeni_Command { get; private set; }
        public RelayCommand NadjiSlobodanSto_Command { get; private set; }
        #endregion

        #region PROPERTIJI
        DateTime _IzabranDatum;
        public DateTime IzabranDatum
        {
            get
            {
                return _IzabranDatum;
            }
            set
            {
                _IzabranDatum = value;
                Globalno.Varijable.IzabranDatum = IzabranDatum;
                PopuniPrikazRezervacijama(_IzabranDatum);
                NotifyPropertyChanged(nameof(IzabranDatum));
            }
        }


        string _IzabranoVreme;
        public string IzabranoVreme
        {
            get
            {
                return _IzabranoVreme;
            }
            set
            {
                _IzabranoVreme = value;
                NotifyPropertyChanged(nameof(IzabranoVreme));
            }
        }


        string _BrojGostiju;
        public string BrojGostiju
        {
            get
            {
                return _BrojGostiju;
            }
            set
            {
                _BrojGostiju = value;
                NotifyPropertyChanged(nameof(BrojGostiju));
            }
        }


        string _Gost = "";
        public string Gost
        {
            get
            {
                return _Gost;
            }
            set
            {
                _Gost = value;
                NotifyPropertyChanged(nameof(Gost));
            }
        }


        int _SirinaCanvasa;
        public int SirinaCanvasa
        {
            get
            {
                return _SirinaCanvasa;
            }
            set 
            {
                _SirinaCanvasa = value;
                NotifyPropertyChanged(nameof(SirinaCanvasa));
            }
        }


        re_Sema _izabranaSema;
        public re_Sema IzabranaSema
        {
            get
            {
                return _izabranaSema;
            }
            set
            {
                _izabranaSema = value;
                Globalno.Varijable.IzabranaSema = _izabranaSema;
                PopuniStolove(); //svaki put kad dodje do promene seme, menjam prikaz
                PopuniPrikazRezervacijama(_IzabranDatum);
                NotifyPropertyChanged(nameof(IzabranaSema));
            }
        }


        ObservableCollection<re_Sema> Seme = new ObservableCollection<re_Sema>();
        ObservableCollection<re_Sto> Stolovi = new ObservableCollection<re_Sto>();
        ObservableCollection<re_Rezervacija> Rezervacije = new ObservableCollection<re_Rezervacija>();
        List<uc_Rezervacija> BrdRezervacije = new List<uc_Rezervacija>(); //rezervacije koje se prikazuju, za svaku rezervaciju u Rezervacije, ovde postoji border
        DockPanel dpSemeIDugmad;
        DockPanel dpVremena;
        DockPanel dpStolovi;
        ScrollViewer svCanvas;
        Canvas canvasPrikaz;
        public int sirinaVremena = 60;
        public int visinaVremena = 40;
        public int visinaStola = 60;
        public int sirinaStola = 70;
        public int brojodeljka = 0;

        public class MojBorderSema : Border
        {
            public re_Sema Sema;
            public MojBorderSema(re_Sema sema)
            {
                this.Sema = sema;      
                this.Margin = new System.Windows.Thickness(5, 5, 5, 5);
                this.Width = 150;
                this.CornerRadius = new System.Windows.CornerRadius(3);
                this.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#284b63");
                StackPanel sp = new StackPanel();
                this.Child = sp;
                sp.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                sp.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                TextBlock tb = new TextBlock();
                if(sema.ToString().Count() > 9)
                {
                    tb.Text = sema.ToString().Substring(0, 11);
                }
                else
                {
                    tb.Text = sema.ToString();
                }
                tb.Margin = new System.Windows.Thickness(0, 0, 0, 5);
                tb.Foreground = Brushes.White;
                tb.FontSize = 16;
                tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                sp.Children.Add(tb);
                Border slika = new Border();
                Image Img = new Image();
                Img.Source = new BitmapImage(new Uri("pack://application:,,,/NoviReservationExpert;component/Resursi/Slike/DefaultSema.png"));
                slika.Child = Img;
                slika.Height = 100;
                slika.Width = 130;
                slika.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                slika.Background = Brushes.DarkGray;
                slika.CornerRadius = new System.Windows.CornerRadius(3);
                slika.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                sp.Children.Add(slika);
            }
        }
        #endregion

        public vm_RadniProstor(DockPanel dpSemeIDugmad, DockPanel dpStolovi, DockPanel dpVremena, Canvas prikaz, ScrollViewer svCanvas)
        {
            Zatvori_Command = new RelayCommand(Zatvori_Metoda);
            OtvoriRadniMeni_Command = new RelayCommand(OtvoriRadniMeni_Metoda);
            NadjiSlobodanSto_Command = new RelayCommand(NadjiSlobodanSto_Metoda);
            Seme = Broker.BrokerSelect.dajSesiju().vratiSeme(); // automatski vraca seme za izabran objekat sa login-a

            canvasPrikaz = prikaz;
            canvasPrikaz.MouseMove += Pomeranje;
            this.dpStolovi = dpStolovi;
            this.dpVremena = dpVremena;
            this.svCanvas = svCanvas;
            this.dpSemeIDugmad = dpSemeIDugmad;
            foreach (re_Sema sema in Seme)
            {
                if (sema.Odabran == 1)
                {
                    IzabranaSema = sema;
                }
            }

            this.IzabranDatum = DateTime.Today;

            PopuniVremena();
            PopuniPrikazSema();

            foreach (MojBorderSema brd1 in dpSemeIDugmad.Children.OfType<MojBorderSema>())
            {
                if (brd1.Sema.Odabran == 1)
                {
                    brd1.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#427aa1");
                }
            }
        }

        private void PopuniPrikazRezervacijama(DateTime datum)
        {
            for(int i = canvasPrikaz.Children.Count - 1; i >= 0; i--)
            {
                if (canvasPrikaz.Children[i].GetType() == typeof(uc_Rezervacija))
                {
                    canvasPrikaz.Children.RemoveAt(i);
                }
            }
            BrdRezervacije.Clear();
            Rezervacije = Broker.BrokerSelect.dajSesiju().VratiRezervacijeZaDatum(datum, Globalno.Varijable.Objekat.Objekat, IzabranaSema.Sema);
            foreach (re_Rezervacija rezervacija in Rezervacije)
            {
                int vremeUMin = (rezervacija.VremeOd.Hour - 7)*sirinaVremena + rezervacija.VremeOd.Minute;
                int ypozicija = sirinaStola + vremeUMin * 2;
                uc_Rezervacija brdRez = new uc_Rezervacija(rezervacija);
                if (brdRez.OsveziRadniProstor == null)
                    brdRez.OsveziRadniProstor = new Action(OsveziRadniProstor);

                brdRez.MouseLeftButtonDown += PocetakPomeranja;
                brdRez.MouseLeftButtonUp += ZavrsetakPomeranja;

                int xpozicija = Stolovi.IndexOf(Stolovi.First(x => x.Sto == rezervacija.Sto));

                canvasPrikaz.Children.Add(brdRez);
                BrdRezervacije.Add(brdRez);
               
                Canvas.SetLeft(brdRez, ypozicija);
                Canvas.SetTop(brdRez, visinaVremena + xpozicija * visinaStola);
            }
        }

        #region PomeranjeRezervacija
        bool pocetakPomeranja = false;
        bool pomeranje = false;
        uc_Rezervacija rezervacijaKojaSePomera;
        private void PocetakPomeranja(object sender, MouseButtonEventArgs e)
        {
            pocetakPomeranja = true;
            rezervacijaKojaSePomera = sender as uc_Rezervacija;
        }
        private void Pomeranje(object sender, MouseEventArgs e)
        {
            if (pocetakPomeranja == true)
            {
                if (rezervacijaKojaSePomera != null)
                {
                    pomeranje = true;
                    //double razlikaX = Mouse.GetPosition(canvasPrikaz).X - Canvas.GetLeft(rezervacijaKojaSePomera);
                    //double razlikaY = Mouse.GetPosition(canvasPrikaz).Y - Canvas.GetTop(rezervacijaKojaSePomera);
                    Canvas.SetTop(rezervacijaKojaSePomera, Mouse.GetPosition(canvasPrikaz).Y - 30);
                    Canvas.SetLeft(rezervacijaKojaSePomera, Mouse.GetPosition(canvasPrikaz).X - 100);
                }
            }
        }
        private void ZavrsetakPomeranja(object sender, MouseButtonEventArgs e)
        {
            uc_Rezervacija rezervacija = sender as uc_Rezervacija;
            if (pomeranje == false)
            {
                v_DetaljiRezervacije prozor = new v_DetaljiRezervacije(rezervacija.Rezervacija);
                prozor.ShowDialog();
            }
            pocetakPomeranja = false;
            pomeranje = false;
            double koordinataX = Mouse.GetPosition(canvasPrikaz).X - sirinaStola - sirinaVremena;
            double koordinataY = Mouse.GetPosition(canvasPrikaz).Y - visinaVremena;

            int xbrojCelina = Convert.ToInt32(koordinataX / sirinaVremena - 1);
            if (xbrojCelina < 0) xbrojCelina = 0;
            Canvas.SetLeft(rezervacijaKojaSePomera, xbrojCelina * sirinaVremena + sirinaStola);

            int ybrojCelina = Convert.ToInt32(koordinataY / visinaStola - 1);
            if (ybrojCelina < 0) ybrojCelina = 0;
            Canvas.SetTop(rezervacijaKojaSePomera, ybrojCelina * visinaStola + visinaVremena);

            rezervacija.Rezervacija.Sto = Stolovi[ybrojCelina].Sto;
            TimeSpan trajanje = rezervacija.Rezervacija.VremeDo - rezervacija.Rezervacija.VremeOd;
            rezervacija.Rezervacija.VremeOd = DateTime.Today.AddHours(7).AddMinutes(xbrojCelina * 30);
            rezervacija.Rezervacija.VremeDo = rezervacija.Rezervacija.VremeOd.AddMinutes(trajanje.TotalMinutes);
            rezervacija.PromeniVreme(rezervacija.Rezervacija.VremeOd, rezervacija.Rezervacija.VremeDo);
            Broker.BrokerUpdate.dajSesiju().UpdateRezervaciju(rezervacija.Rezervacija);
        }
        #endregion

        public void OsveziRadniProstor()
        {
            PopuniPrikazRezervacijama(IzabranDatum);
        }
        private void NadjiSlobodanSto_Metoda(object obj)
        {
            if(BrojGostiju == null)
            {
                v_WarningBox warningprozor = new v_WarningBox("Nije unet broj osoba.");
                warningprozor.ShowDialog();
                return;
            }
            DateTime vreme = DateTime.Now;
            if (IzabranoVreme == null) IzabranoVreme = DateTime.Now.ToString("HH:mm dd.MM.yyyy");
            DateTime.TryParseExact(IzabranoVreme, "HH:mm dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out vreme);
            v_SlobodniStolovi prozor = new v_SlobodniStolovi(Convert.ToInt32(BrojGostiju),vreme,Gost);
            prozor.ShowDialog();
        }
        private void PopuniPrikazSema()
        {
            foreach(re_Sema sema in Seme)
            {
                MojBorderSema brd = new MojBorderSema(sema);
                brd.MouseLeftButtonDown += PromeniSemu;
                dpSemeIDugmad.Children.Add(brd);
                DockPanel.SetDock(brd, Dock.Left);
            }
        }
        List<Line> listaHorizontalnihLinija = new List<Line>();
        private void PopuniStolove()
        {
            Canvas.SetTop(dpStolovi, visinaVremena - 1);
            foreach(Line linija in listaHorizontalnihLinija)
            {
                canvasPrikaz.Children.Remove(linija);
            }
            dpStolovi.Children.Clear();
            int stoporedu = 0;
            Stolovi = Broker.BrokerSelect.dajSesiju().vratiStolove(IzabranaSema.Sema);
            foreach(re_Sto sto in Stolovi)
            {
                Border brd = new Border();
                dpStolovi.Children.Add(brd);
                brd.Height = visinaStola;
                brd.Background = Brushes.White;
                brd.Width = sirinaStola;
                brd.Margin = new System.Windows.Thickness(0, 0, 0, 0);
                DockPanel.SetDock(brd, Dock.Top);
                if(stoporedu == 0)
                {
                    brd.BorderThickness = new System.Windows.Thickness(0, 2, 2, 0);
                } else 
                {
                    brd.BorderThickness = new System.Windows.Thickness(0, 1, 2, 0);
                }

                brd.BorderBrush = Brushes.Black;
                StackPanel sp = new StackPanel();
                brd.Child = sp;
                sp.Orientation = Orientation.Horizontal;
                sp.Margin = new System.Windows.Thickness(0, 0, 0, 0);
                sp.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                sp.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                Image img = new Image();
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri("/Resursi/Slike/Sto.PNG", UriKind.RelativeOrAbsolute);
                bi.EndInit();
                img.Margin = new System.Windows.Thickness(10, 0, 0, 0);
                img.Source = bi;
                img.Height = 20;
                img.Width = 20;
                TextBlock tb = new TextBlock();
                tb.Text = sto.ToString();
                tb.FontSize = 16;
                tb.Margin = new System.Windows.Thickness(10, 0, 0, 0);
                sp.Children.Add(img); 
                sp.Children.Add(tb);

                //dodavanje linija radi lakseg pregleda
                Line linija = new Line();
                Canvas.SetLeft(linija, sirinaStola);
                Canvas.SetTop(linija, visinaVremena);
                linija.StrokeThickness = 2;
                linija.Stroke = Brushes.Black;
                linija.Opacity = 0.05;
                double[] dbl = new double[] { 4, 0 };
                linija.StrokeDashArray = new DoubleCollection(dbl);
                linija.Y1 = visinaStola * stoporedu +1; //+1 da bi izravnao sa borderom
                linija.Y2 = visinaStola * stoporedu +1;
                stoporedu++;
                linija.X1 = 0;
                linija.X2 = 5000;
                listaHorizontalnihLinija.Add(linija);
                canvasPrikaz.Children.Add(linija);

            }
            canvasPrikaz.Height = visinaStola * Stolovi.Count;
        }
        private void PromeniSemu(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MojBorderSema brd = sender as MojBorderSema;
            foreach(MojBorderSema brd1 in dpSemeIDugmad.Children.OfType<MojBorderSema>())
            {
                brd1.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#284b63");
            }
            brd.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#427aa1");
            if (brd != null)
            {
                IzabranaSema = brd.Sema;               
            }
        }
        private void PopuniVremena()
        {
            Canvas.SetLeft(dpVremena, sirinaStola - sirinaVremena / 2);
            DateTime dt = DateTime.Today;
            DateTime dtprovera = DateTime.Today;
            dt = dt.AddHours(7); //odakle krece dan
            do
            {
                //pravljenje i dodavanje vremena
                Border brd = new Border();
                brd.Background = Brushes.White;
                brd.Width = sirinaVremena;
                brd.Height = visinaVremena;
                brd.BorderThickness = new System.Windows.Thickness(0,0,0,1);
                brd.BorderBrush = Brushes.Black;
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Vertical;
                TextBlock tb = new TextBlock();
                brd.Child = sp;
                sp.Margin = new System.Windows.Thickness(0, 5, 0, 0);
                sp.Children.Add(tb);
                Rectangle rec = new Rectangle();
                rec.Width = 2;
                rec.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                rec.Height = 15;
                rec.Fill = Brushes.Black;
                sp.Children.Add(rec);
                tb.Text = dt.ToString("HH:mm");
                tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                dt = dt.AddMinutes(30);
                tb.FontSize = 16;
                dpVremena.Children.Add(brd);
                DockPanel.SetDock(tb, Dock.Left);

                //dodavanje linija radi lakseg pregleda
                Line linija = new Line();
                linija.Stroke = Brushes.Black;
                linija.Opacity = 0.1;
                double[] dbl = new double[] { 6, 6 };
                linija.StrokeDashArray = new DoubleCollection(dbl);
                linija.Y1 = 0;
                linija.Y2 = 5000;
                linija.X1 = sirinaVremena*brojodeljka;
                linija.X2 = sirinaVremena * brojodeljka;
                canvasPrikaz.Children.Add(linija);
                Canvas.SetLeft(linija, sirinaStola);
                Canvas.SetTop(linija, visinaVremena);
                brojodeljka++;
            } while (dt.Day == dtprovera.Day);
            //hocu da mi vreme pocinje od 00:00 i zavrsava se sa 00:00
            Border brd1 = new Border();
            brd1.Background = Brushes.White;
            brd1.Width = sirinaVremena;
            brd1.Height = visinaVremena;
            brd1.BorderBrush = Brushes.Black;
            brd1.BorderThickness = new System.Windows.Thickness(0, 0, 0, 1);
            StackPanel sp1 = new StackPanel();
            sp1.Orientation = Orientation.Vertical;
            TextBlock tb1 = new TextBlock();
            brd1.Child = sp1;
            sp1.Margin = new System.Windows.Thickness(0, 5, 0, 0);
            sp1.Children.Add(tb1);
            Rectangle rec1 = new Rectangle();
            rec1.Width = 2;
            rec1.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            rec1.Height = 15;
            rec1.Fill = Brushes.Black;
            sp1.Children.Add(rec1);
            tb1.Text = dt.ToString("HH:mm");
            tb1.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            tb1.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            dt = dt.AddMinutes(30);
            tb1.FontSize = 16;
            dpVremena.Children.Add(brd1);
            DockPanel.SetDock(tb1, Dock.Left);


            Line linija2 = new Line();
            linija2.Stroke = Brushes.Black;
            linija2.Opacity = 0.1;
            double[] dbl2 = new double[] { 6, 6 };
            linija2.StrokeDashArray = new DoubleCollection(dbl2);
            linija2.Y1 = 0;
            linija2.Y2 = 5000;
            linija2.X1 = sirinaVremena * brojodeljka;
            linija2.X2 = sirinaVremena * brojodeljka;
            canvasPrikaz.Children.Add(linija2);
            Canvas.SetLeft(linija2, sirinaStola);
            Canvas.SetTop(linija2, visinaVremena);
            brojodeljka++;
            SirinaCanvasa =  brojodeljka  * sirinaVremena + 30;
        }

        static v_Meni meni = new v_Meni("-","-");

        //pozicija misa pri kliku
        double mouseX;
        double mouseY;
        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }
        private void OtvoriRadniMeni_Metoda(object sender)
        {
            double mousey = Mouse.GetPosition(canvasPrikaz).X; //treba mi X koordinata da odredim koje je vreme
            double mousex = Mouse.GetPosition(canvasPrikaz).Y; //treba mi Y koordinata da odredim koji je sto

            mouseX = mousex;
            mouseY = mousey;

            //odredjujem vreme
            int vreme = Convert.ToInt32(Math.Floor((mousey - sirinaStola) / sirinaVremena));
            DateTime dt = DateTime.Today;
            dt = dt.AddHours(7);
            dt = dt.AddMinutes(vreme * 30);

            //odredjujem sto
            int pozicijasto = Convert.ToInt32(Math.Floor((mousex - visinaVremena) / visinaStola));
            re_Sto sto = Stolovi[pozicijasto];
            if (IsWindowOpen<v_Meni>())
            {
                meni.Close();
            }
            meni = new v_Meni(dt.ToString("HH:mm"), sto.Sto);
            if (meni.ActionNovaRezervacija == null)
            {
                meni.ActionNovaRezervacija = new Action(NovaRezervacija);
            }
            meni.Left = Mouse.GetPosition(canvasPrikaz).X - 170 - svCanvas.HorizontalOffset;
            meni.Top = Mouse.GetPosition(canvasPrikaz).Y - svCanvas.VerticalOffset;
            meni.Show();
        }
        public void NovaRezervacija()
        {
            meni.Close();

            int vreme = Convert.ToInt32(Math.Floor((mouseY - sirinaStola) / sirinaVremena));
            int pozicijasto = Convert.ToInt32(Math.Floor((mouseX - visinaVremena) / visinaStola));


            DateTime dt = DateTime.Today;
            dt = dt.AddHours(7);
            dt = dt.AddMinutes(vreme * 30);
            re_Sto sto = Stolovi[pozicijasto];
            re_Rezervacija novarezervacija = new re_Rezervacija();
            novarezervacija.Datum = IzabranDatum;
            novarezervacija.VremeOd = dt;
            novarezervacija.VremeDo = dt.AddMinutes(120);
            novarezervacija.IdGost = "0";
            novarezervacija.Objekat = Globalno.Varijable.Objekat.Objekat;
            novarezervacija.Sema = IzabranaSema.Sema;
            novarezervacija.Sto = sto.Sto;
            novarezervacija.BrojOdraslih = 2;
            novarezervacija.BrojDece = 0;
            bool upsesno = Broker.BrokerInsert.dajSesiju().UpisiRezervaciju(novarezervacija);

            OsveziRadniProstor();

            //uc_Rezervacija brdRez = new uc_Rezervacija(novarezervacija);
            //brdRez.Width = 240;

            //canvasPrikaz.Children.Add(brdRez);
            //BrdRezervacije.Add(brdRez);

            //Canvas.SetLeft(brdRez, vreme * sirinaVremena + sirinaStola);
            //Canvas.SetTop(brdRez, pozicijasto * visinaStola + visinaVremena);
        }
        private void Zatvori_Metoda(object obj)
        {
            if (meni != null)
            {
                meni.Close();
            }
            v_Login prozor = new v_Login();
            prozor.Show();
            this.ZatvoriFormu();
        }
    }
}
