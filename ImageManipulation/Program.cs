using ImageLib;
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

            var form = new DoubleBufferForm(1024, 768);
            form.StartPosition = FormStartPosition.CenterScreen;
            //form.FormBorderStyle = FormBorderStyle.None;
            form.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    Application.Exit();
                    return;
                }
            };
            form.Show();

            // prepare resources

            var sprite = new Sprite();

            // render

            var refrate = 50;

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = (int)Math.Round(1000.0 / refrate, 0);
            timer.Tick += (s, e) =>
            {
                var start = DateTime.Now;

                // prepare buffer

                var backBuffer = ScreenBuffer.FromBitmap(form.Backbuffer);
                backBuffer.Begin(Colors.Green);

                backBuffer.DrawImage(sprite.Texture, sprite.Angle, (form.ClientRectangle.Width/2, form.ClientRectangle.Height / 2));

                backBuffer.End();

                form.Invalidate();

                // do something

                sprite.RotateRight();

                // complete

                var stop = DateTime.Now;
                var frameRate = (int)Math.Round(1000.0 / (stop - start).TotalMilliseconds, 0);

                form.Text = $"Framerate={frameRate}";
            };
            timer.Start();

            Application.Run(form);
        }
    }
}
