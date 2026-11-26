using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Reportissue : BaseForm
    {
        public Reportissue()
        {
            InitializeComponent();
            // "Report" button e age kono kaj lagano chilo na, worker issue report korte parto na
            button1.Click += Report_Click;
            Load += (s, e) => button3_Click(s, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new WorkerPage().Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Db.Query("SELECT * FROM IssueInfo ORDER BY [Issue-Id]");
        }

        // kon task e ki somossa seta likhe save kore, status Pending thake jotokkhon manager update na kore
        private void Report_Click(object sender, EventArgs e)
        {
            using (var dlg = new InputDialog("Report an Issue", "Task ID:", "Problem:"))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                if (!int.TryParse(dlg.Value(0), out int taskId))
                {
                    MessageBox.Show("Task ID must be a number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string problem = dlg.Value(1);
                if (problem.Length == 0)
                {
                    MessageBox.Show("Please describe the problem.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                object taskExists = Db.Scalar("SELECT COUNT(*) FROM TaskInfo WHERE [Task-Id] = @t", Db.P("@t", taskId));
                if (taskExists == null) return;
                if (Convert.ToInt32(taskExists) == 0)
                {
                    MessageBox.Show("There is no task with ID " + taskId + ".", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                object next = Db.Scalar("SELECT ISNULL(MAX([Issue-Id]), 0) + 1 FROM IssueInfo");
                if (next == null) return;
                int rows = Db.Execute("INSERT INTO IssueInfo ([Issue-Id], Description, status, [Task-Id]) VALUES (@id, @d, 'Pending', @t)",
                    Db.P("@id", Convert.ToInt32(next)), Db.P("@d", problem), Db.P("@t", taskId));
                if (rows > 0)
                {
                    MessageBox.Show("Issue reported. The manager will see it on the Update Issue page.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    button3_Click(sender, e);
                }
            }
        }
    }
}
