using System;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class ViewProducts : BaseForm
    {
        public ViewProducts()
        {
            InitializeComponent();
            Load += (s, e) => button2_Click(s, e); // khulle-i sob product dekhay
            dataGridView1.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string productName = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(productName))
            {
                button2_Click(sender, e);
                return;
            }
            DataTable dataTable = Db.Query("SELECT * FROM ProductInfo WHERE [Product Name] LIKE @ProductName", Db.P("@ProductName", "%" + productName + "%"));
            dataGridView1.DataSource = dataTable;
            if (dataTable.Rows.Count == 0)
                MessageBox.Show("No products found.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Db.Query("SELECT * FROM ProductInfo");
        }

        // age "Order" shudhu arekta page khulto, kono order toiri hoto na (code e kothao OrderInfo te insert chilo na).
        // ekhon select kora product er order hoy, stock check kore
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Please select a product first.", "Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataGridViewRow row = dataGridView1.CurrentRow;
            string productId = row.Cells["Product ID"].Value.ToString().Trim();
            string productName = row.Cells["Product Name"].Value.ToString().Trim();
            double price = Convert.ToDouble(row.Cells["Price/unit"].Value);
            int stock = Convert.ToInt32(row.Cells["Quantity"].Value);

            using (var dlg = new InputDialog("Order " + productName, "Quantity:"))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                if (!int.TryParse(dlg.Value(0), out int qty) || qty <= 0)
                {
                    MessageBox.Show("Quantity must be a positive number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (qty > stock)
                {
                    MessageBox.Show("Only " + stock + " in stock.", "Not Enough Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!int.TryParse(productId, out int pid))
                {
                    MessageBox.Show("This product has an invalid ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                double total = qty * price;
                if (MessageBox.Show(qty + " x " + productName + " = " + total.ToString("F2") + " Tk.\nPlace this order?", "Confirm Order",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                object next = Db.Scalar("SELECT ISNULL(MAX([Order-Id]), 0) + 1 FROM OrderInfo");
                if (next == null) return;
                int rows = Db.Execute(@"INSERT INTO OrderInfo ([Order-Id], [Product-Id], [Product Name], Quantity, Price, [Total price], [Buyer-Id], Confirmation)
                                        VALUES (@o, @p, @n, @q, @pr, @t, @b, 'Pending')",
                    Db.P("@o", Convert.ToInt32(next)), Db.P("@p", pid), Db.P("@n", productName), Db.P("@q", qty.ToString()),
                    Db.P("@pr", price), Db.P("@t", total), Db.P("@b", Session.UserId));
                if (rows > 0)
                    MessageBox.Show("Order placed! Order ID: " + next + ". The admin will accept it soon.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new BuyerPage().Show();
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
