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
            capture();
            maximize();
        }

        private void maximize()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            //this.WindowState = FormWindowState.Maximized;

            WindowStateHolder wsh = new WindowStateHolder();
            this.Location = new Point(wsh.sourceX, wsh.sourceY);
            this.Size = wsh.size;
        }
        private void capture()
        {
            WindowStateHolder wsh = new WindowStateHolder();
            Bitmap img = new Bitmap(wsh.width, wsh.height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(img);
            g.CopyFromScreen(wsh.sourceX, wsh.sourceY, 0, 0, wsh.size);
            img.Save("test.png", ImageFormat.Png);
            System.Diagnostics.Process.Start(@".");

            pictureBox1.ClientSize = wsh.size;
            pictureBox1.Image = (Image)img;

        }
    }
    public class WindowStateHolder
    {
        const int inf = 100100100;
        public int sourceX { get; } = inf;
        public int sourceY { get; } = inf;
        public int destinationX { get; } = -inf;
        public int destinationY { get; } = -inf;
        public int height { get; }
        public int width{ get; }
        public Size size { get; }
        public WindowStateHolder(){
            for(int i=0;i< Screen.AllScreens.Length; i++)
            {
                Rectangle rect = Screen.AllScreens[i].Bounds;
                sourceX = Math.Min(sourceX, rect.X);
                sourceY = Math.Min(sourceY, rect.Y);
                destinationX = Math.Max(destinationX, rect.X + rect.Width);
                destinationY = Math.Max(destinationY, rect.Y + rect.Height);
            }
            width = destinationX - sourceX;
            height = destinationY - sourceY;
            size = new Size(width, height);
        }
    }
}
