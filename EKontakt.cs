using EKontakt.ekontaktKlase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace EKontakt
{
    public partial class EKontakt : Form
    {
        public EKontakt()
        {
            InitializeComponent();
        }
        contactClass c = new contactClass();
        private void label1IDKorisnika_Click(object sender, EventArgs e)
        {

        }

        private void label7Pretraga_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2Izlaz_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1Dodaj_Click(object sender, EventArgs e)
        {
            //Uzimanje vrednosti iz inputa
            c.Ime = textBox2Ime.Text;
            c.Prezime = textBox3Prezime.Text;
            c.Broj = textBox4Broj.Text;
            c.Adresa = textBox5Adresa.Text;
            c.Pol = comboBox1Pol.Text;

            if (comboBox2Kategorija.Text == "Privatni")
            {
                c.IDKategorije = 1; // 1 je ID za Privatni
            }
            else if (comboBox2Kategorija.Text == "Poslovni")
            {
                c.IDKategorije = 2; // 2 je ID za Poslovni
            }


            //Unosenje podataka metodom koju smo kreirali
            bool success = c.Insert(c);
            if (success == true)
            {
                //Ispis za uspesan unos
                MessageBox.Show("Kontakt uspesno unet");
                //Pozivanje metode za brisanje unosa
                Clear();
            }
            else
            {
                //Ispis za neuspesan unos
                MessageBox.Show("Neuspesan unos kontakta");
            }
            //Ucitavanje podataka na datagridu
            DataTable dt = c.Select();
            dataGridListaKontakta.DataSource = dt;

        }

        private void EKontakt_Load(object sender, EventArgs e)
        {
            LoadCategories(); // Učitaj kategorije
    DataTable dt = c.Select(); // Učitaj kontakte
    dataGridListaKontakta.DataSource = dt;
        }
        //Metoda za brisanje unosa nakon dodatog kontakta   
        public void Clear()
        {
            textBox2Ime.Text = "";
            textBox3Prezime.Text = "";
            textBox4Broj.Text = "";
            textBox5Adresa.Text = "";
            comboBox1Pol.Text = "";
      


        }

        private void button2Ažuriraj_Click(object sender, EventArgs e)
        {
            //Uzimanje podataka iz textboxova
            c.IDKorisnika = int.Parse(textBox1IDKorisnika.Text);
            c.Ime = textBox2Ime.Text;
            c.Prezime = textBox3Prezime.Text;
            c.Broj = textBox4Broj.Text;
            c.Adresa = textBox5Adresa.Text;
            c.Pol = comboBox1Pol.Text;
           
            //Azuriranje podataka 
            bool success = c.Update(c);
            if (success == true)
            {
                //Uspesno azurirano
                MessageBox.Show("Uspesno ažurirano");
                //Ucitavanje podataka na datagridu
                DataTable dt = c.Select();
                dataGridListaKontakta.DataSource = dt;
                //Ciscenje boxova nakon uspesnog azuriranja
                Clear();
            }
            else
            {
                //Neuspesno azrurianje
                MessageBox.Show("Ažuriranje nije uspelo");
            }

        }

        private void dataGridListaKontakta_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Unos podataka sa data grida u textboxove
            //Identifikovanje reda koji je izabran klikom
            int rowIndex = e.RowIndex;
            textBox1IDKorisnika.Text = dataGridListaKontakta.Rows[rowIndex].Cells[0].Value.ToString();
            textBox2Ime.Text = dataGridListaKontakta.Rows[rowIndex].Cells[1].Value.ToString();
            textBox3Prezime.Text = dataGridListaKontakta.Rows[rowIndex].Cells[2].Value.ToString();
            textBox4Broj.Text = dataGridListaKontakta.Rows[rowIndex].Cells[3].Value.ToString();
            textBox5Adresa.Text = dataGridListaKontakta.Rows[rowIndex].Cells[4].Value.ToString();
            comboBox1Pol.Text = dataGridListaKontakta.Rows[rowIndex].Cells[5].Value.ToString();

        }

        private void button4Očisti_Click(object sender, EventArgs e)
        {
            //Pozivanje metode za ciscenje boxova
            Clear();
        }

        private void button3Izbriši_Click(object sender, EventArgs e)
        {
            //Pozivanje podataka iz boxova
            c.IDKorisnika = Convert.ToInt32(textBox1IDKorisnika.Text);
            bool success = c.Delete(c);
            if (success == true)
            {
                //Uspesno brisanje
                MessageBox.Show("Kontakt uspesno izbrisan");
                //Ucitavanje podataka na datagridu
                DataTable dt = c.Select();
                dataGridListaKontakta.DataSource = dt;
                //Pozivanje metode ciscenja
                Clear();
            }
            else
            {
                //Neuspesno brisanje
                MessageBox.Show("Kontakt nije obrisan");
            }
        }
        static string mojakonekcija = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        private void textBoxPretraga_TextChanged(object sender, EventArgs e)
        {
            //Uzimanje vrednosti iz textboa
            string keyword = textBoxPretraga.Text;

            SqlConnection conn = new SqlConnection(mojakonekcija);
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM EKontaktDB WHERE Ime LIKE '%" + keyword + "%' OR Prezime LIKE '%" + keyword + "%' OR Adresa LIKE '%" + keyword + "%'", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridListaKontakta.DataSource = dt;

        }

        private void comboBoxKategorija_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Provera koja je kategorija izabrana
            string nazivKategorije = comboBox2Kategorija.Text;

            // Filtriranje kontakata na osnovu naziva kategorije
            DataTable dt = c.FilterByCategory(nazivKategorije);
            dataGridListaKontakta.DataSource = dt;
        }


        private void LoadCategories()
        {
            SqlConnection conn = new SqlConnection(mojakonekcija);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT NazivKategorije FROM KategorijaKontakta", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                comboBox2Kategorija.Items.Clear(); // Očisti prethodne vrednosti

                while (reader.Read())
                {
                    string nazivKategorije = reader["NazivKategorije"].ToString();
                    comboBox2Kategorija.Items.Add(nazivKategorije);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }




    }
}

