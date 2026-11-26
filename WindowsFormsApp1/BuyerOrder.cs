using System;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    // buyer er nijer order er page. age eikhane sob buyer er order dekha jeto
    public partial class BuyerOrder : BaseForm
    {
        public BuyerOrder()
        {
            InitializeComponent();
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
            DataTable dataTable = Db.Query("SELECT * FROM OrderInfo WHERE [Buyer-Id] = @b AND [Product Name] LIKE @ProductName",
                Db.P("@b", Session.UserId), Db.P("@ProductName", "%" + productName + "%"));
            dataGridView1.DataSource = dataTable;
            if (dataTable.Rows.Count == 0)
                MessageBox.Show("No orders found for that product.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Db.Query("SELECT * FROM OrderInfo WHERE [Buyer-Id] = @b ORDER BY [Order-Id]", Db.P("@b", Session.UserId));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new ShowOrders().Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new BuyerPage().Show();
            this.Close();
        }

        private void BuyerOrder_Load(object sender, EventArgs e)
        {
            button2_Click(sender, e);
        }
    }
}
