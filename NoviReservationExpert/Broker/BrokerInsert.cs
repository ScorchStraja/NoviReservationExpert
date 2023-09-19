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
        public void ZapisiLog(int tip = 0, string opis = "-", string modul = "-", int brerror = 0, string imeforme = "-", bool ufile = true)
        {
            re_Log log = new re_Log()
            {
                Tip = tip,
                Datum = DateTime.Today,
                Vreme = new DateTime().Add(DateTime.Now.TimeOfDay),
                Opis = opis,
                Modul = modul,
                BrError = brerror,
                TipApp = "Rezervacije",
                TipIzmene = 0,
                ImeForme = imeforme,
                Operacija = "-",
                TipObj = 0
            };

            upisiLog_Pr(log);
        }
        private bool upisiLog_Pr(re_Log log)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();
                    string upit = "INSERT INTO XX_LOG_PR (TIP, KRS_ID, DTM, VRM, OPIS, MODUL, BRERR, TIPAPP, LOGON_KRSIME, LOGON_MASIME, TIPIZM, OPERACIJA, IME_FORME, TIPOBJ) " +
                                "values " +
                                "(@tip, @korisnickiid, @datum, @vreme, @opis, @modul, @brojerror, @tipapp, @logonkorisnickoime, @logonimemasine, @tipizmene, @operacija, @imeforme, @tipobj)";

                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.Parameters.AddWithValue("@tip", log.Tip);
                        komanda.Parameters.AddWithValue("@korisnickiid", log.KorisnikId);
                        komanda.Parameters.AddWithValue("@datum", log.Datum.ToString("yyyy-MM-dd"));
                        komanda.Parameters.AddWithValue("@vreme", new DateTime(1900, 01, 01, log.Vreme.Hour, log.Vreme.Minute, log.Vreme.Second));
                        komanda.Parameters.AddWithValue("@opis", log.Opis);
                        komanda.Parameters.AddWithValue("@modul", log.Modul);
                        komanda.Parameters.AddWithValue("@brojerror", log.BrError);
                        komanda.Parameters.AddWithValue("@tipapp", log.TipApp);
                        komanda.Parameters.AddWithValue("@logonkorisnickoime", "KRSMonitoring");
                        komanda.Parameters.AddWithValue("@logonimemasine", log.LogOnMasinskoIme);
                        komanda.Parameters.AddWithValue("@tipizmene", 0);
                        komanda.Parameters.AddWithValue("@operacija", 0);
                        komanda.Parameters.AddWithValue("@imeforme", log.ImeForme);
                        komanda.Parameters.AddWithValue("@tipobj", 0);

                        int rezultat = komanda.ExecuteNonQuery();

                        if (rezultat < 0) return false;

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool UpisiParametar_PrikazivanjeOtkazanihRezervacija(string status)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();
                    string upit = "DELETE FROM KS_NPAR WHERE GRP=2000 AND OBJ=@obj AND RB=1";

                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.Parameters.AddWithValue("@obj", Globalno.Varijable.Objekat.Objekat);
                        int rezultat = komanda.ExecuteNonQuery();

                        if (rezultat < 0) return false;

                    }

                    upit = "INSERT into KS_NPAR " +
                        "(OBJ, RB, OPIS,STATUS, OPCIJE, KORISNIK, GRP) " +
                        "values " +
                        "(@obj,@rb,@opis,@status,@opcije, @korisnik, @grp)";

                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.CommandText = upit;

                        komanda.Parameters.AddWithValue("@obj", Globalno.Varijable.Objekat.Objekat);
                        komanda.Parameters.AddWithValue("@rb", 1);
                        komanda.Parameters.AddWithValue("@opis", "Prikazivanje otkazanih rezervacija.");
                        komanda.Parameters.AddWithValue("@status", status);
                        komanda.Parameters.AddWithValue("@opcije", "Da, Ne");
                        komanda.Parameters.AddWithValue("@korisnik", Globalno.Varijable.Korisnik.LogOnIme);
                        komanda.Parameters.AddWithValue("@grp", 2000);

                            ;
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
        public bool UpisiParametar_AutomatskoMenjanjeStatusa(string status)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();
                    string upit = "DELETE FROM KS_NPAR WHERE GRP=2000 AND OBJ=@obj AND RB=2";

                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.Parameters.AddWithValue("@obj", Globalno.Varijable.Objekat.Objekat);
                        int rezultat = komanda.ExecuteNonQuery();

                        if (rezultat < 0) return false;

                    }

                    upit = "INSERT into KS_NPAR " +
                        "(OBJ, RB, OPIS,STATUS, OPCIJE, KORISNIK, GRP) " +
                        "values " +
                        "(@obj,@rb,@opis,@status,@opcije, @korisnik, @grp)";

                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.CommandText = upit;

                        komanda.Parameters.AddWithValue("@obj", Globalno.Varijable.Objekat.Objekat);
                        komanda.Parameters.AddWithValue("@rb", 2);
                        komanda.Parameters.AddWithValue("@opis", "Automatsko menjanje statusa rezervacija");
                        komanda.Parameters.AddWithValue("@status", status);
                        komanda.Parameters.AddWithValue("@opcije", "Da, Ne");
                        komanda.Parameters.AddWithValue("@korisnik", Globalno.Varijable.Korisnik.LogOnIme);
                        komanda.Parameters.AddWithValue("@grp", 2000);

                        ;
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
        public bool UpisiRezervaciju(re_Rezervacija novarezervacija)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();
                    string upit = "INSERT into [REZERVACIJA] " +
                    "(Dtm, Vreme_Od, Vreme_Do,Gost,Sto, Obj, Sema, ImeGosta, PrezimeGosta, TelefonGosta) " +
                    "values " +
                    "(@dtm,@vreme_od,@vreme_do,@gost,@sto, @obj, @sema,@imegosta,@prezimegosta,@telefon)";

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
                        komanda.Parameters.AddWithValue("@imegosta", novarezervacija.ImeGosta);
                        komanda.Parameters.AddWithValue("@prezimegosta", novarezervacija.PrezimeGosta);
                        komanda.Parameters.AddWithValue("@telefon", novarezervacija.BrojTelefona)
                            ;
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
