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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Kontakti_v2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void PrikaziKontakte()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = KontaktiDal.VratiKontakte();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'winFormDataSet.Kontakti' table. You can move, or remove it, as needed.
            this.kontaktiTableAdapter.Fill(this.winFormDataSet.Kontakti);
            PrikaziKontakte();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            Dodavanje dodavanje=new Dodavanje();
            dodavanje.ShowDialog();

            Osvezi();
        }

        private void btnOsvezi_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = KontaktiDal.VratiKontakte();

            dataGridView1.Refresh();
        }
        private void Osvezi()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = KontaktiDal.VratiKontakte();
            dataGridView1.Refresh();
        }

        private void IzbrisiKorisnika(int KontaktiID)
        {
            using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnKontaktiDB))
            {
                konekcija.Open();

                using (SqlCommand komanda = new SqlCommand("DELETE FROM Kontakti WHERE KontaktiID = @KontaktiID", konekcija))
                {
                    komanda.Parameters.AddWithValue("@KontaktiID", KontaktiID);
                    komanda.ExecuteNonQuery();
                }
            }
        }
        private void btnObrisi_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.CurrentRow;

            if (selectedRow != null)
            {
                // Dohvati identifikator reda koji se želi izbrisati
                int kontaktiID = Convert.ToInt32(selectedRow.Cells["KontaktiID"].Value);

                // Potvrdi brisanje putem dijaloga (možete dodati dijalog za potvrdu ako želite)
                DialogResult result = MessageBox.Show("Jeste li sigurni da želite izbrisati ovaj kontakt?", "Potvrda brisanja", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    IzbrisiKorisnika(kontaktiID);

                }
            }
            else
            {
                MessageBox.Show("Odaberite redak koji želite izbrisati.");
            }

            Osvezi();

        }
  
        private void btnIzmeni_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.CurrentRow;

            if (selectedRow != null)
            {
                int kontaktiID = Convert.ToInt32(selectedRow.Cells["KontaktiID"].Value);

                DialogResult result = MessageBox.Show("Jeste li sigurni da želite izmeniti ovaj kontakt?", "Potvrda menjanja", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    Izmena izmena = new Izmena();

                    // Prenesite vrednosti izabrane vrste u TextBox-ove forme IzmenaForm
                    izmena.SetValues(
                        selectedRow.Cells["Ime"].Value.ToString(),
                        selectedRow.Cells["Prezime"].Value.ToString(),
                        selectedRow.Cells["Mail"].Value.ToString(),
                        selectedRow.Cells["BrojTelefona"].Value.ToString()
                    );

                    izmena.KontaktId=kontaktiID;
                    izmena.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Odaberite redak koji želite izmeniti.");
            }
            Osvezi();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var odabraniItem = comboBox1.SelectedItem;

            if (odabraniItem=="Ime")
            {
                Sortiraj("Ime");
            }
            else if (odabraniItem=="Prezime")
            {
                Sortiraj("Prezime");
            }
            else if (odabraniItem=="-")
            {
                Sortiraj("KontaktiID");
            }
        }

        private void Sortiraj(string orderBy)
        {
            using (SqlConnection konekcija = new SqlConnection(Konekcija.cnnKontaktiDB))
            {
                konekcija.Open();

                using (SqlCommand komanda = new SqlCommand($"SELECT * FROM Kontakti ORDER BY {orderBy}", konekcija))
                {
                    // Koristi SqlDataAdapter za dohvaćanje podataka iz baze
                    using (SqlDataAdapter adapter = new SqlDataAdapter(komanda))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Postavi DataTable u DataGridView
                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
        }
    }
}
