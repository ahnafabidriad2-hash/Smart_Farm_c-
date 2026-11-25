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
    public partial class Supplies : Form
    {
        private SqlDataAdapter ad;
        private DataTable D;
        string connectionString = "data source=RIAD\\SQLEXPRESS; database=SmartFarm; integrated security=SSPI";
        public Supplies()
        {
            InitializeComponent();
            FillDataGridView("Select * From SupplyInfo");
        }
        private void FillDataGridView(string query)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                ad = new SqlDataAdapter(query, connectionString);
                SqlCommandBuilder builder = new SqlCommandBuilder(ad);

                D = new DataTable();
                ad.Fill(D);
                dataGridView1.DataSource = D;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Commit any edits in the grid
                dataGridView1.EndEdit();

                // Get values from the currently selected row
                if (dataGridView1.CurrentRow == null) return;

                int orderId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Supply-Id"].Value);
                string supplyName = dataGridView1.CurrentRow.Cells["Supply name"].Value.ToString();
                decimal price = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Price"].Value);
                decimal qty = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["Quantity"].Value);
                decimal totalAmount = price * qty;
                DateTime orderDate = DateTime.Now;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {
                            // Insert into OrderInfo
                            using (SqlCommand cmd = new SqlCommand(
    "INSERT INTO SupplyInfo ([Supply-Id], [Supply name], Price, Quantity, [Date and Time]) " +
    "VALUES (@OrderId, @SupplyName, @Price, @Qty, @OrderDate)", conn, tran))
                            {

                                cmd.Parameters.AddWithValue("@OrderId", orderId);
                                cmd.Parameters.AddWithValue("@SupplyName", supplyName);
                                cmd.Parameters.AddWithValue("@Price", price);
                                cmd.Parameters.AddWithValue("@Qty", qty);
                                cmd.Parameters.AddWithValue("@OrderDate", orderDate);
                                cmd.ExecuteNonQuery();
                            }

                            // Insert into Expense
                            using (SqlCommand cmd = new SqlCommand(
                                "INSERT INTO Expense ([Expense Id],[Expense Catagory],Details,Amount,[Date and Time]) " +
                                "VALUES (@ExpenseId,@Category,@Details,@Amount,@When)", conn, tran))
                            {



                                cmd.Parameters.AddWithValue("@ExpenseId", "ORD-" + orderId);
                                cmd.Parameters.AddWithValue("@Category", supplyName);
                                cmd.Parameters.AddWithValue("@Details", "Order for " + supplyName);
                                cmd.Parameters.AddWithValue("@Amount", totalAmount.ToString());
                                cmd.Parameters.AddWithValue("@When", orderDate);
                                cmd.ExecuteNonQuery();
                            }

                            tran.Commit();
                            MessageBox.Show("Saved to both tables.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            MessageBox.Show("Save failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            ManagerPage  managerPage = new ManagerPage();   
            managerPage.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
