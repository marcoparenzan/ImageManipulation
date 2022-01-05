using System.Drawing.Imaging;

namespace ImageManipulation
{
    public partial class ScreenBuffer
    {
        Bitmap target;
        int w, h;

        BitmapData data;
        unsafe byte* scan0;

        public static ScreenBuffer FromBitmap(Bitmap target)
        {
            return new ScreenBuffer
            {
                target = target,
                w = target.Width,
                h = target.Height
            };
        }

        unsafe public ScreenBuffer Begin(Color background)
        {
            this.data = target.LockBits(new System.Drawing.Rectangle(0,0, target.Width, target.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            byte* scan0 = (byte*)data.Scan0.ToPointer();
            int bytes = Math.Abs(data.Stride) * target.Height;
            for (var i = 0; i < bytes; i += 4)
            {
                scan0[0] = background.B; scan0++;
                scan0[0] = background.G; scan0++;
                scan0[0] = background.R; scan0++;
                scan0[0] = background.A; scan0++;
            }
            this.scan0 = (byte*)data.Scan0.ToPointer();

            return this;
        }

        public unsafe void SetPixel(double x, double y, Span<byte> color, int offset1)
        {
            // 2022.01.05: look at alpha channel!
            var offset = ((int)y) * w * 4 + ((int)x) * 4;
            scan0[offset++] = color[0];
            scan0[offset++] = color[1];
            scan0[offset++] = color[2];
            scan0[offset++] = color[3];
        }

        public unsafe void SetPixel(int offset, Span<byte> color)
        {
            // 2022.01.05: look at alpha channel!
            scan0[offset++] = color[0];
            scan0[offset++] = color[1];
            scan0[offset++] = color[2];
            scan0[offset++] = color[3];
        }

        public ScreenBuffer End()
        {
            target.UnlockBits(data);
            return this;
        }
    }
}
