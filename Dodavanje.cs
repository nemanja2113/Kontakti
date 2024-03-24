using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kontakti_v2
{
    public partial class Dodavanje : Form
    {
        public Dodavanje()
        {
            InitializeComponent();

        }

        
        private void btnUbaci_Click(object sender, EventArgs e)
        {
            int ID = 0;
            string poruka = "";
            using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnKontaktiDB))
            {
                using (SqlCommand komanda = new SqlCommand("UbaciKorisnika",konekcija))
                {
                    komanda.CommandType = CommandType.StoredProcedure;
                    SqlParameter IdParametar=new SqlParameter("@KontaktiID",SqlDbType.Int);
                    IdParametar.Direction= ParameterDirection.Output;
                    komanda.Parameters.Add(IdParametar);

                    try
                    {
                        komanda.Parameters.AddWithValue("@Ime", txbIme.Text);
                        komanda.Parameters.AddWithValue("@Prezime", txtbPrezime.Text);
                        komanda.Parameters.AddWithValue("@Mail", txtbMail.Text);
                        komanda.Parameters.AddWithValue("@BrojTelefona", txtbBrojTelefona.Text);
                        konekcija.Open();
                        komanda.ExecuteNonQuery();
                        ID = (int)IdParametar.Value;
                    }
                    catch (Exception xcp)
                    {
                        poruka=xcp.Message;
                    }
                    if (poruka!="")
                    {
                        MessageBox.Show(poruka);
                    }
                    else
                    {
                        MessageBox.Show("Ubacen je kontakt ");
                        
                    }
                }
            }
            this.Close();
        }

        private void btnNazad_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
