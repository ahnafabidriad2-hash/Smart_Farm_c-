using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class AssignTask : BaseForm
    {
        public AssignTask()
        {
            InitializeComponent();
            tstatuscomboBox1.DataSource = new[] { "Pending", "Working", "Completed" };
            tstatuscomboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            // porer khali task ID ta age thekei bosiye dei
            object max = Db.Scalar("SELECT ISNULL(MAX([Task-Id]), 0) + 1 FROM TaskInfo");
            if (max != null) taskidtextBox1.Text = max.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string taskID = taskidtextBox1.Text.Trim();
            string status = tstatuscomboBox1.SelectedItem?.ToString();
            string description = tdestextBox2.Text.Trim();
            string workerID = wtextBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(taskID) || string.IsNullOrWhiteSpace(status) ||
                string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(workerID))
            {
                MessageBox.Show("Please fill in all fields.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // age okkhor dile ba eki ID duibar dile app crash korto
            if (!int.TryParse(taskID, out int tid) || !int.TryParse(workerID, out int wid))
            {
                MessageBox.Show("Task ID and Worker ID must be numbers.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // je worker nai tar kache task deya jabe na
            object isWorker = Db.Scalar("SELECT COUNT(*) FROM Login WHERE ID = @id AND Role = 'worker'", Db.P("@id", wid));
            if (isWorker == null) return;
            if (Convert.ToInt32(isWorker) == 0)
            {
                MessageBox.Show("There is no worker with ID " + wid + ".", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int result = Db.Execute("INSERT INTO TaskInfo ([Task-ID], Status, Description, [Worker-ID]) VALUES (@TaskID, @Status, @Description, @WorkerID)",
                Db.P("@TaskID", tid), Db.P("@Status", status), Db.P("@Description", description), Db.P("@WorkerID", wid));
            if (result > 0)
            {
                MessageBox.Show("Task assigned successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                taskidtextBox1.Text = (tid + 1).ToString();
                tdestextBox2.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new ManagerPage().Show();
            this.Close();
        }
    }
}
