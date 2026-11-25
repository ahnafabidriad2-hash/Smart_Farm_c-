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
    public partial class ProductDashboard : Form
    {
        private SqlDataAdapter adapter;
        private DataTable dt;
        string connectionString = "data source=.\\SQLEXPRESS; database=SmartFarm; integrated security=SSPI";

        public ProductDashboard()
        {
            InitializeComponent();
            FillDataGridView("SELECT * FROM ProductInfo");
        }

        private void FillDataGridView(string query)
        {
            SqlConnection con = new SqlConnection(connectionString);
            adapter = new SqlDataAdapter(query, con);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

            
            adapter.InsertCommand = builder.GetInsertCommand();
            adapter.UpdateCommand = builder.GetUpdateCommand();
            adapter.DeleteCommand = builder.GetDeleteCommand();

            dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
       

        private void button2_Click(object sender, EventArgs e)
        {
            if (adapter == null || dt == null)
            {
                MessageBox.Show("No data loaded to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                adapter.Update(dt);
                MessageBox.Show("Product information updated successfully.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FillDataGridView("SELECT * FROM ProductInfo"); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            AdminPage Ap3 = new AdminPage();
            Ap3.Show();
            this.Close();
        }
    }
}
