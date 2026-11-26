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
    public partial class UpdateIssue : BaseForm
    {
        private SqlDataAdapter Adapter;
        private DataTable DT;
        string connectionString = Db.ConnectionString;
        public UpdateIssue()
        {
            InitializeComponent();
            FillDataGridView("Select * From IssueInfo");
        }
        private void FillDataGridView(string query)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                Adapter = new SqlDataAdapter(query, connectionString);
                SqlCommandBuilder builder = new SqlCommandBuilder(Adapter);

                DT = new DataTable();
                Adapter.Fill(DT);
                dataGridView1.DataSource = DT;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

        }

        private void updateissuebutton1_Click(object sender, EventArgs e)
        {
            if (Adapter == null || DT == null)
            {
                MessageBox.Show("No data loaded to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                Adapter.Update(DT);
                MessageBox.Show("Issue information updated successfully.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FillDataGridView("SELECT * FROM IssueInfo");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backbutton3_Click(object sender, EventArgs e)
        {
            this.Close();
            ManagerPage managerPage = new ManagerPage();    
            managerPage.Show();
        }
    }
}
