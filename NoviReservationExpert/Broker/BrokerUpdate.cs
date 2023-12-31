﻿using Microsoft.VisualBasic;
using NoviReservationExpert.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.AvalonDock.Themes;

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
                                  "ImeGosta=@imegosta ," +
                                  "PrezimeGosta=@prezimegosta, " +
                                  "TelefonGosta=@telefongosta, " +
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
                        komanda.Parameters.AddWithValue("@imegosta", rezervacija.ImeGosta);
                        komanda.Parameters.AddWithValue("@prezimegosta", rezervacija.PrezimeGosta);
                        komanda.Parameters.AddWithValue("@telefongosta", rezervacija.BrojTelefona);
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
        public bool UpdateRezervaciju(int idrezervacije,int status)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();
                    string upit = "UPDATE [REZERVACIJA] SET " +
                                  "Status=@status " +
                                  "WHERE ID = @idrezervacije";
                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {
                        komanda.Parameters.AddWithValue("@idrezervacije", idrezervacije);
                        komanda.Parameters.AddWithValue("@status", status);

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
        public bool UpdateSveRezervacijePreDanasnjegDana()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DBBroker.konekcioniString))
                {
                    connection.Open();
                    string upit = "UPDATE [REZERVACIJA] SET " +
                                  "Status = 2 " +
                                  "WHERE Dtm < @datum";
                    using (SqlCommand komanda = new SqlCommand(upit, connection))
                    {

                        komanda.Parameters.AddWithValue("@datum", DateTime.Today);

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
