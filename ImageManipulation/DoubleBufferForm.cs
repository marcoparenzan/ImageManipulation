using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ImageManipulation
{
    public partial class DoubleBufferForm : Form
    {
        public DoubleBufferForm(int width, int height)
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.ClientSize = new Size(width, height);
            this.backBuffer = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
        }

        BufferedGraphics bufferedGraphics;
        Bitmap backBuffer;

        public bool Suspended { get; set; } = true;
        public int FrameRate { get; set; }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);

            if (this.bufferedGraphics != null)
            {
                this.Suspended = true;
                this.bufferedGraphics.Dispose();
                this.bufferedGraphics = null;
                this.backBuffer = null;
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.bufferedGraphics == null)
            {
                this.bufferedGraphics = BufferedGraphicsManager.Current.Allocate(e.Graphics, this.ClientRectangle);
                this.backBuffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height, PixelFormat.Format32bppArgb);
                this.Suspended = false;
            }
            else
            {
                this.bufferedGraphics.Graphics.DrawImage(backBuffer, 0, 0);
                this.bufferedGraphics.Render(e.Graphics);
            }
        }

        public Bitmap Backbuffer => backBuffer;
    }
}
