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
    public partial class ManagerPage : Form
    {
        public ManagerPage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string loggedInManagerID = "9294";
            ManagerProfile profileForm = new ManagerProfile(loggedInManagerID);
            profileForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AssignTask assignTask = new AssignTask();
            assignTask.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UpdateIssue ui = new UpdateIssue();
            ui.Show();
            this.Hide(); 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Inventory inventory = new Inventory();
            inventory.Show();
            this.Hide();

        }

        private void button5_Click(object sender, EventArgs e)
        {

            Supplies supplies = new Supplies();
            supplies.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
