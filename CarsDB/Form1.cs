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

namespace CarsDB
{
    public partial class Form1 : Form
    {
        string connectionString;
        SqlConnection connection;
        public Form1()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["CarsDB.Properties.Settings.CarsConnectionString"].ConnectionString;
        }

        private void PopulateCarsTable()
        {
            using (connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM CarMark", connection))
            {
                DataTable carTable = new DataTable();
                adapter.Fill(carTable);

                listCars.DisplayMember = "CarMarkName";
                listCars.ValueMember = "Id";
                listCars.DataSource = carTable;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateCarsTable();
        }

        private void PopulateModels()
        {
            string query = "SELECT CarInGarage.CarModelName FROM CarMark INNER JOIN CarInGarage ON CarInGarage.CarMarkId = CarMark.Id WHERE CarMark.Id = @TypeId";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.Parameters.AddWithValue("@TypeId", listCars.SelectedValue);
                DataTable carCarModelNameTable = new DataTable();
                adapter.Fill(carCarModelNameTable);

                listModels.DisplayMember = "CarModelName";
                listModels.ValueMember = "Id";
                listModels.DataSource = carCarModelNameTable;
            }
        }

        private void listCars_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateModels();
        }
    }
}
