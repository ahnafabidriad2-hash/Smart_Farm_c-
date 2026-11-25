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

namespace WindowsFormsApp1
{
    public partial class Profile : Form
    {

        string connectionString = "data source=.\\SQLEXPRESS; database=SmartFarm; integrated security=SSPI";
        private int adminId = 1;
        public Profile()
        {
            InitializeComponent();
            LoadDetails();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox2.Text.ToString();
            string password = textBox3.Text.ToString();
            string address = textBox4.Text.ToString();
            string phone = textBox5.Text.ToString();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE Login SET username = @Name, password = @pass, Address = @Address , Phone = @Phone WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", adminId);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@pass", password);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@Phone", phone);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Close(); 
                    }
                    else
                    {
                        MessageBox.Show("No record was updated. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void LoadDetails()
        {
            string query = "SELECT id, username, password, Address, Phone FROM Login WHERE id = @adminid";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@adminid", adminId);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        
                        textBox1.Text = reader["id"].ToString();
                        textBox2.Text = reader["username"].ToString();
                        textBox3.Text = reader["password"].ToString();
                        textBox4.Text = reader["Address"].ToString();
                        textBox5.Text = reader["Phone"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("No details found for the given ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Close(); 
                    }
                }
            }

        }

        private void Profile_Load(object sender, EventArgs e)
        {

        }

        private void Exit_Click(object sender, EventArgs e)
        {
            AdminPage ap5 = new AdminPage();
            ap5.Show();
            this.Close();
        }
    }
}
