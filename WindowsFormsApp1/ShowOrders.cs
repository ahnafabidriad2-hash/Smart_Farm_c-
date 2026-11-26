using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class ShowOrders : BaseForm
    {
        // age buyerId kokhono set hoto na, tai order list faka thakto ar Confirm/Cancel chaplei crash korto
        private int buyerId => Session.UserId;

        public ShowOrders()
        {
            InitializeComponent();
            richTextBox1.ReadOnly = true;
            // Load event designer e lagano chilo na, tai order kokhono load hoto na
            Load += MyOrdersForm_Load;
        }

        private void MyOrdersForm_Load(object sender, EventArgs e)
        {
            LoadOrders();
        }

        private void LoadOrders()
        {
            richTextBox1.Clear();
            DataTable dt = Db.Query("SELECT [Order-Id], [Product-Id], [Product Name], Quantity, Price, [Total price], Confirmation FROM OrderInfo WHERE [Buyer-Id] = @BuyerId ORDER BY [Order-Id]",
                Db.P("@BuyerId", buyerId));
            if (dt.Rows.Count == 0)
            {
                richTextBox1.Text = "You have no orders.";
                return;
            }
            // age column er naam vul chilo ("OrderId", "ProduuctName"), tai crash korto
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                sb.AppendLine($"Order Id: {row["Order-Id"]}");
                sb.AppendLine($"Product: {row["Product Name"].ToString().Trim()} (ID {row["Product-Id"]})");
                sb.AppendLine($"Quantity: {row["Quantity"].ToString().Trim()}   Price: {row["Price"]} Tk");
                sb.AppendLine($"Total: {row["Total price"]} Tk");
                sb.AppendLine($"Status: {row["Confirmation"].ToString().Trim()}");
                sb.AppendLine("-------------------------------------");
            }
            richTextBox1.Text = sb.ToString();
        }

        // shudhu Pending order gula confirm hoy, admin je order accept/decline kore feleche seta bodlay na
        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to confirm your pending orders?", "Confirm Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            int rows = Db.Execute("UPDATE OrderInfo SET Confirmation = 'Confirmed' WHERE [Buyer-Id] = @BuyerId AND RTRIM(Confirmation) = 'Pending'",
                Db.P("@BuyerId", buyerId));
            if (rows > 0)
                MessageBox.Show("Your order has been confirmed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (rows == 0)
                MessageBox.Show("You have no pending orders to confirm.", "Confirm Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadOrders();
        }

        // admin accept korar age porjonto cancel kora jay
        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel your orders that are not accepted yet?", "Cancel Order", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
            int rows = Db.Execute("UPDATE OrderInfo SET Confirmation = 'Cancelled' WHERE [Buyer-Id] = @BuyerId AND RTRIM(Confirmation) IN ('Pending', 'Confirmed')",
                Db.P("@BuyerId", buyerId));
            if (rows > 0)
                MessageBox.Show(rows + " order(s) cancelled.", "Order Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (rows == 0)
                MessageBox.Show("There are no orders that can be cancelled.", "Cancel Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadOrders();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new BuyerOrder().Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out?", "Confirm Log Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Session.Logout();
                new Login().Show();
                this.Close();
            }
        }
    }
}
