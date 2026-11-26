using System;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    // age buyer ke nijer naam type kore "Show All" chapte hoto, ID faka rekhe Update chaple crash korto,
    // ar Delete ID ke naam hishebe khujto tai kichu delete hoto na. ekhon login kora buyer er info nijei ase
    public partial class Buyer_Profile : BaseForm
    {
        public Buyer_Profile()
        {
            InitializeComponent();
            textBox1.ReadOnly = true; // nijer ID bodlano jabe na
            dataGridView1.ReadOnly = true;
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void LoadProfile()
        {
            DataTable dt = Db.Query("SELECT * FROM BuyerInfo WHERE [Buyer-ID] = @id", Db.P("@id", Session.UserId));
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Your buyer profile was not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataRow r = dt.Rows[0];
            textBox1.Text = r["Buyer-ID"].ToString();
            textBox2.Text = r["Username"].ToString().Trim();
            textBox3.Text = r["Password"].ToString().Trim();
            textBox4.Text = r["Phone"].ToString().Trim();
            richTextBox1.Text = r["Address"].ToString().Trim();
            // nicher table e nijer order gula
            dataGridView1.DataSource = Db.Query("SELECT [Order-Id], [Product Name], Quantity, Price, [Total price], Confirmation FROM OrderInfo WHERE [Buyer-Id] = @id",
                Db.P("@id", Session.UserId));
        }

        // "Show All" = abar load
        private void button3_Click(object sender, EventArgs e)
        {
            LoadProfile();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userName = textBox2.Text.Trim();
            string password = textBox3.Text.Trim();
            string phone = textBox4.Text.Trim();
            string address = richTextBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(phone, out int Phone))
            {
                MessageBox.Show("Phone must be numeric.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int rows = Db.Execute("UPDATE BuyerInfo SET UserName = @u, Password = @p, Phone = @ph, Address = @a WHERE [Buyer-ID] = @id",
                Db.P("@u", userName), Db.P("@p", password), Db.P("@ph", Phone), Db.P("@a", address), Db.P("@id", Session.UserId));
            if (rows > 0)
            {
                // login er password o eki shathe bodlay
                Db.Execute("UPDATE Login SET Username = @u, Password = @p, Phone = @ph, Address = @a WHERE ID = @id",
                    Db.P("@u", userName), Db.P("@p", password), Db.P("@ph", Phone), Db.P("@a", address), Db.P("@id", Session.UserId));
                MessageBox.Show("Record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (rows == 0)
            {
                MessageBox.Show("No record was updated. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // profile delete korle login o muche jay, tarpor login page e fire jay
        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete your profile? You will not be able to log in again.", "Confirm Deletion",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
            int rows = Db.Execute("DELETE FROM BuyerInfo WHERE [Buyer-ID] = @id", Db.P("@id", Session.UserId));
            if (rows < 0) return;
            Db.Execute("DELETE FROM Login WHERE ID = @id AND Role = 'buyer'", Db.P("@id", Session.UserId));
            MessageBox.Show("Profile deleted.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Session.Logout();
            new Login().Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new BuyerPage().Show();
            this.Close();
        }

        private void Buyer_Profile_Load(object sender, EventArgs e)
        {
            LoadProfile();
        }
    }
}
