using NoviReservationExpert.Model;
using NoviReservationExpert.View;
using NoviReservationExpert.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Globalno
{
    public static class Varijable
    {
        private static string radniServer;
        private static string radnaBaza;

        public static string RadniServer { get => radniServer; set => radniServer = value; }
        public static string RadnaBaza { get => radnaBaza; set => radnaBaza = value; }

        private static v_Login _loginProzor;
        public static v_Login LoginProzor { get => _loginProzor; set => _loginProzor = value; }

        public static vm_RadniProstor _RadniProstor;
        public static vm_RadniProstor RadniProstor { get => _RadniProstor; set => _RadniProstor = value; }

        private static bool _otvorenaTastaturaLogin;
        public static bool otvorenaTastaturaLogin { get => _otvorenaTastaturaLogin; set => _otvorenaTastaturaLogin = value; }

        public static re_Objekat _objekat;
        public static re_Objekat Objekat { get => _objekat; set => _objekat = value; }

        private static re_Korisnik _Korisnik;
        public static re_Korisnik Korisnik { get => _Korisnik; set => _Korisnik = value; }

        private static re_Sema _izabranaSema;
        public static re_Sema IzabranaSema { get => _izabranaSema; set => _izabranaSema = value; }

        private static DateTime _izabranDatum;
        public static DateTime IzabranDatum { get => _izabranDatum; set => _izabranDatum = value;}

        private static re_Gost _izabranGost;
        public static re_Gost IzabranGost { get => _izabranGost; set => _izabranGost = value;}

        private static bool _sacuvanePromene;
        public static bool sacuvanePromene { get => _sacuvanePromene; set => _sacuvanePromene = value;}
    }
}
