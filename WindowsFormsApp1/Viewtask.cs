using System;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Viewtask : BaseForm
    {
        public Viewtask()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new WorkerPage().Show();
            this.Close();
        }

        // khulle-i nijer task gula dekhay
        private void Viewtask_Load(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }

        // worker hole shudhu nijer task, onno keu hole sob
        private void button2_Click(object sender, EventArgs e)
        {
            if (Session.Role == "worker")
            {
                textBox1.Text = Session.UserId.ToString();
                dataGridView1.DataSource = Db.Query("SELECT * FROM TaskInfo WHERE [Worker-Id] = @id ORDER BY [Task-Id]", Db.P("@id", Session.UserId));
            }
            else
            {
                dataGridView1.DataSource = Db.Query("SELECT * FROM TaskInfo ORDER BY [Task-Id]");
            }
        }

        // age query te "@WrokerId" bananer vul chilo, tai search korlei crash korto.
        // ar faka/okkhor dile int.Parse e crash korto
        private void button3_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text.Trim(), out int workerId))
            {
                MessageBox.Show("Please enter a worker ID (number).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataTable dataTable = Db.Query("SELECT * FROM TaskInfo WHERE [Worker-Id] = @WorkerId ORDER BY [Task-Id]", Db.P("@WorkerId", workerId));
            dataGridView1.DataSource = dataTable;
            if (dataTable.Rows.Count == 0)
                MessageBox.Show("No tasks found for worker " + workerId + ".", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
