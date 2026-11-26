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
    public partial class Login : BaseForm
    {
        public Login()
        {
            InitializeComponent();
            AcceptButton = button1; // enter chaplei login
            textBox2.UseSystemPasswordChar = true; // password lukano thake
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string id = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both Id and Password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // age ID te okkhor dile database error e app crash korto
            if (!int.TryParse(id, out int userId))
            {
                MessageBox.Show("Id must be a number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable found = Db.Query("SELECT Role, Username FROM Login WHERE Id = @id AND Password = @password",
                Db.P("@id", userId), Db.P("@password", password));
            {
                {
                    if (found.Rows.Count > 0)
                    {
                        string role = found.Rows[0]["Role"].ToString().Trim().ToLower();

                        // ke login korlo mone rakhi, profile ar order page gula eita dekhe
                        Session.UserId = userId;
                        Session.Role = role;
                        Session.Username = found.Rows[0]["Username"].ToString().Trim();

                        this.Hide();

                        switch (role)
                        {
                            case "admin":
                                AdminPage adminPage = new AdminPage();
                                adminPage.Show();
                                break;

                            case "worker":
                                WorkerPage workerPage = new WorkerPage();
                                workerPage.Show();
                                break;

                            case "manager":
                                ManagerPage managerPage = new ManagerPage();
                                managerPage.Show();
                                break;

                            case "buyer":
                                BuyerPage BPage = new BuyerPage();
                                BPage.Show();
                                break;

                            default:
                                MessageBox.Show("Unknown role: " + role, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Id or Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please contact with admin at ahnafabidriad2@gmail.com or call -01751330014, for faster response.Hope you have a good day.", "Admin Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
