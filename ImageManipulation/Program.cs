using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ImageManipulation
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var source = (Bitmap)Bitmap.FromFile("image.png");
            var texture = Texture.FromBitmap(source);

            var target = (Bitmap)new Bitmap(1024, 768);
            var buffer = ScreenBuffer.FromBitmap(target);
            var angle = (Angle) 0;

            var form = new DoubleBufferForm((2,2));
            form.StartPosition = FormStartPosition.CenterScreen;
            form.FormBorderStyle = FormBorderStyle.None;
            form.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    Application.Exit();
                    return;
                }
            };
            form.ClientSize = new Size(1024, 768);
            form.Show();

            // render

            var refrate = 10;

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = (int)Math.Round(1000.0 / refrate, 0);
            timer.Tick += (s, e) =>
            {
                var start = DateTime.Now;

                // prepare buffer

                buffer.Begin(Color.White);

                buffer.DrawImage(texture, angle, (512, 384));

                buffer.End();

                form.CurrentGraphics.DrawImage(target, 0, 0);
                form.Invalidate();

                // do something

                angle.RotateBy(5);


                // complete

                var stop = DateTime.Now;
                var frameRate = (int)Math.Round(1000.0 / (stop - start).TotalMilliseconds, 0);
            };
            timer.Start();

            Application.Run(form);
        }
    }
}
