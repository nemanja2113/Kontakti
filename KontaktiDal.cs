using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontakti_v2
{
    static class KontaktiDal
    {
        public static List<Kontakti> VratiKontakte()
        {
            List<Kontakti> listaKontakata = new List<Kontakti>();
            using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnKontaktiDB))
            {
                using (SqlCommand komanda = new SqlCommand("SELECT * FROM Kontakti", konekcija))
                {
                    try
                    {
                        konekcija.Open();
                        using (SqlDataReader dr= komanda.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Kontakti k1 = new Kontakti
                                {
                                    KontaktiID = dr.GetInt32(0),
                                    Ime = dr.GetString(1),
                                    Prezime = dr.GetString(2),
                                    Mail=dr.GetString(3),
                                    BrojTelefona = dr.GetString(4)
                                };
                                listaKontakata.Add(k1);
                            }
                        }
                        return listaKontakata;
                    }
                    catch (Exception)
                    {

                        return null;
                    }
                }
            }
        }
    }
}
