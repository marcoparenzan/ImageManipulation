using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManipulation
{
    public partial class ScreenBuffer
    {
        public ScreenBuffer DrawImage(Texture texture, Angle angle, (double x, double y) c)
        {
            var tri = Trigonometry.FromDeg(angle.deg_q);

            var swap = angle.deg >= 90 && angle.deg < 180 || angle.deg >= 270 && angle.deg < 360;
            var w = swap ? texture.h : texture.w;
            var h = swap ? texture.w : texture.h;

            var w_c = w * tri.cos;
            var w_s = w * tri.sin;
            var h_c = h * tri.cos;
            var h_s = h * tri.sin;

            var v = new (double x, double y)[] { (0, w_s), (w_c, 0), (w_c + h_s, h_c), (h_s, w_s + h_c) };

            var vt = v[1];
            var vl = v[0];
            var vr = v[2];
            var vb = v[3];

            // x boundaries on each scanline
            var xl = vt.x;
            var xr = vt.x;
            // left and right x increments
            var dxl = -tri.ctg;
            var dxr = tri.tan;

            // vertices y on left and right to reach
            var yl = vl.y;
            var yr = vr.y;

            // texture
            var iterator = texture.UpperTriangle(angle, tri, Color.Blue);

            // prepare loop
            var y = vt.y;

            // center
            var cx = c.x - (v[2].x - v[0].x) / 2;
            var cy = c.y - (v[3].y - v[1].y) / 2;

            while (true) // first stripe until one left/right edge is reached 
            {
                if (y > vb.y)
                {
                    break;
                }
                if (y >= yl || xl < 0)
                {
                    yl = vb.y;
                    dxl = tri.tan;
                    xl = 0;
                    iterator = texture.LowerTriangle(angle, tri, Color.Green); // change slope in texture
                }
                else if (y >= yr) // evaluate if xr check is needed
                {
                    yr = vb.y;
                    dxr = -tri.ctg;
                }
                var x = xl;
                while (x < xr)
                {
                    SetPixel((int)(x + cx), (int)(y + cy), iterator.Scan());  // texture.Scan()
                    x++; // horizontal on screen
                }
                iterator.Next();
                xl += dxl;
                xr += dxr;
                y++;
            }

            return this;
        }
    }
}
