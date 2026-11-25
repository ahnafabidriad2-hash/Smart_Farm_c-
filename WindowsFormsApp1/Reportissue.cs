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
    public partial class Reportissue : Form
    {
        public Reportissue()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WorkerPage wp = new WorkerPage();
            wp.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = "data source=RIAD\\SQLEXPRESS; database=SmartFarm; integrated security=SSPI";
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                string query = "Select * from IssueInfo ";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                DataTable dt = ds.Tables[0];
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = dt;
                cmd.ExecuteNonQuery();

            }
            catch { }
        }
    }
}
    

