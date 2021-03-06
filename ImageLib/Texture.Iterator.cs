using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageLib
{
    public partial class Texture
    {
        internal struct TextureTriangle
        {
            internal Texture texture;

            internal (double x, double y) p;
            internal (double x, double y) q;
            internal (double x, double y) dp;
            internal (double x, double y) dq;

            static internal byte[] faultColor = new byte[] { Color.Green.B, Color.Green.G, Color.Green.R, Color.Green.A };

            internal void NextScanLine()
            {
                p = (p.x + dp.x, p.y + dp.y);
                this.q = this.p;
            }

            internal Span<byte> Scan()
            {
                Span<byte> color = default;
                var offset = ((int)q.y) * texture.w * 4 + ((int)q.x) * 4;
                if (offset < 0 || offset >= texture.bits.Length)
                    color = faultColor;
                else
                    color = texture.bits.AsSpan(offset, 4);
                q = (q.x + dq.x, q.y + dq.y);
                return color;
            }
        }

        internal TextureTriangle UpperTriangle(Angle angle, Trigonometry tri)
        {
            var i = new TextureTriangle
            {
                texture = this
            };

            i.p = angle.q switch
            {
                0 => (w - 1, 0),
                1 => (w - 1, h - 1),
                2 => (0, h - 1),
                3 => (0, 0)
            };
            i.dp = angle.q switch
            {
                0 => (-tri.i_sin, 0),
                1 => (0, -tri.i_sin),
                2 => (tri.i_sin, 0),
                3 => (0, tri.i_sin),
            };
            i.dq = angle.q switch
            {
                0 => (tri.cos, tri.sin),
                1 => (-tri.sin, tri.cos),
                2 => (-tri.cos, -tri.sin),
                3 => (tri.sin, -tri.cos)
            };

            // reset pointer
            i.q = i.p;

            return i;
        }

        internal TextureTriangle LowerTriangle(Angle angle, Trigonometry tri)
        {
            var i = new TextureTriangle
            {
                texture = this
            };

            i.p = angle.q switch
            {
                0 => (0, 0),
                1 => (w - 1, 0),
                2 => (w - 1, h - 1),
                3 => (0, h - 1)
            };
            i.dp = angle.q switch
            {
                0 => (0, tri.i_cos),
                1 => (-tri.i_cos, 0),
                2 => (0, -tri.i_cos),
                3 => (tri.i_cos, 0),
            };
            i.dq = angle.q switch
            {
                0 => (tri.cos, tri.sin),
                1 => (-tri.sin, tri.cos),
                2 => (-tri.cos, -tri.sin),
                3 => (tri.sin, -tri.cos)
            };

            // reset pointer
            i.q = i.p;

            return i;
        }
    }
}
