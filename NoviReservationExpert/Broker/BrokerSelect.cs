using NoviReservationExpert.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace NoviReservationExpert.Broker
{
    public class BrokerSelect
    {
        public static BrokerSelect instance;
        public static BrokerSelect dajSesiju()
        {
            if (instance == null)
            {
                instance = new BrokerSelect();
            }
            return instance;
        }

        public ObservableCollection<re_Objekat> vratiObjektePoSifri(int sifra = 0)
        {
            try
            {
                ObservableCollection<re_Objekat> lista = new ObservableCollection<re_Objekat>();

                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();
                    string dodatniUslov = "";

                    string upit = "select * from [RB-MAGACIN] where mag = @mag and ind_mag=2 " + dodatniUslov + " order by mag";
                    if (sifra == 0) { upit = "select * from [RB-MAGACIN] where ind_mag=2 " + dodatniUslov + " order by mag"; }

                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.Parameters.AddWithValue("@sifra", sifra);

                        SqlDataReader reader = komanda.ExecuteReader();
                        while (reader.Read())
                        {
                            re_Objekat x = new re_Objekat
                            {
                                Objekat = Convert.ToInt32(reader["mag"]),
                                Naziv = reader["naz"].ToString() ?? "-",
                                IndMag = Convert.ToInt32(reader["ind_mag"]),
                                Korisnik = reader["korisnik"].ToString() ?? "-",
                                Zakljucano = Convert.ToInt32(reader["zakljucano"]),
                                Mar = Convert.ToInt32(reader["mar"]),
                                Mesto = reader["mes_obj"].ToString() ?? "-",
                                Adresa = reader["adr_obj"].ToString() ?? "-"
                            };
                            lista.Add(x);
                        }
                    }
                }
                return lista;
            }
            catch (Exception)
            {
                return new ObservableCollection<re_Objekat>();
            }
        }
        public ObservableCollection<re_Korisnik> vratiOperateraPoSifri(string sifra)
        {
            try
            {
                ObservableCollection<re_Korisnik> lista = new ObservableCollection<re_Korisnik>();            
                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();
                    string upit = "select * from XX_SISKRS_CLN where krs_sid like @sifra";

                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.Parameters.AddWithValue("@sifra", sifra);

                        SqlDataReader reader = komanda.ExecuteReader();
                        while (reader.Read())
                        {
                            re_Korisnik x = new re_Korisnik()
                            {
                                Id = Convert.ToInt32(reader["KRS_ID"]),
                                IdGrupa = Convert.ToInt32(reader["GRP_ID"]),
                                SID = reader["KRS_SID"].ToString() ?? "-",
                                LogOnIme = reader["KRS_LOGON"].ToString() ?? "-",
                                Ime = reader["KRS_IME"].ToString() ?? "-",
                                Prezime = reader["KRS_PRE"].ToString() ?? "-"
                            };
                            lista.Add(x);
                        }
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                return new ObservableCollection<re_Korisnik>();
            }
        }
        public ObservableCollection<re_Sema> vratiSeme()
        {
            try
            {
                ObservableCollection<re_Sema> lista = new ObservableCollection<re_Sema>();

                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();
                    string upit = "select * from KS_SEME_ZG where OBJ=@obj";

                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.Parameters.AddWithValue("@obj", Globalno.Varijable.Objekat.Objekat);

                        SqlDataReader reader = komanda.ExecuteReader();
                        while (reader.Read())
                        {
                            re_Sema x = new re_Sema()
                            {
                                Objekat = Convert.ToInt32(reader["OBJ"]),
                                Sema = reader["Sema"].ToString() ?? "-",
                                Odabran = Convert.ToInt32(reader["ODABRAN"]),
                                Napomena = reader["NAP"].ToString() ?? "-",
                                Slika = reader["SLIKA_PATH"].ToString() ?? "-"
                            };
                            lista.Add(x);
                        }
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                return new ObservableCollection<re_Sema>();
            }
        }
        public ObservableCollection<re_Sto> vratiStolove(string sema)
        {
            try
            {
                ObservableCollection<re_Sto> lista = new ObservableCollection<re_Sto>();

                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();

                    string upit = "select * from KS_SEME_PR where OBJ=@obj and SEMA=@sema ORDER BY STO ASC";


                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.Parameters.AddWithValue("@obj", Globalno.Varijable.Objekat.Objekat);
                        komanda.Parameters.AddWithValue("@sema", sema);

                        SqlDataReader reader = komanda.ExecuteReader();
                        while (reader.Read())
                        {
                            re_Sto x = new re_Sto()
                            {
                                Objekat = Convert.ToInt32(reader["OBJ"]),
                                Sema = reader["SEMA"].ToString() ?? "-",
                                Sto = reader["STO"].ToString() ?? "-",
                                PozicijaTop = Convert.ToInt32(reader["P_TOP"]),
                                PozicijaLeft = Convert.ToInt32(reader["P_LEFT"]),
                                Sirina = Convert.ToInt32(reader["P_WIDTH"]),
                                Visina = Convert.ToInt32(reader["P_HEIGHT"]),
                                Oblik = Convert.ToInt32(reader["OBLIK"])
                            };
                            lista.Add(x);
                        }
                    }
                    foreach (re_Sto sto in lista)
                    {
                        upit = "SELECT brOsoba FROM KS_STOLOVI WHERE OBJ=@obj and STO=@sto";
                        using (SqlCommand komanda = new SqlCommand(upit, connection))
                        {
                            komanda.Parameters.AddWithValue("@obj", Globalno.Varijable.Objekat.Objekat);
                            komanda.Parameters.AddWithValue("@sto", sto.Sto);

                            SqlDataReader reader = komanda.ExecuteReader();
                            while (reader.Read())
                            {
                                sto.BrojOsoba = Convert.ToInt32(reader["brOsoba"]);
                            }
                        }
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                return new ObservableCollection<re_Sto>();
            }
        }
        public re_Sto VratiSto(string sema, string sto)
        {
            try
            {
                re_Sto resto = new re_Sto();
                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();

                    string upit = "select * from KS_SEME_PR where OBJ=@obj and SEMA=@sema and STO=@sto ORDER BY STO ASC";


                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.Parameters.AddWithValue("@obj", Globalno.Varijable.Objekat.Objekat);
                        komanda.Parameters.AddWithValue("@sema", sema);
                        komanda.Parameters.AddWithValue("@sto", sto);

                        SqlDataReader reader = komanda.ExecuteReader();
                        while (reader.Read())
                        {
                            resto = new re_Sto()
                            {
                                Objekat = Convert.ToInt32(reader["OBJ"]),
                                Sema = reader["SEMA"].ToString() ?? "-",
                                Sto = reader["STO"].ToString() ?? "-",
                                PozicijaTop = Convert.ToInt32(reader["P_TOP"]),
                                PozicijaLeft = Convert.ToInt32(reader["P_LEFT"]),
                                Sirina = Convert.ToInt32(reader["P_WIDTH"]),
                                Visina = Convert.ToInt32(reader["P_HEIGHT"]),
                                Oblik = Convert.ToInt32(reader["OBLIK"])
                            };
                        }
                    }

                    upit = "SELECT brOsoba FROM KS_STOLOVI WHERE OBJ=@obj and STO=@sto";
                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.Parameters.AddWithValue("@obj", Globalno.Varijable.Objekat.Objekat);
                        komanda.Parameters.AddWithValue("@sto", resto.Sto);

                        SqlDataReader reader = komanda.ExecuteReader();
                        while (reader.Read())
                        {
                            resto.BrojOsoba = Convert.ToInt32(reader["brOsoba"]);
                        }
                    }
                }
                return resto;
            }
            catch (Exception ex)
            {
                return new re_Sto();
            }
        }
        public ObservableCollection<re_Rezervacija> VratiRezervacijeZaDatum(DateTime datum, int obj, string sema)
        {
            try
            {
                ObservableCollection<re_Rezervacija> lista = new ObservableCollection<re_Rezervacija>();

                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();

                    string upit = "SELECT * FROM REZERVACIJA INNER JOIN KS_GOSTI ON REZERVACIJA.Gost = KS_GOSTI.ID_GST WHERE Dtm=@datum AND OBJ=@obj AND SEMA=@sema ORDER BY STO ASC, VREME_OD DESC, ID DESC";
                    if(datum == new DateTime())
                    {
                        upit = "SELECT * FROM REZERVACIJA INNER JOIN KS_GOSTI ON REZERVACIJA.Gost = KS_GOSTI.ID_GST WHERE OBJ=@obj and SEMA=@sema ORDER BY VREME_OD ASC";
                        datum = datum.AddYears(1800);
                    }
                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.Parameters.AddWithValue("@datum", datum);
                        komanda.Parameters.AddWithValue("@obj", obj);
                        komanda.Parameters.AddWithValue("@sema", sema);

                        SqlDataReader reader = komanda.ExecuteReader();
                        while (reader.Read())
                        {
                            re_Rezervacija x = new re_Rezervacija()
                            {
                                Id = reader["id"] == DBNull.Value ? 1 : Convert.ToInt32(reader["id"]),
                                Datum = reader["Dtm"] == DBNull.Value ? DateTime.Today : reader["Dtm"] as DateTime? ?? DateTime.Now,
                                VremeOd = reader["Vreme_Od"] == DBNull.Value ? DateTime.Now : reader["Vreme_Od"] as DateTime? ?? DateTime.Now,
                                VremeDo = reader["Vreme_Do"] == DBNull.Value ? DateTime.Now.AddMinutes(120) : reader["Vreme_Do"] as DateTime? ?? DateTime.Now,
                                Objekat = reader["Obj"] == DBNull.Value ? Globalno.Varijable.Objekat.Objekat : Convert.ToInt32(reader["Obj"]),
                                Sema = reader["Sema"] == DBNull.Value ? "-" : reader["Sema"].ToString() ?? "-",
                                Grupa = reader["Grupa"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Grupa"]),
                                Napomena = reader["Napomena"] == DBNull.Value ? "-" : reader["Napomena"].ToString() ?? "-",
                                Kuhinja = reader["Kuhinja"] == DBNull.Value ? "-" : reader["Kuhinja"].ToString() ?? "-",
                                IdGost = reader["Gost"] == DBNull.Value ? "0" : reader["Gost"].ToString() ?? "-",
                                Sto = reader["Sto"] == DBNull.Value ? "0" : reader["Sto"].ToString() ?? "-",
                                Status = reader["Status"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Status"]),
                                BrojOdraslih = reader["Br_Odraslih"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Br_Odraslih"]),
                                BrojDece = reader["Br_Dece"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Br_Dece"]),
                                Korisnik = reader["Korisnik"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Korisnik"]),
                                Obrisano = reader["Obrisano"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Obrisano"]),
                                ImeGosta = reader["Ime"] == DBNull.Value ? "N.N." : reader["Ime"].ToString() ?? "-",
                                PrezimeGosta = reader["Prezime"] == DBNull.Value ? "N." : reader["Prezime"].ToString() ?? "-",
                                BrojTelefona = reader["TEL"] == DBNull.Value ? "-" : reader["TEL"].ToString() ?? "-"
                            };

                            lista.Add(x);
                        }
                    }
                }
                return lista;
            }
            catch
            {
                return new ObservableCollection<re_Rezervacija>();
            }
        }
        public ObservableCollection<re_Rezervacija> VratiRezervacijeKojeSePoklapajuSaVremenom(DateTime vreme, DateTime vremeTrajanja, DateTime datum, int idrez=-1)
        {
            try
            {
                ObservableCollection<re_Rezervacija> lista = new ObservableCollection<re_Rezervacija>();

                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();

                    string upit = "SELECT * FROM REZERVACIJA INNER JOIN KS_GOSTI ON REZERVACIJA.Gost = KS_GOSTI.ID_GST WHERE Vreme_Do > @vreme AND Vreme_Od < @vremetrajanja AND Dtm =@vremedtm and id != @id";
                    if(idrez == -1)
                    {
                        upit = "SELECT * FROM REZERVACIJA INNER JOIN KS_GOSTI ON REZERVACIJA.Gost = KS_GOSTI.ID_GST WHERE Vreme_Do > @vreme AND Vreme_Od < @vremetrajanja AND Dtm =@vremedtm";
                    }
                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.Parameters.AddWithValue("@vreme", vreme);
                        komanda.Parameters.AddWithValue("@vremetrajanja", vremeTrajanja);
                        komanda.Parameters.AddWithValue("@vremedtm", datum);
                        komanda.Parameters.AddWithValue("@id", idrez);

                        SqlDataReader reader = komanda.ExecuteReader();
                        while (reader.Read())
                        {
                            re_Rezervacija x = new re_Rezervacija()
                            {
                                Id = reader["id"] == DBNull.Value ? 1 : Convert.ToInt32(reader["id"]),
                                Datum = reader["Dtm"] == DBNull.Value ? DateTime.Today : reader["Dtm"] as DateTime? ?? DateTime.Now,
                                VremeOd = reader["Vreme_Od"] == DBNull.Value ? DateTime.Now : reader["Vreme_Od"] as DateTime? ?? DateTime.Now,
                                VremeDo = reader["Vreme_Do"] == DBNull.Value ? DateTime.Now.AddMinutes(120) : reader["Vreme_Do"] as DateTime? ?? DateTime.Now,
                                Objekat = reader["Obj"] == DBNull.Value ? Globalno.Varijable.Objekat.Objekat : Convert.ToInt32(reader["Obj"]),
                                Sema = reader["Sema"] == DBNull.Value ? "-" : reader["Sema"].ToString() ?? "-",
                                Grupa = reader["Grupa"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Grupa"]),
                                Napomena = reader["Napomena"] == DBNull.Value ? "-" : reader["Napomena"].ToString() ?? "-",
                                Kuhinja = reader["Kuhinja"] == DBNull.Value ? "-" : reader["Kuhinja"].ToString() ?? "-",
                                IdGost = reader["Gost"] == DBNull.Value ? "0" : reader["Gost"].ToString() ?? "-",
                                Sto = reader["Sto"] == DBNull.Value ? "0" : reader["Sto"].ToString() ?? "-",
                                Status = reader["Status"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Status"]),
                                BrojOdraslih = reader["Br_Odraslih"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Br_Odraslih"]),
                                BrojDece = reader["Br_Dece"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Br_Dece"]),
                                Korisnik = reader["Korisnik"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Korisnik"]),
                                Obrisano = reader["Obrisano"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Obrisano"]),
                                ImeGosta = reader["Ime"] == DBNull.Value ? "N.N." : reader["Ime"].ToString() ?? "-",
                                PrezimeGosta = reader["Prezime"] == DBNull.Value ? "N." : reader["Prezime"].ToString() ?? "-",
                                BrojTelefona = reader["TEL"] == DBNull.Value ? "-" : reader["TEL"].ToString() ?? "-"

                            };

                            lista.Add(x);
                        }
                    }
                }
                return lista;
            }
            catch
            {
                return new ObservableCollection<re_Rezervacija>();
            }
        }
        public ObservableCollection<re_Rezervacija> PostojiPoklapanje(DateTime vreme, DateTime vremetrajanja, int idrezervacije)
        {
            try
            {
                ObservableCollection<re_Rezervacija> lista = new ObservableCollection<re_Rezervacija>();

                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();

                    string upit = "SELECT * FROM REZERVACIJA INNER JOIN KS_GOSTI ON REZERVACIJA.Gost = KS_GOSTI.ID_GST " +
                        "WHERE " +
                        "(@vremeod >= Vreme_Od and @vremeod < Vreme_Do) " +
                        "and Dtm=@datum and Id != @id ";

                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.Parameters.AddWithValue("@vremeod", vreme);
                        komanda.Parameters.AddWithValue("@datum", vreme.Date);
                        komanda.Parameters.AddWithValue("@id", idrezervacije);

                        SqlDataReader reader = komanda.ExecuteReader();
                        while (reader.Read())
                        {
                            re_Rezervacija x = new re_Rezervacija()
                            {
                                Id = reader["id"] == DBNull.Value ? 1 : Convert.ToInt32(reader["id"]),
                                Datum = reader["Dtm"] == DBNull.Value ? DateTime.Today : reader["Dtm"] as DateTime? ?? DateTime.Now,
                                VremeOd = reader["Vreme_Od"] == DBNull.Value ? DateTime.Now : reader["Vreme_Od"] as DateTime? ?? DateTime.Now,
                                VremeDo = reader["Vreme_Do"] == DBNull.Value ? DateTime.Now.AddMinutes(120) : reader["Vreme_Do"] as DateTime? ?? DateTime.Now,
                                Objekat = reader["Obj"] == DBNull.Value ? Globalno.Varijable.Objekat.Objekat : Convert.ToInt32(reader["Obj"]),
                                Sema = reader["Sema"] == DBNull.Value ? "-" : reader["Sema"].ToString() ?? "-",
                                Grupa = reader["Grupa"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Grupa"]),
                                Napomena = reader["Napomena"] == DBNull.Value ? "-" : reader["Napomena"].ToString() ?? "-",
                                Kuhinja = reader["Kuhinja"] == DBNull.Value ? "-" : reader["Kuhinja"].ToString() ?? "-",
                                IdGost = reader["Gost"] == DBNull.Value ? "0" : reader["Gost"].ToString() ?? "-",
                                Sto = reader["Sto"] == DBNull.Value ? "0" : reader["Sto"].ToString() ?? "-",
                                Status = reader["Status"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Status"]),
                                BrojOdraslih = reader["Br_Odraslih"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Br_Odraslih"]),
                                BrojDece = reader["Br_Dece"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Br_Dece"]),
                                Korisnik = reader["Korisnik"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Korisnik"]),
                                Obrisano = reader["Obrisano"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Obrisano"]),
                                ImeGosta = reader["Ime"] == DBNull.Value ? "N.N." : reader["Ime"].ToString() ?? "-",
                                PrezimeGosta = reader["Prezime"] == DBNull.Value ? "N." : reader["Prezime"].ToString() ?? "-",
                                BrojTelefona = reader["TEL"] == DBNull.Value ? "-" : reader["TEL"].ToString() ?? "-"

                            };
                            lista.Add(x);
                        }
                    }
                }
                return lista;
            }
            catch
            {
                return new ObservableCollection<re_Rezervacija>();
            }
        }
        public re_Rezervacija VratiRezervacijuPoId(int id)
        {
            try
            {
                re_Rezervacija x = new re_Rezervacija();
                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();

                    string upit = "SELECT * FROM REZERVACIJA INNER JOIN KS_GOSTI ON REZERVACIJA.Gost = KS_GOSTI.ID_GST WHERE Id=@id";

                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.Parameters.AddWithValue("@Id", id);

                        SqlDataReader reader = komanda.ExecuteReader();
                        while (reader.Read())
                        {
                            x = new re_Rezervacija()
                            {
                                Id = reader["id"] == DBNull.Value ? 1 : Convert.ToInt32(reader["id"]),
                                Datum = reader["Dtm"] == DBNull.Value ? DateTime.Today : reader["Dtm"] as DateTime? ?? DateTime.Now,
                                VremeOd = reader["Vreme_Od"] == DBNull.Value ? DateTime.Now : reader["Vreme_Od"] as DateTime? ?? DateTime.Now,
                                VremeDo = reader["Vreme_Do"] == DBNull.Value ? DateTime.Now.AddMinutes(120) : reader["Vreme_Do"] as DateTime? ?? DateTime.Now,
                                Objekat = reader["Obj"] == DBNull.Value ? Globalno.Varijable.Objekat.Objekat : Convert.ToInt32(reader["Obj"]),
                                Sema = reader["Sema"] == DBNull.Value ? "-" : reader["Sema"].ToString() ?? "-",
                                Grupa = reader["Grupa"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Grupa"]),
                                Napomena = reader["Napomena"] == DBNull.Value ? "-" : reader["Napomena"].ToString() ?? "-",
                                Kuhinja = reader["Kuhinja"] == DBNull.Value ? "-" : reader["Kuhinja"].ToString() ?? "-",
                                IdGost = reader["Gost"] == DBNull.Value ? "0" : reader["Gost"].ToString() ?? "-",
                                Sto = reader["Sto"] == DBNull.Value ? "0" : reader["Sto"].ToString() ?? "-",
                                Status = reader["Status"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Status"]),
                                BrojOdraslih = reader["Br_Odraslih"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Br_Odraslih"]),
                                BrojDece = reader["Br_Dece"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Br_Dece"]),
                                Korisnik = reader["Korisnik"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Korisnik"]),
                                Obrisano = reader["Obrisano"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Obrisano"]),
                                ImeGosta = reader["Ime"] == DBNull.Value ? "N.N." : reader["Ime"].ToString() ?? "-",
                                PrezimeGosta = reader["Prezime"] == DBNull.Value ? "N." : reader["Prezime"].ToString() ?? "-",
                                BrojTelefona = reader["TEL"] == DBNull.Value ? "-" : reader["TEL"].ToString() ?? "-"

                            };

                        }
                    }
                }
                return x;
            }
            catch
            {
                return new re_Rezervacija();
            }
        }
        public ObservableCollection<re_Rezervacija> VratiNotifikacije(DateTime vreme, DateTime izabranDatum)
        {
            try
            {
                ObservableCollection<re_Rezervacija> lista = new ObservableCollection<re_Rezervacija>();

                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();

                    string upit = "SELECT * FROM REZERVACIJA INNER JOIN KS_GOSTI ON REZERVACIJA.Gost = KS_GOSTI.ID_GST WHERE Vreme_Od < @vreme AND Dtm=@datum";

                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.Parameters.AddWithValue("@vreme", vreme.AddMinutes(30));
                        komanda.Parameters.AddWithValue("@datum", izabranDatum.Date);

                        SqlDataReader reader = komanda.ExecuteReader();
                        while (reader.Read())
                        {
                            re_Rezervacija x = new re_Rezervacija()
                            {
                                Id = reader["id"] == DBNull.Value ? 1 : Convert.ToInt32(reader["id"]),
                                Datum = reader["Dtm"] == DBNull.Value ? DateTime.Today : reader["Dtm"] as DateTime? ?? DateTime.Now,
                                VremeOd = reader["Vreme_Od"] == DBNull.Value ? DateTime.Now : reader["Vreme_Od"] as DateTime? ?? DateTime.Now,
                                VremeDo = reader["Vreme_Do"] == DBNull.Value ? DateTime.Now.AddMinutes(120) : reader["Vreme_Do"] as DateTime? ?? DateTime.Now,
                                Objekat = reader["Obj"] == DBNull.Value ? Globalno.Varijable.Objekat.Objekat : Convert.ToInt32(reader["Obj"]),
                                Sema = reader["Sema"] == DBNull.Value ? "-" : reader["Sema"].ToString() ?? "-",
                                Grupa = reader["Grupa"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Grupa"]),
                                Napomena = reader["Napomena"] == DBNull.Value ? "-" : reader["Napomena"].ToString() ?? "-",
                                Kuhinja = reader["Kuhinja"] == DBNull.Value ? "-" : reader["Kuhinja"].ToString() ?? "-",
                                IdGost = reader["Gost"] == DBNull.Value ? "0" : reader["Gost"].ToString() ?? "-",
                                Sto = reader["Sto"] == DBNull.Value ? "0" : reader["Sto"].ToString() ?? "-",
                                Status = reader["Status"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Status"]),
                                BrojOdraslih = reader["Br_Odraslih"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Br_Odraslih"]),
                                BrojDece = reader["Br_Dece"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Br_Dece"]),
                                Korisnik = reader["Korisnik"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Korisnik"]),
                                Obrisano = reader["Obrisano"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Obrisano"]),
                                ImeGosta = reader["Ime"] == DBNull.Value ? "N.N." : reader["Ime"].ToString() ?? "-",
                                PrezimeGosta = reader["Prezime"] == DBNull.Value ? "N." : reader["Prezime"].ToString() ?? "-",
                                BrojTelefona = reader["TEL"] == DBNull.Value ? "-" : reader["TEL"].ToString() ?? "-"
                            };

                            lista.Add(x);
                        }
                    }
                }
                return lista;
            }
            catch
            {
                return new ObservableCollection<re_Rezervacija>();
            }
        }
        public ObservableCollection<re_Gost> VratiGoste()
        {
            try
            {
                ObservableCollection<re_Gost> lista = new ObservableCollection<re_Gost>();

                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();

                    string upit = "select * from KS_GOSTI ORDER BY ID_GST ASC";


                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        SqlDataReader reader = komanda.ExecuteReader();
                        while (reader.Read())
                        {
                            re_Gost x = new re_Gost()
                            {
                                ID= reader["ID_GST"].ToString(),
                                NazivInt= reader["NAZIV_INT"].ToString(),
                                NazivExt= reader["NAZIV_EXT"].ToString(),
                                SifKD= Convert.ToInt32(reader["SIF_KD"]),
                                Aktivna= Convert.ToInt32(reader["AKTIVNA"]),
                                Telefon= reader["TEL"].ToString(),
                                Email= reader["EMAIL"].ToString(),
                                Ime= reader["IME"].ToString(),
                                Prezime= reader["PREZIME"].ToString(),
                                Grad= reader["GRAD"].ToString()
                            };
                            lista.Add(x);
                        }
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                return new ObservableCollection<re_Gost>();
            }
        }
        public bool ProveraPostojanjaGosta(string telefon)
        {
            try
            {
                ObservableCollection<re_Gost> lista = new ObservableCollection<re_Gost>();

                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();

                    string upit = "select * from KS_GOSTI WHERE TEL = @tel ORDER BY ID_GST ASC";

                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        SqlDataReader reader = komanda.ExecuteReader();
                        while (reader.Read())
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return true;
            }
        }
        public bool ZauzetostStola(re_Sto sto)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();

                    string upit = "SELECT * FROM KS_BLK_ZG WHERE IND_KNJ = 0 AND STO=@sto and OBJ=@obj";


                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.Parameters.AddWithValue("@sto", sto.Sto);
                        komanda.Parameters.AddWithValue("@obj", Globalno.Varijable.Objekat.Objekat);

                        SqlDataReader reader = komanda.ExecuteReader();

                        while (reader.Read())
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
