using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class ViewProducts : Form
    {
        public ViewProducts()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "data source=.\\SQLEXPRESS; database=SmartFarm; integrated security=SSPI";
            string productName = textBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(productName))
            {
                MessageBox.Show("Please enter a search term.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"SELECT * FROM ProductInfo
                     WHERE [Product Name] LIKE @ProductName";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductName", "%" + productName + "%");


                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dataGridView1.DataSource = dataTable;
                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("No matching rows found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "data source=.\\SQLEXPRESS; database=SmartFarm; integrated security=SSPI";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                string query = "Select * from  ProductInfo";
                   

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                DataTable dt = ds.Tables[0];
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = dt;
                cmd.ExecuteNonQuery();

            }
            catch { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            BuyerOrder bo = new BuyerOrder();
            bo.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BuyerPage bp1 = new BuyerPage();
            bp1.Show();
            this.Hide();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
