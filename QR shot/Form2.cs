using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QR_shot
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void maximize()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }
        private void capture()
        {
            for(int i=0;i< Screen.AllScreens.Length; i++)
            {
                Rectangle rect = Screen.AllScreens[i].Bounds;
                Bitmap img = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(img);
                g.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size);
                img.Save("test" + i + ".png", ImageFormat.Png);
                System.Diagnostics.Process.Start(@".");

                pictureBox1.ClientSize = new Size(rect.Size.Width,rect.Height);
                pictureBox1.Image = (Image)img;
            }

        }
    }
}
