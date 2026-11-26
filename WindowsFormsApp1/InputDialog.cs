using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    // choto ekta form, kichu field puron korte deye. report issue er moto jaygay lage
    public class InputDialog : Form
    {
        private readonly TextBox[] boxes;

        public InputDialog(string title, params string[] labels)
        {
            Text = title;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            BackColor = BaseForm.Back;
            Font = new Font(BaseForm.FontName, 10);
            ClientSize = new Size(420, 70 + labels.Length * 40);

            boxes = new TextBox[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                Controls.Add(new Label { Text = labels[i], Location = new Point(20, 22 + i * 40), AutoSize = true, ForeColor = BaseForm.TextColor });
                boxes[i] = new TextBox { Location = new Point(160, 19 + i * 40), Width = 230 };
                Controls.Add(boxes[i]);
            }
            var ok = new Button { Text = "OK", DialogResult = DialogResult.OK, Location = new Point(200, 25 + labels.Length * 40), Size = new Size(90, 32) };
            var cancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Location = new Point(300, 25 + labels.Length * 40), Size = new Size(90, 32) };
            BaseForm.StyleButton(ok);
            BaseForm.StyleButton(cancel);
            Controls.Add(ok);
            Controls.Add(cancel);
            AcceptButton = ok;
            CancelButton = cancel;
        }

        public string Value(int i)
        {
            return boxes[i].Text.Trim();
        }
    }
}
