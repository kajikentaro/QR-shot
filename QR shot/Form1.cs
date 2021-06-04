using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;

namespace QR_shot
{
    public partial class Form1 : Form
    {
        public string response { get; set; }

        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "ドラッグで画面上の二次元バーコードを選択してください。";
            richTextBox1.Refresh();
            shot.Text = "読み取り中…";
            shot.Refresh();
            Form2 form2 = new Form2();
            if (System.Windows.Forms.DialogResult.OK == form2.ShowDialog(this))
            {
                richTextBox1.Text = response;
                shot.Text = "読み取り開始";
            }
        }
        private void richTextBox1_LinkClicked_1(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://kajindowsxp.com/qr-shot");
        }

        private void shotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(null, null);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
