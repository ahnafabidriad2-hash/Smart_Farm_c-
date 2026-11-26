using System;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class WorkerProfile : BaseForm
    {
        public object username { get; set; }

        public WorkerProfile()
        {
            InitializeComponent();
            textBox4.ReadOnly = true; // worker ID bodlano jabe na
        }

        // age profile kokhono load hoto na: query te "Worker_Id" nam er column chilo na,
        // parameter er naam o milto na, ar Show button er vitore kichu chilo na
        private void LoadUserProfile()
        {
            DataTable dt = Db.Query("SELECT [Worker Id], Username, Password, Address, Phone FROM WorkerInfo WHERE [Worker Id] = @id",
                Db.P("@id", Session.UserId));
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Worker info not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataRow r = dt.Rows[0];
            textBox4.Text = r["Worker Id"].ToString();
            textBox1.Text = r["Username"].ToString().Trim();
            textBox2.Text = r["Password"].ToString().Trim();
            textBox3.Text = r["Phone"].ToString().Trim();
            richTextBox1.Text = r["Address"].ToString().Trim();
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new WorkerPage().Show();
            this.Close();
        }

        private void WorkerProfile_Load(object sender, EventArgs e)
        {
            LoadUserProfile();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userName = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();
            string phone = textBox3.Text.Trim();
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

            // age naam diye khuje update korto (naam bodlale kaj korto na), ekhon ID diye.
            // login er password o bodlay, age shudhu WorkerInfo bodlato
            int rows = Db.Execute("UPDATE WorkerInfo SET Username = @u, Password = @p, Phone = @ph, Address = @a WHERE [Worker Id] = @id",
                Db.P("@u", userName), Db.P("@p", password), Db.P("@ph", Phone), Db.P("@a", address), Db.P("@id", Session.UserId));
            if (rows > 0)
            {
                Db.Execute("UPDATE Login SET Username = @u, Password = @p, Phone = @ph, Address = @a WHERE ID = @id",
                    Db.P("@u", userName), Db.P("@p", password), Db.P("@ph", Phone), Db.P("@a", address), Db.P("@id", Session.UserId));
                MessageBox.Show("Record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (rows == 0)
            {
                MessageBox.Show("No record was updated. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // "Show" chaple abar database theke load kore
        private void button3_Click(object sender, EventArgs e)
        {
            LoadUserProfile();
        }

        private void textBox4_TextChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void richTextBox1_TextChanged(object sender, EventArgs e) { }
    }
}
