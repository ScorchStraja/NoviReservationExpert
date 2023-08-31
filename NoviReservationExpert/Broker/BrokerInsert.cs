using NoviReservationExpert.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoviReservationExpert.Broker
{
    internal class BrokerInsert
    {
        public static BrokerInsert instance;
        public static BrokerInsert dajSesiju()
        {
            if (instance == null)
            {
                instance = new BrokerInsert();
            }
            return instance;
        }

        public bool UpisiRezervaciju(re_Rezervacija novarezervacija)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();
                    string upit = "INSERT into [REZERVACIJA] " +
                    "(Dtm, Vreme_Od, Vreme_Do,Gost,Sto, Obj, Sema) " +
                    "values " +
                    "(@dtm,@vreme_od,@vreme_do,@gost,@sto, @obj, @sema)";

                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.CommandText = upit;

                        komanda.Parameters.AddWithValue("@dtm", novarezervacija.Datum);
                        komanda.Parameters.AddWithValue("@vreme_od", novarezervacija.VremeOd);
                        komanda.Parameters.AddWithValue("@vreme_do", novarezervacija.VremeDo);
                        komanda.Parameters.AddWithValue("@gost", novarezervacija.IdGost);
                        komanda.Parameters.AddWithValue("@sto", novarezervacija.Sto);
                        komanda.Parameters.AddWithValue("@obj", Globalno.Varijable.Objekat.Objekat);
                        komanda.Parameters.AddWithValue("@sema", novarezervacija.Sema);

                        int rezultat = komanda.ExecuteNonQuery();
                        if (rezultat < 0)
                        { 
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool NapraviNovogGosta(string gost, string telefon)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();
                    string upit = "SELECT MAX(ID_GST) as id FROM KS_GOSTI";
                    int maxid = -1;
                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        SqlDataReader reader = komanda.ExecuteReader();
                        while (reader.Read())
                        {
                            maxid = Convert.ToInt32(reader["id"]);
                        }
                    }
                    if (maxid < 0) return false;

                    upit = "INSERT INTO KS_GOSTI (ID_GST, TEL, IME, PREZIME) " +
                        "VALUES(@maxid, @tel, @ime,@prezime)";
                    string ime = "";
                    string prezime = "";
                    if(gost.Contains(" "))
                    {
                        ime = gost.Substring(0, gost.IndexOf(" "));
                        int duzina = gost.Length - ime.Length;
                        prezime = gost.Substring(gost.IndexOf(" "), duzina);
                    } else
                    {
                        ime = gost;
                        prezime = "N.";
                    }
                    maxid = maxid + 1;

                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.CommandText = upit;

                        komanda.Parameters.AddWithValue("@maxid", maxid);
                        komanda.Parameters.AddWithValue("@tel", telefon);
                        komanda.Parameters.AddWithValue("@ime", ime);
                        komanda.Parameters.AddWithValue("@prezime", prezime);

                        int rezultat = komanda.ExecuteNonQuery();
                        if (rezultat < 0)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
