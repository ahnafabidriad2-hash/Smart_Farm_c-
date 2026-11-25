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
    public partial class ShowOrders : Form
    {
        string connectionString = "data source=RIAD\\SQLEXPRESS; database=SmartFarm; integrated security=SSPI";
        private object buyerId;

        public ShowOrders()
        {
            InitializeComponent();
        }

        private void MyOrdersForm_Load(object sender, EventArgs e)
        {
            LoadOrders();
        }
        private void LoadOrders()
        {
            richTextBox1.Clear();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT [Order-Id], [Product-Id], [Product Name], Quantity, Price,  [Total Price], Confirmation FROM OrderInfo WHERE [Buyer-Id] = @BuyerId";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@BuyerId", buyerId);

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    richTextBox1.Text = "You have no orders.";
                    return;
                }

                StringBuilder sb = new StringBuilder();
                foreach (DataRow row in dt.Rows)
                {
                    sb.AppendLine($"Order Id: {row["OrderId"]}");
                    sb.AppendLine($"Product Id: {row["ProductId"]}");
                    sb.AppendLine($"Product Name: {row["ProduuctName"]}");
                    sb.AppendLine($"Quantity: {row["Quantity"]}");
                    sb.AppendLine($"Price: {row["Price"]}");
                    sb.AppendLine($"Confirmation: {row["Confirmation"]}");
                    sb.AppendLine("-------------------------------------");
                }

                richTextBox1.Text = sb.ToString();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
           
            DialogResult result = MessageBox.Show(
       "Do you want to confirm your order?",
       "Confirm Order",
       MessageBoxButtons.YesNo,
       MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    // Update all orders for this buyer
                    string query = "UPDATE OrderInfo SET Confirmation = 'Confirmed' WHERE [Buyer-Id] = @BuyerId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@BuyerId", buyerId);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    con.Close();

                    if (rows > 0)
                    {
                        MessageBox.Show("Your order has been confirmed!",
                                        "Success",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                        LoadOrders(); // Refresh RichTextBox with updated status
                    }
                    else
                    {
                        MessageBox.Show("No orders found to confirm.",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel your entire order?",
                                                  "Cancel Order",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "UPDATE OrderInfo SET Confirmation = 'Cancelled' WHERE [Buyer-Id] = @BuyerId";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@BuyerId", buyerId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                MessageBox.Show("Your entire order has been cancelled.",
                                "Order Cancelled",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                LoadOrders(); 
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
           BuyerOrder bo = new BuyerOrder();
            bo.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit the application?", "Confirm Log Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

                Login bi = new Login();
                bi.Show();
                this.Close();
            }
        }
        }
    }

