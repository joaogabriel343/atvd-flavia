using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace computaçãofc
{
    public partial class CustomMessageBox : Form
    {
        public CustomMessageBox()
        {
            InitializeComponent();
        }

        private void CustomMessageBox_Load(object sender, EventArgs e)
        {

        }
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            Form msgBox = new Form();
            msgBox.Text = caption;
            msgBox.StartPosition = FormStartPosition.CenterScreen;
            msgBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            msgBox.MinimizeBox = false;
            msgBox.MaximizeBox = false;
            msgBox.ControlBox = false; 
            msgBox.ShowInTaskbar = false;
            msgBox.BackColor = Color.Black; 

            Label lbl = new Label();
            lbl.Text = text;
            lbl.ForeColor = Color.Orange;
            lbl.Font = new Font("Arial", 10, FontStyle.Bold);
            lbl.AutoSize = true;
            lbl.Location = new Point(20, 20);
            msgBox.Controls.Add(lbl);

            int textWidth = TextRenderer.MeasureText(text, lbl.Font).Width;
            int textHeight = TextRenderer.MeasureText(text, lbl.Font).Height;

            int formWidth = Math.Max(250, textWidth + 80); 
            int formHeight = textHeight + 120;

            msgBox.Size = new Size(formWidth, formHeight);

            int buttonLeft = (formWidth - (buttons == MessageBoxButtons.OK ? 75 : (buttons == MessageBoxButtons.OKCancel ? 160 : 250))) / 2; // Centralizar os botões
            int buttonTop = msgBox.ClientSize.Height - 50;

            if (buttons == MessageBoxButtons.OK || buttons == MessageBoxButtons.OKCancel || buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel || buttons == MessageBoxButtons.AbortRetryIgnore || buttons == MessageBoxButtons.RetryCancel)
            {
                Button btnOK = new Button();
                btnOK.Text = "OK";
                btnOK.DialogResult = DialogResult.OK;
                btnOK.FlatStyle = FlatStyle.Flat;
                btnOK.BackColor = Color.Orange;
                btnOK.ForeColor = Color.Black; 
                btnOK.FlatAppearance.BorderSize = 0; 
                btnOK.Font = new Font("Arial", 9, FontStyle.Bold);
                btnOK.Size = new Size(75, 25);
                btnOK.Location = new Point(buttonLeft, buttonTop);
                msgBox.Controls.Add(btnOK);

                if (buttons == MessageBoxButtons.OKCancel || buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel || buttons == MessageBoxButtons.AbortRetryIgnore || buttons == MessageBoxButtons.RetryCancel)
                {
                    Button btnCancel = new Button();
                    btnCancel.Text = "Cancelar";
                    btnCancel.DialogResult = DialogResult.Cancel;
                    btnCancel.FlatStyle = FlatStyle.Flat;
                    btnCancel.BackColor = Color.Orange; 
                    btnCancel.ForeColor = Color.Black; 
                    btnCancel.FlatAppearance.BorderSize = 0;
                    btnCancel.Font = new Font("Arial", 9, FontStyle.Bold);
                    btnCancel.Size = new Size(75, 25);
                    btnCancel.Location = new Point(buttonLeft + 85, buttonTop); 
                    msgBox.Controls.Add(btnCancel);
                }
            }

            if (icon != MessageBoxIcon.None)
            {
                PictureBox iconPb = new PictureBox();
                iconPb.SizeMode = PictureBoxSizeMode.StretchImage;
                iconPb.Size = new Size(32, 32);
                iconPb.Location = new Point(msgBox.Width - 60, 20); 

                switch (icon)
                {
                    case MessageBoxIcon.Error:
                        iconPb.Image = SystemIcons.Error.ToBitmap();
                        break;
                    case MessageBoxIcon.Information:
                        iconPb.Image = SystemIcons.Information.ToBitmap();
                        break;
                    case MessageBoxIcon.Warning:
                        iconPb.Image = SystemIcons.Warning.ToBitmap();
                        break;
                    case MessageBoxIcon.Question:
                        iconPb.Image = SystemIcons.Question.ToBitmap();
                        break;
                }
                msgBox.Controls.Add(iconPb);
            }

            return msgBox.ShowDialog();
        }
    
    }
}
