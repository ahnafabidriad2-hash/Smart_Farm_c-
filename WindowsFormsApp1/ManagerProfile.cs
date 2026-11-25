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
    public partial class ManagerProfile : Form
    {
        private string currentManagerID;

        public ManagerProfile()
        {
        }

        public ManagerProfile(string managerID)
        {
            InitializeComponent();
            currentManagerID = managerID;
            this.Load += new EventHandler(ManagerProfile_Load);
        }

        private void ManagerProfile_Load(object sender, EventArgs e)
        {
            string query = "SELECT Username, Password, Phone, Address FROM ManagerInfo WHERE [Manager Id] = @ManagerID";

            using (SqlConnection connection = new SqlConnection("data source=RIAD\\SQLEXPRESS; database=SmartFarm; integrated security=SSPI"))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ManagerID", currentManagerID);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            midtextBox1.Text = currentManagerID;
                            mnametextBox2.Text = reader["Username"].ToString();
                            mpasstextBox3.Text = reader["Password"].ToString();
                            mphonetextBox4.Text = reader["Phone"].ToString();
                            maddresstextBox5.Text = reader["Address"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Manager not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void mupdatebutton1_Click(object sender, EventArgs e)
        {
            string ManagerID = midtextBox1.Text.Trim();
            string Username = mnametextBox2.Text.Trim();
            string Password = mpasstextBox3.Text.Trim();
            string Phone = mphonetextBox4.Text.Trim();
            string Address = maddresstextBox5.Text.Trim();

            if (string.IsNullOrWhiteSpace(ManagerID) ||
                string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(Phone) ||
                string.IsNullOrWhiteSpace(Address))
            {
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE ManagerInfo SET Username = @Username, Password = @Password, Phone = @Phone, Address = @Address " +
                           "WHERE [Manager Id] = @ManagerID";

            using (SqlConnection connection = new SqlConnection(@"data source=RIAD\SQLEXPRESS; database=SmartFarm; integrated security=SSPI"))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ManagerID", ManagerID);
                    command.Parameters.AddWithValue("@Username", Username);
                    command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@Phone", Phone);
                    command.Parameters.AddWithValue("@Address", Address);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("No record was updated. Please check ManagerID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
