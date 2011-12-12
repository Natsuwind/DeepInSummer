using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Natsuhime.TXTReader
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            G.FontColor = tbxForeColor.Text.Trim();
            G.BackColor = tbxBlankColor.Text.Trim();
            G.FontSize = Convert.ToInt32(this.tbxFont.Text.Trim());
            this.DialogResult = DialogResult.OK;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                tbxBlankColor.Text = Convert.ToString(colorDialog1.Color.ToArgb(), 16); ;
            }
        }

        private void lbChooseFont_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                //tbxBlankColor.Text=fontDialog1.
                Font font = fontDialog1.Font;
                //tbxFont.Text = string.Format("{0},{1},{2},{3}", font.FontFamily.Name, font.Style.ToString(), font.Size, fontDialog1.Color.Name);
                tbxFont.Text = fontDialog1.Color.Name;
            }
        }

        private void lbForeColorSelector_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                tbxForeColor.Text = Convert.ToString(colorDialog1.Color.ToArgb(), 16);
            }
        }

        private void btnUnSet_Click(object sender, EventArgs e)
        {
            this.tbxForeColor.Text = "ff000000";
            this.tbxBlankColor.Text = "ffc0c0c0";
            this.tbxFont.Text = "14";
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            this.tbxForeColor.Text = G.FontColor;
            this.tbxBlankColor.Text = G.BackColor;
            this.tbxFont.Text = G.FontSize.ToString();
        }
    }
}
