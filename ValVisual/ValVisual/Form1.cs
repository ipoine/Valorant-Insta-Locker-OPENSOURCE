using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace ValVisual
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadComboBox();
        }

        private void LoadComboBox()
        {
            if (!Directory.Exists("agents"))
            {
                Directory.CreateDirectory("agents");
            }

            string[] files = Directory.GetFiles("agents", "*.png");
            comboBox1.Items.AddRange(files);
            comboBox1.Items.Remove("lock.PNG");
        }

        private void ClickOnImageAndLocation()
        {
            string imageFileName = comboBox1.SelectedItem.ToString();
            string imagePath = Path.Combine(imageFileName);

            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException("The image file does not exist.", imagePath);
            }

            Bitmap image = new Bitmap(imagePath);

            int x = -1;
            int y = -1;
            Rectangle bounds = Screen.PrimaryScreen.Bounds;

            while (x == -1 && y == -1)
            {
                using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(System.Drawing.Point.Empty, System.Drawing.Point.Empty, bounds.Size);
                    }

                    using (Mat mat1 = OpenCvSharp.Extensions.BitmapConverter.ToMat(bitmap))
                    using (Mat mat2 = OpenCvSharp.Extensions.BitmapConverter.ToMat(image))
                    using (Mat result = new Mat())
                    {
                        Cv2.MatchTemplate(mat1, mat2, result, TemplateMatchModes.CCoeffNormed);
                        Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out OpenCvSharp.Point maxLoc);
                        if (maxVal >= 0.95)
                        {
                            x = maxLoc.X + image.Width / 2;
                            y = maxLoc.Y + image.Height / 2;
                        }
                    }
                }

                if (x == -1 && y == -1)
                {
                    Thread.Sleep(1); // wait for half a second before trying again (lower the value is higher the ram useage)
                }
            }

            int lockX = -1;
            int lockY = -1;

            string lockImageFileName = "lock.PNG";
            string lockImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, lockImageFileName);
            if (!File.Exists(lockImagePath))
            {
                throw new FileNotFoundException("The image file does not exist.", lockImagePath);
            }

            Bitmap lockImage = new Bitmap(lockImagePath);
            bool lockFound = false;
            while (!lockFound)
            {
                using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(System.Drawing.Point.Empty, System.Drawing.Point.Empty, bounds.Size);
                    }

                    using (Mat mat1 = OpenCvSharp.Extensions.BitmapConverter.ToMat(bitmap))
                    using (Mat mat2 = OpenCvSharp.Extensions.BitmapConverter.ToMat(lockImage))
                    using (Mat result = new Mat())
                    {
                        Cv2.MatchTemplate(mat1, mat2, result, TemplateMatchModes.CCoeffNormed);
                        Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out OpenCvSharp.Point maxLoc);
                        if (maxVal >= 0.95)
                        {
                            lockFound = true;
                            lockX = maxLoc.X + lockImage.Width / 2;
                            lockY = maxLoc.Y + lockImage.Height / 2;
                        }
                    }
                }
            }

            //for (int i = 0; i < 3; i++)
            //{
            Cursor.Position = new System.Drawing.Point(x, y);
                Thread.Sleep(100);
                Mouse.LeftClick();
                Thread.Sleep(100);

                Cursor.Position = new System.Drawing.Point(lockX, lockY);
                Thread.Sleep(100);
                Mouse.LeftClick();
                Thread.Sleep(100);
            //}

            //for (int i = 0; i < 3; i++)
            //{

            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClickOnImageAndLocation();
        }

        public static class Mouse
        {
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            private static extern bool SetCursorPos(int x, int y);

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

            private const int MOUSEEVENTF_LEFTDOWN = 0x02;
            private const int MOUSEEVENTF_LEFTUP = 0x04;

            public static void LeftClick()
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
            }
        }
    }
}
