using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EKontakt.ekontaktKlase
{
    class contactClass
    {
        //Nosioc Podataka u Aplikaciji
        public int IDKorisnika { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Broj { get; set; }
        public string Adresa { get; set; }
        public string Pol { get; set; }
        public int IDKategorije { get; set; }



        static string mojakonekcija = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        //Povlacenje podataka iz baze
        public DataTable Select()
        {



            //1. Konekcija sa bazom
            SqlConnection conn = new SqlConnection(mojakonekcija);
            DataTable dt = new DataTable();
            try
            {
                //Ispis SQL Querry-ja
                string sql = "SELECT EKontaktDB.IDKorisnika, EKontaktDB.Ime, EKontaktDB.Prezime, EKontaktDB.Broj, EKontaktDB.Adresa, EKontaktDB.Pol, KategorijaKontakta.NazivKategorije FROM EKontaktDB INNER JOIN KategorijaKontakta ON EKontaktDB.IDKategorije = KategorijaKontakta.IDKategorije";


                //Stvaranje komande koristeci sql i conn
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Stvaranje adaptera koriscenjem cmd
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                conn.Open();
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return dt;

        }

        //Unosenje podataka u bazu
        public bool Insert(contactClass c)
        {
            //Stvaranje default return type i postavljanje vrednosti na false
            bool isSuccess = false;
            //1. Povezivanje baze
            SqlConnection conn = new SqlConnection(mojakonekcija);
            try
            {
                //Stvaranje SQL Query za unos podataka
                string sql = "INSERT INTO EKontaktDB (Ime, Prezime, Broj, Adresa, Pol, IDKategorije) VALUES (@Ime, @Prezime, @Broj, @Adresa, @Pol, @IDKategorije)";


                //Stvaranje SQL komande koriscenjem sql i conn
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Stvaranje parametara za unos podataka
                cmd.Parameters.AddWithValue("@Ime", c.Ime);
                cmd.Parameters.AddWithValue("@Prezime", c.Prezime);
                cmd.Parameters.AddWithValue("@Broj", c.Broj);
                cmd.Parameters.AddWithValue("@Adresa", c.Adresa);
                cmd.Parameters.AddWithValue("@Pol", c.Pol);
                cmd.Parameters.AddWithValue("@IDKategorije", c.IDKategorije);




                //Otvaranje konekcije
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                //Ako se querry uspesno pokrene vrednost redova ce biti veca od 0
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return isSuccess;
        }

        //Metoda za azuriranje podataka 
        public bool Update(contactClass c)
        {
            //Stvaranje defaultnog tipa vracanja podataka i postavljanje pocetne vrednosti na false
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(mojakonekcija);
            try
            {
                //Upit za azuriranje podataka u bazi
                string sql = "UPDATE EKontaktDB SET Ime=@Ime, Prezime=@Prezime, Broj=@Broj, Adresa=@Adresa, Pol=@Pol, IDKategorije=@IDKategorije WHERE IDKorisnika=@IDKorisnika";



                //Stvaranje komande 
                SqlCommand cmd = new SqlCommand(sql, conn);
                //Stvaranje parametara za dodavanje vrednosti
                cmd.Parameters.AddWithValue("@Ime", c.Ime);
                cmd.Parameters.AddWithValue("@Prezime", c.Prezime);
                cmd.Parameters.AddWithValue("@Broj", c.Broj);
                cmd.Parameters.AddWithValue("@Adresa", c.Adresa);
                cmd.Parameters.AddWithValue("@Pol", c.Pol);
                cmd.Parameters.AddWithValue("IDKorisnika", c.IDKorisnika);
                cmd.Parameters.AddWithValue("@IDKategorije", c.IDKategorije);



                //Otvaranje konekcije 
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                //Ako bude uspesna vrednost ce biti veca od 0
                if (rows <= 0)
                {
                    isSuccess = false;
                }
                else
                {
                    isSuccess = true;
                }
            }

            catch (Exception ex)
            {


            }
            finally
            {
                conn.Close();
            }
            return isSuccess;

        }

        private static bool @false()
        {
            bool isSuccess;
            {
                isSuccess = false;
            }

            return isSuccess;
        }
        //Metoda za brisanje podataka iz baze
        public bool Delete(contactClass c)
        {
            //Postavljanje default vrednosti vracanja
            bool isSuccess = false;
            //Postavljanje SQL konekcije
            SqlConnection conn = new SqlConnection(mojakonekcija);
            try
            {
                //Upit za brisanje podataka
                string sql = "DELETE FROM EKontaktDB WHERE IDKorisnika=@IDKorisnika";

                //Stvaranje komande
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@IDKorisnika", c.IDKorisnika);
                //Otvaranje konekcije
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                //Ako je uspesno vrednost ce biti veca od 0
                if (rows <= 0)
                {
                    isSuccess = false;
                }
                else
                {
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
            return isSuccess;

        }
        public DataTable FilterByCategory(string nazivKategorije)
        {
            SqlConnection conn = new SqlConnection(mojakonekcija);
            DataTable dt = new DataTable();
            try
            {
                // SQL upit za filtriranje kontakata po nazivu kategorije
                string sql = "SELECT EKontaktDB.IDKorisnika, EKontaktDB.Ime, EKontaktDB.Prezime, EKontaktDB.Broj, EKontaktDB.Adresa, EKontaktDB.Pol, KategorijaKontakta.NazivKategorije FROM EKontaktDB INNER JOIN KategorijaKontakta ON EKontaktDB.IDKategorije = KategorijaKontakta.IDKategorije WHERE KategorijaKontakta.NazivKategorije = @NazivKategorije";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@NazivKategorije", nazivKategorije);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                conn.Open();
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                // Obrada izuzetka
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

    }
}

    
    