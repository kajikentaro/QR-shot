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
        WindowStateHolder wsh;
        public Form2()
        {
            InitializeComponent();
            init();
            Bitmap img = capture();
            maximize();
            pictureBox1.Image = img;
        }
        private void init()
        {
            wsh = new WindowStateHolder();
            pictureBox1.ClientSize = wsh.size;
            pictureBox2.ClientSize = wsh.size;

            pictureBox1.Location = new Point(0, 0);
            pictureBox2.Location = new Point(0, 0);

            pictureBox2.Parent = pictureBox1;

            canvas = new Bitmap(wsh.width, wsh.height, PixelFormat.Format32bppArgb);
        }

        private void maximize()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            //this.WindowState = FormWindowState.Maximized;

            this.Location = new Point(wsh.sourceX, wsh.sourceY);
            this.Size = wsh.size;
        }
        private Bitmap capture()
        {
            Bitmap img = new Bitmap(wsh.width, wsh.height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(img);
            g.CopyFromScreen(wsh.sourceX, wsh.sourceY, 0, 0, wsh.size);
            return img;

            //img.Save("test.png", ImageFormat.Png);
            //System.Diagnostics.Process.Start(@".");
        }

        Point mouseDown;
        Bitmap canvas;
        bool click = false;

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            click = true;
            mouseDown = e.Location;
        }
        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (click == false) return;
            drawRect(e);
        }
        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            if (click == false) return;
            drawRect(e);
            click = false;
        }
        private void drawRect(MouseEventArgs e)
        {
            Point start = new Point();
            Point end = new Point();
            start.X = Math.Min(e.X, mouseDown.X);
            start.Y = Math.Min(e.Y, mouseDown.Y);
            end.X = Math.Max(e.X, mouseDown.X);
            end.Y = Math.Max(e.Y, mouseDown.Y);

            Console.WriteLine(start.X + " " + start.Y);
            Console.WriteLine(end.X + " " + end.Y);
            

            Pen blackPen = new Pen(Color.Black);

            // 描画する線を点線に設定
            blackPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            Graphics graphics = Graphics.FromImage(canvas);
            // 画面を消去
            graphics.Clear(Color.FromArgb(20, 0, 0, 0));
            pictureBox2.BackColor = Color.Transparent;
            //pictureBox2.Visible = false;


            graphics.DrawRectangle(blackPen, start.X, start.Y, Math.Abs(start.X - end.X),Math.Abs(start.Y - end.Y));


            pictureBox2.Image = canvas;
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
