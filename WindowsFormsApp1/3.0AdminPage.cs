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
    public partial class AdminPage : BaseForm
    {
        string connectionString = Db.ConnectionString;
        public AdminPage()
        {
            InitializeComponent();
            LoadOrderInfo();
        }

        private void LoadOrderInfo()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
         SELECT 
             [Order-Id], 
             [Product-Id], 
             [Product Name], 
             Quantity, 
             Price, 
             (CAST(Quantity AS float) * Price) AS [Total price], 
             [Buyer-Id], 
             Confirmation
         FROM dbo.OrderInfo";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;

                   
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading OrderInfo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WorkerDashboard_Click(object sender, EventArgs e)
        {
            WorkerDashboard WD= new WorkerDashboard();
            WD.Show();
            this.Hide();
        }

        private void BuyerDashboard_Click(object sender, EventArgs e)
        {
            BuyerDashboard BD1= new BuyerDashboard();
            BD1.Show(); 
            this.Hide();

        }

        private void Exit_Click(object sender, EventArgs e)
        {
            // age shudhu window bondho hoto ar app pichone cholte thakto, ekhon login e fire jay
            Session.Logout();
            new Login().Show();
            this.Close();
        }

        private void ViewProductInfo_Click(object sender, EventArgs e)
        {
            ProductDashboard Pd1 = new ProductDashboard();
            Pd1 .Show();
            this.Hide();
        }

        private void Profile_Click(object sender, EventArgs e)
        {
           Profile p1 = new Profile();
            p1.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            FinanaceDashboard fd1= new FinanaceDashboard();
            fd1.Show();
            this.Hide();
        }

        // admin order er Confirmation ghore "Accept" ba "Decline" likhe ei button chape.
        // age eki order duibar accept korle income duibar jog hoto, cancel kora order o accept hoto,
        // ar stock kombe na. ekhon sob ekshathe (transaction) hoy, kono ek ta vul hole kichui bodlay na
        private void ConfirmOrder_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Please select an order first.", "Confirm Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataGridViewRow row = dataGridView1.CurrentRow;
            int orderId = Convert.ToInt32(row.Cells["Order-Id"].Value);
            string decision = row.Cells["Confirmation"].Value?.ToString().Trim() ?? "";

            bool accept = decision.Equals("Accept", StringComparison.OrdinalIgnoreCase);
            bool decline = decision.Equals("Decline", StringComparison.OrdinalIgnoreCase);
            if (!accept && !decline)
            {
                MessageBox.Show("Type 'Accept' or 'Decline' in the Confirmation column of the order, then press Confirm Order.",
                    "Confirm Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string current = (Db.Scalar("SELECT RTRIM(Confirmation) FROM OrderInfo WHERE [Order-Id] = @o", Db.P("@o", orderId)) ?? "").ToString();
            if (current.Equals("Accept", StringComparison.OrdinalIgnoreCase) || current.Equals("Decline", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Order " + orderId + " was already " + current.ToLower() + "ed.", "Confirm Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadOrderInfo();
                return;
            }
            if (current.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("The buyer cancelled order " + orderId + ".", "Confirm Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadOrderInfo();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlTransaction tran = conn.BeginTransaction())
                    {
                        new SqlCommand("UPDATE OrderInfo SET Confirmation = @c WHERE [Order-Id] = @o", conn, tran)
                        { Parameters = { new SqlParameter("@c", accept ? "Accept" : "Decline"), new SqlParameter("@o", orderId) } }.ExecuteNonQuery();

                        if (accept)
                        {
                            string productId = row.Cells["Product-Id"].Value.ToString().Trim();
                            string productName = row.Cells["Product Name"].Value.ToString().Trim();
                            double unitPrice = Convert.ToDouble(row.Cells["Price"].Value);
                            int quantity = Convert.ToInt32(row.Cells["Quantity"].Value.ToString().Trim());

                            // stock theke kome; stock kom thakle accept hoy na
                            object stockObj = new SqlCommand("SELECT Quantity FROM ProductInfo WHERE [Product ID] = @p", conn, tran)
                            { Parameters = { new SqlParameter("@p", productId) } }.ExecuteScalar();
                            if (stockObj != null && Convert.ToInt32(stockObj) < quantity)
                            {
                                tran.Rollback();
                                MessageBox.Show("Not enough stock: only " + stockObj + " " + productName + " left.", "Confirm Order", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            if (stockObj != null)
                            {
                                new SqlCommand("UPDATE ProductInfo SET Quantity = Quantity - @q WHERE [Product ID] = @p", conn, tran)
                                { Parameters = { new SqlParameter("@q", quantity), new SqlParameter("@p", productId) } }.ExecuteNonQuery();
                            }

                            new SqlCommand(@"INSERT INTO Income ([Product Id], [Product Name], [Unit Price], Quantity, [Total Sell], [Date and time])
                                             VALUES (@p, @n, @u, @q, @t, @d)", conn, tran)
                            {
                                Parameters = { new SqlParameter("@p", productId), new SqlParameter("@n", productName), new SqlParameter("@u", unitPrice),
                                               new SqlParameter("@q", quantity), new SqlParameter("@t", unitPrice * quantity), new SqlParameter("@d", DateTime.Now) }
                            }.ExecuteNonQuery();
                        }
                        tran.Commit();
                    }
                }
                MessageBox.Show(accept ? "Order " + orderId + " accepted and added to income." : "Order " + orderId + " declined.",
                    "Confirm Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException ex)
            {
                Db.ShowError(ex);
            }
            LoadOrderInfo();
        }
    }
}
