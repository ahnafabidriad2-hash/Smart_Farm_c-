using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class BuyerPage : BaseForm
    {
        public BuyerPage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Buyer_Profile bup = new Buyer_Profile();
            bup.Show();        
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // worker er moto buyer o logout er age jiggesh kore
            if (MessageBox.Show("Are you sure you want to log out?", "Confirm Log Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            Session.Logout();
            Login lb = new Login();
            lb.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ViewProducts vp = new ViewProducts();
            vp.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            BuyerOrder bo = new BuyerOrder();
            bo.Show();
        }
    }
}
