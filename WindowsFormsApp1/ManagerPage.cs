using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class ManagerPage : BaseForm
    {
        public ManagerPage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // age ID "9294" hardcode kora chilo tai "Manager not found" ashto, ekhon je login korse tar ID
            new ManagerProfile(Session.UserId.ToString()).Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new AssignTask().Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new UpdateIssue().Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Inventory().Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new Supplies().Show();
            this.Hide();
        }

        // exit = logout, login page e fire jay (age shudhu ei window bondho hoto, app pichone cholte thakto)
        private void button6_Click(object sender, EventArgs e)
        {
            Session.Logout();
            new Login().Show();
            this.Close();
        }
    }
}
