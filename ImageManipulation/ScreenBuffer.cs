using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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

        public unsafe void SetPixel(double x, double y, Color color)
        {
            var offset = ((int)y) * w * 4 + ((int)x) * 4;
            scan0[offset++] = color.B;
            scan0[offset++] = color.G;
            scan0[offset++] = color.R;
            scan0[offset++] = color.A;
        }

        public ScreenBuffer End()
        {
            target.UnlockBits(data);
            return this;
        }
    }
}
