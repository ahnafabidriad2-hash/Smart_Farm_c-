using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    // sob form eita theke ase, jate protita window er size, jayga, rong, font ek rokom hoy.
    // protita page er nijer design thik thake, shudhu ekta standard window er majhkhane boshe
    public class BaseForm : Form
    {
        public static readonly Size StandardClient = new Size(1280, 660);
        public const int HeaderHeight = 54;

        public static readonly Color Back = Color.FromArgb(238, 244, 236);
        public static readonly Color HeaderColor = Color.FromArgb(27, 67, 50);
        public static readonly Color Primary = Color.FromArgb(46, 125, 50);
        public static readonly Color Danger = Color.FromArgb(198, 40, 40);
        public static readonly Color TextColor = Color.FromArgb(33, 33, 33);
        public const string FontName = "Segoe UI";

        static readonly System.Collections.Generic.Dictionary<string, string> PageNames = new System.Collections.Generic.Dictionary<string, string>
        {
            { "LoadingScreen", "Welcome" }, { "Login", "Login" }, { "AdminPage", "Admin Dashboard" },
            { "WorkerDashboard", "Workers" }, { "WorkerRegistration", "Register Worker" }, { "BuyerDashboard", "Buyers" },
            { "BuyerRegister", "Register Buyer" }, { "FinanaceDashboard", "Finance" }, { "ProductDashboard", "Products" },
            { "Profile", "Admin Profile" }, { "ManagerPage", "Manager Dashboard" }, { "ManagerProfile", "Manager Profile" },
            { "AssignTask", "Assign Task" }, { "UpdateIssue", "Update Issues" }, { "Inventory", "Inventory" },
            { "Supplies", "Supplies" }, { "WorkerPage", "Worker Dashboard" }, { "WorkerProfile", "Worker Profile" },
            { "Viewtask", "My Tasks" }, { "Reportissue", "Report Issue" }, { "BuyerPage", "Buyer Dashboard" },
            { "ViewProducts", "Products" }, { "BuyerOrder", "My Orders" }, { "ShowOrders", "Order Summary" },
            { "Buyer_Profile", "My Profile" },
        };

        private static Icon appIcon;
        // test program eita false kore, jate window bondho hole app nije bondho na hoy
        public static bool ExitWhenLastWindowCloses = true;
        private static readonly Timer exitCheck = new Timer { Interval = 400 };
        private bool standardized;

        static BaseForm()
        {
            // shob window bondho hoye gele app o bondho, age pichone lukano form er jonno cholte thakto
            exitCheck.Tick += (s, e) =>
            {
                exitCheck.Stop();
                if (ExitWhenLastWindowCloses && !Application.OpenForms.Cast<Form>().Any(f => f.Visible)) Application.Exit();
            };
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode && LicenseManager.UsageMode != LicenseUsageMode.Designtime) Standardize();
            base.OnLoad(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            exitCheck.Stop();
            exitCheck.Start();
        }

        private void Standardize()
        {
            if (standardized) return;
            standardized = true;

            // designer e onek form er title "Form1", "Viewtask" er moto chilo, tai sundor naam dei
            string page;
            if (!PageNames.TryGetValue(GetType().Name, out page))
                page = string.IsNullOrWhiteSpace(Text) ? Name : Text.Trim();
            Size designed = ClientSize;
            var controls = Controls.Cast<Control>().ToList();

            // page er nijer jinish gula ekta panel e, ager position e
            var content = new Panel
            {
                Size = designed,
                BackColor = Color.Transparent,
                BackgroundImage = BackgroundImage,
                BackgroundImageLayout = BackgroundImageLayout
            };
            SuspendLayout();
            foreach (Control c in controls) content.Controls.Add(c);

            // page ta standard jaygar cheye boro hole choto kore boshai
            Size room = new Size(StandardClient.Width - 20, StandardClient.Height - HeaderHeight - 10);
            float scale = Math.Min(1f, Math.Min((float)room.Width / designed.Width, (float)room.Height / designed.Height));
            if (scale < 1f) content.Scale(new SizeF(scale, scale));

            var header = new Panel { Dock = DockStyle.Top, Height = HeaderHeight, BackColor = HeaderColor };
            header.Controls.Add(new Label
            {
                Text = "Smart Farm", ForeColor = Color.White, AutoSize = true,
                Font = new Font(FontName, 16, FontStyle.Bold), Location = new Point(20, 12)
            });
            var pageLabel = new Label
            {
                Text = page, ForeColor = Color.FromArgb(200, 230, 201), AutoSize = true,
                Font = new Font(FontName, 11)
            };
            header.Controls.Add(pageLabel);

            BackgroundImage = null;
            BackColor = Back;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            ClientSize = StandardClient;
            Controls.Add(content);
            Controls.Add(header);
            pageLabel.Location = new Point(StandardClient.Width - pageLabel.PreferredWidth - 20, 18);
            content.Location = new Point((StandardClient.Width - content.Width) / 2,
                HeaderHeight + (StandardClient.Height - HeaderHeight - content.Height) / 2);

            Text = "Smart Farm - " + page;
            Icon = AppIcon();
            StartPosition = FormStartPosition.CenterScreen;
            Style(content);
            ResumeLayout(true);
            CenterToScreen();
        }

        // vitorer sob jinish e ek rokom rong ar font
        public static void Style(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is Button b) StyleButton(b);
                else if (c is DataGridView g) StyleGrid(g);
                else if (c is Label l)
                {
                    l.ForeColor = TextColor;
                    l.BackColor = Color.Transparent;
                    l.Font = new Font(FontName, l.Font.Size, l.Font.Style);
                }
                else if (c is TextBoxBase t)
                {
                    t.Font = new Font(FontName, t.Font.Size, t.Font.Style);
                    t.BackColor = Color.White;
                    t.ForeColor = TextColor;
                }
                else if (c is ComboBox cb) cb.Font = new Font(FontName, cb.Font.Size, cb.Font.Style);
                else if (c is Panel || c is GroupBox)
                {
                    if (c.BackgroundImage == null) c.BackColor = Color.Transparent;
                }
                if (c.HasChildren && !(c is DataGridView)) Style(c);
            }
        }

        static readonly string[] DangerWords = { "back", "go back", "exit", "log out", "logout", "remove", "delete", "cancel" };

        public static void StyleButton(Button b)
        {
            string t = b.Text.Trim().ToLower();
            bool danger = DangerWords.Any(w => t == w || t.StartsWith(w + " "));
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = danger ? Danger : Primary;
            b.ForeColor = Color.White;
            // bold e lekha button e na dhorle (jemon "Confirm Orde") normal font
            var bold = new Font(FontName, Math.Max(9f, b.Font.Size), FontStyle.Bold);
            bool fits = TextRenderer.MeasureText(b.Text, bold).Width <= b.Width - 8;
            b.Font = fits ? bold : new Font(FontName, Math.Max(9f, b.Font.Size), FontStyle.Regular);
            b.Cursor = Cursors.Hand;
            b.UseVisualStyleBackColor = false;
        }

        public static void StyleGrid(DataGridView g)
        {
            g.BackgroundColor = Color.White;
            g.BorderStyle = BorderStyle.FixedSingle;
            g.EnableHeadersVisualStyles = false;
            g.ColumnHeadersDefaultCellStyle.BackColor = HeaderColor;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.Font = new Font(FontName, 10, FontStyle.Bold);
            g.DefaultCellStyle.Font = new Font(FontName, 10);
            g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(165, 214, 167);
            g.DefaultCellStyle.SelectionForeColor = TextColor;
            g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(241, 248, 233);
            // remove/confirm er jonno puro row select hote hobe, age shudhu cell select hoto
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // choto ekta sobuj icon, sob window e eki
        static Icon AppIcon()
        {
            if (appIcon != null) return appIcon;
            using (var bmp = new Bitmap(32, 32))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.Clear(Color.Transparent);
                    using (var br = new SolidBrush(Primary)) g.FillEllipse(br, 1, 1, 30, 30);
                    using (var f = new Font(FontName, 15, FontStyle.Bold))
                        TextRenderer.DrawText(g, "SF", f, new Rectangle(0, 0, 32, 32), Color.White,
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
                appIcon = Icon.FromHandle(bmp.GetHicon());
            }
            return appIcon;
        }
    }
}
