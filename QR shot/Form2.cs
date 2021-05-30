using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace QR_shot
{
    public partial class Form2 : Form
    {
        WindowStateHolder wsh;
        Bitmap img;
        public Form2()
        {
            InitializeComponent();
            init();
            img = capture();
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
            pictureBox2.BackColor = Color.FromArgb(50,255,255,255);

            canvas = new Bitmap(wsh.width, wsh.height, PixelFormat.Format32bppArgb);
            graphics = Graphics.FromImage(canvas);

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
            g.Dispose();
            return img;

            //img.Save("test.png", ImageFormat.Png);
            //System.Diagnostics.Process.Start(@".");
        }

        Graphics graphics;
        Bitmap canvas;
        bool selecting = false;
        bool clickSelectionMode = false;

        private void readQRandResponse(Bitmap qrImg)
        {
            IBarcodeReader reader = new BarcodeReader();
            var result = reader.Decode(qrImg);
            Form1 form1 = this.Owner as Form1;
            if (result != null) {
                form1.response = result.Text;
            }
            else
            {
                form1.response = "読み取りに失敗しました。";
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();

        }
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if(clickSelectionMode == true)
            {
                RectStateHolder rsh = new RectStateHolder(e.X, e.Y);
                Bitmap qrImg = cutImg(img, rsh.start.X, rsh.start.Y, rsh.width, rsh.height);
                readQRandResponse(qrImg);
                return;
            }
            selecting = true;
            RectStateHolder.mouseDown = e.Location;
        }
        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (selecting == false) return;
            drawRect(e);
        }
        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            if (selecting == false) return;
            drawRect(e);
            selecting = false;

            RectStateHolder rsh = new RectStateHolder(e.X, e.Y);
            if(rsh.width == 0 || rsh.height == 0)
            {
                clickSelectionMode = true;
                selecting = true;
                return;
            }
            Bitmap qrImg = cutImg(img, rsh.start.X, rsh.start.Y, rsh.width, rsh.height);
            readQRandResponse(qrImg);
        }
        private Bitmap cutImg(Bitmap img,int sourceX, int sourceY, int width, int height)
        {
            Bitmap c = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(c);
            Rectangle srcRect = new Rectangle(sourceX, sourceY, width, height);
            Rectangle desRect = new Rectangle(0, 0, width, height);
            g.DrawImage(img, desRect, srcRect, GraphicsUnit.Pixel);
            g.Dispose();
            //c.Save("test.png", ImageFormat.Png);
            //System.Diagnostics.Process.Start(".");
            return c;
        }
        private void drawRect(MouseEventArgs e)
        {
            graphics.Clear(Color.Transparent);
            RectStateHolder rsh = new RectStateHolder(e.X, e.Y);


            Pen pen = new Pen(Color.Black);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            pen.Width = 3;
            graphics.DrawRectangle(pen, rsh.start.X, rsh.start.Y, rsh.width, rsh.height);

            SolidBrush blueBrush = new SolidBrush(Color.FromArgb(15,0,0,0));
            graphics.FillRectangle(blueBrush, rsh.start.X, rsh.start.Y, rsh.width, rsh.height);

            pictureBox2.Image = canvas;
        }
    }
    public class RectStateHolder
    {
        public static Point mouseDown { get; set; }
        public Point start{ get; set; }
        public Point end { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        
        public RectStateHolder(int x, int y)
        {
            this.start = new Point(Math.Min(x, mouseDown.X), Math.Min(y, mouseDown.Y));
            this.end= new Point(Math.Max(x, mouseDown.X), Math.Max(y, mouseDown.Y));
            this.width = Math.Abs(this.start.X - this.end.X);
            this.height = Math.Abs(this.start.Y - this.end.Y);
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
