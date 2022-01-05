using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageManipulation
{
    public partial class Texture
    {
        internal struct TextureIterator
        {
            internal Texture texture;

            internal (double x, double y) p;
            internal (double x, double y) q;
            internal (double x, double y) dp;
            internal (double x, double y) dq;

            internal Color faultColor;

            internal void Next()
            {
                p = (p.x + dp.x, p.y + dp.y);
                this.q = this.p;
            }

            internal Color Scan()
            {
                Color color = default;
                var offset = ((int)q.y) * texture.w * 4 + ((int)q.x) * 4;
                if (offset < 0 || offset >= texture.bits.Length)
                    color = faultColor;
                else
                    color = Color.FromArgb(BitConverter.ToInt32(texture.bits, offset));
                q = (q.x + dq.x, q.y + dq.y);
                return color;
            }
        }

        internal TextureIterator Q0(Angle angle, Trigonometry tri, Color faultColor)
        {
            var i = new TextureIterator
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

            i.faultColor = faultColor;

            return i;
        }

        internal TextureIterator Q1(Angle angle, Trigonometry tri, Color faultColor)
        {
            var i = new TextureIterator
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

            i.faultColor = faultColor;

            return i;
        }
    }
}
