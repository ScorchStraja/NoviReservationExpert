using NoviReservationExpert.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoviReservationExpert.Broker
{
    internal class BrokerUpdate
    {
        public static BrokerUpdate instance;
        public static BrokerUpdate dajSesiju()
        {
            if (instance == null)
            {
                instance = new BrokerUpdate();
            }
            return instance;
        }
        public bool UpdateRezervaciju(re_Rezervacija rezervacija)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();
                    string upit = "UPDATE [REZERVACIJA] SET " +
                                  "Dtm=@datum, " +
                                  "Vreme_Od=@vremeod, " +
                                  "Vreme_Do=@vremedo, " +
                                  "Gost=@gost, " +
                                  "Sema=@sema, " +
                                  "Napomena=@napomena, " +
                                  "Sto=@sto, " +
                                  "Br_Odraslih=@brodraslih," +
                                  "Br_Dece=@brdece " +
                                  "WHERE ID = @id";
                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.Parameters.AddWithValue("@id", rezervacija.Id);

                        komanda.Parameters.AddWithValue("@datum", rezervacija.Datum);
                        komanda.Parameters.AddWithValue("@vremeod", rezervacija.VremeOd);
                        komanda.Parameters.AddWithValue("@vremedo", rezervacija.VremeDo);
                        komanda.Parameters.AddWithValue("@gost", rezervacija.IdGost);
                        komanda.Parameters.AddWithValue("@sema",rezervacija.Sema);
                        komanda.Parameters.AddWithValue("@napomena",rezervacija.Napomena);
                        komanda.Parameters.AddWithValue("@sto",rezervacija.Sto);
                        komanda.Parameters.AddWithValue("@brodraslih",rezervacija.BrojOdraslih);
                        komanda.Parameters.AddWithValue("@brdece",rezervacija.BrojDece);

                        int rezultat = komanda.ExecuteNonQuery();
                        if (rezultat < 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateNapomenu(int id, string napomena)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();
                    string upit = "UPDATE [REZERVACIJA] SET " +
                                  "Napomena=@napomena " +
                                  "WHERE ID = @id";
                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.Parameters.AddWithValue("@id", id);

                        komanda.Parameters.AddWithValue("@napomena", napomena);

                        int rezultat = komanda.ExecuteNonQuery();
                        if (rezultat < 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
    
}
