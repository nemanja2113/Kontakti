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
    public partial class Izmena : Form
    {
        private int kontaktId;
        public Izmena()
        {
            InitializeComponent();

        }
        public int KontaktId
        {
            get { return kontaktId; }
            set { kontaktId = value; }
        }
        public void SetValues(string ime, string prezime, string mail, string brojTelefona)
        {
            txbIme.Text = ime;
            txtbPrezime.Text = prezime;
            txtbMail.Text = mail;
            txtbBrojTelefona.Text = brojTelefona;
        }

        private void btnIzmeni_Click(object sender, EventArgs e)
        {
            
             using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnKontaktiDB))
             {
                 using (SqlCommand komanda = new SqlCommand("IzmeniKontakt", konekcija)) 
                 {
                     komanda.CommandType = CommandType.StoredProcedure;
                     SqlParameter IdParametar = new SqlParameter("@KontaktiID", SqlDbType.Int);
                     IdParametar.Direction = ParameterDirection.Input;
                     komanda.Parameters.Add(IdParametar);

                    komanda.Parameters.AddWithValue("@Ime", txbIme.Text);
                    komanda.Parameters.AddWithValue("@Prezime", txtbPrezime.Text);
                    komanda.Parameters.AddWithValue("@Mail", txtbMail.Text);
                    komanda.Parameters.AddWithValue("@BrojTelefona", txtbBrojTelefona.Text);

                    try
                    {
                        komanda.Parameters["@KontaktiID"].Value = kontaktId;
                        konekcija.Open();
                        komanda.ExecuteNonQuery();
                        MessageBox.Show("Podaci su uspešno izmenjeni.");
                        this.Close(); 
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Došlo je do greške prilikom čuvanja izmena: " + ex.Message);
                    }

                 }

             } 

            /*  drugi nacin
            using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnKontaktiDB))
            {
                using (SqlCommand komanda = new SqlCommand("UPDATE Kontakti SET Ime = @Ime, Prezime = @Prezime, Mail = @Mail, BrojTelefona = @BrojTelefona WHERE KontaktiID = @KontaktiID", konekcija))
                {
                    komanda.Parameters.AddWithValue("@KontaktiID", kontaktId);
                    komanda.Parameters.AddWithValue("@Ime", txbIme.Text);
                    komanda.Parameters.AddWithValue("@Prezime", txtbPrezime.Text);
                    komanda.Parameters.AddWithValue("@Mail", txtbMail.Text);
                    komanda.Parameters.AddWithValue("@BrojTelefona", txtbBrojTelefona.Text);

                    try
                    {
                        konekcija.Open();
                        komanda.ExecuteNonQuery();
                        MessageBox.Show("Podaci su uspešno izmenjeni.");
                        this.Close(); // Zatvorite formu nakon čuvanja
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Došlo je do greške prilikom čuvanja izmena: " + ex.Message);
                    }
                }
            }*/
        }

        private void Izmena_Load(object sender, EventArgs e)
        {

        }

        private void btnNazad_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
