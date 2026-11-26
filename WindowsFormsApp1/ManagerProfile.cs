using System;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class ManagerProfile : BaseForm
    {
        private string currentManagerID;

        // age ei constructor e InitializeComponent chilo na, khulle faka window ashto
        public ManagerProfile() : this(Session.UserId.ToString())
        {
        }

        public ManagerProfile(string managerID)
        {
            InitializeComponent();
            currentManagerID = managerID;
            midtextBox1.ReadOnly = true; // nijer ID bodlano jabe na
            this.Load += new EventHandler(ManagerProfile_Load);
            // back button e age kono kaj lagano chilo na
            backbutton2.Click += (s, e) => { new ManagerPage().Show(); this.Close(); };
        }

        private void ManagerProfile_Load(object sender, EventArgs e)
        {
            DataTable dt = Db.Query("SELECT Username, Password, Phone, Address FROM ManagerInfo WHERE [Manager Id] = @ManagerID",
                Db.P("@ManagerID", currentManagerID));
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Manager not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            midtextBox1.Text = currentManagerID;
            mnametextBox2.Text = dt.Rows[0]["Username"].ToString().Trim();
            mpasstextBox3.Text = dt.Rows[0]["Password"].ToString().Trim();
            mphonetextBox4.Text = dt.Rows[0]["Phone"].ToString().Trim();
            maddresstextBox5.Text = dt.Rows[0]["Address"].ToString().Trim();
        }

        private void mupdatebutton1_Click(object sender, EventArgs e)
        {
            string ManagerID = midtextBox1.Text.Trim();
            string Username = mnametextBox2.Text.Trim();
            string Password = mpasstextBox3.Text.Trim();
            string Phone = mphonetextBox4.Text.Trim();
            string Address = maddresstextBox5.Text.Trim();

            if (string.IsNullOrWhiteSpace(ManagerID) || string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(Phone) || string.IsNullOrWhiteSpace(Address))
            {
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(Phone, out int phone))
            {
                MessageBox.Show("Phone must be a number (without the first 0).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // login er password Login table e thake, tai duitai update kori. age shudhu ManagerInfo bodlato,
            // tai notun password diye login kora jeto na
            int rows = Db.Execute("UPDATE ManagerInfo SET Username = @u, Password = @p, Phone = @ph, Address = @a WHERE [Manager Id] = @id",
                Db.P("@u", Username), Db.P("@p", Password), Db.P("@ph", phone), Db.P("@a", Address), Db.P("@id", ManagerID));
            if (rows > 0)
            {
                Db.Execute("UPDATE Login SET Username = @u, Password = @p, Phone = @ph, Address = @a WHERE ID = @id",
                    Db.P("@u", Username), Db.P("@p", Password), Db.P("@ph", phone), Db.P("@a", Address), Db.P("@id", ManagerID));
                MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (rows == 0)
            {
                MessageBox.Show("No record was updated. Please check ManagerID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }
    }
}
