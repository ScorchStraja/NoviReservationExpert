using System;
using System.Data.SqlClient;

namespace NoviReservationExpert.Broker
{
    public class DBBroker
    {

        public static string konekcioniString;

        public static DBBroker instanca;

        public static DBBroker dajSesiju()
        {
            if (instanca == null)
            {
                instanca = new DBBroker();
            }
            return instanca;
        }

        public void kreirajKonekcioniString() 
        {

            konekcioniString = konekcioniString = $@"Data Source={Globalno.Varijable.RadniServer};Initial Catalog={Globalno.Varijable.RadnaBaza};MultipleActiveResultSets=true;Persist Security Info=True;User ID=ProSoftSistem;Password=ProSoft123!@#";
        }

        public bool proveriKonekciju()
        {
            SqlConnection konekcija = new SqlConnection();
            try
            {

                konekcija = new SqlConnection(konekcioniString);

                konekcija.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally 
            {
                konekcija.Close();
            }
        }
    }
}
