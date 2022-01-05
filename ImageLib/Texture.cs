using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageLib
{
    public partial class Texture
    {
        public int w { get; private set; }
        public int h { get; private set; }

        byte[] bits;

        unsafe public static Texture FromBitmap(Bitmap source)
        {
            var newTexture = new Texture
            {
                w = source.Width,
                h = source.Height
            };

            var data = source.LockBits(new System.Drawing.Rectangle(0, 0, source.Width, source.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int bytes = Math.Abs(data.Stride) * source.Height;
            newTexture.bits = new byte[bytes];
            Marshal.Copy(data.Scan0, newTexture.bits, 0, bytes);
            source.UnlockBits(data);

            return newTexture;
        }
    }
}
