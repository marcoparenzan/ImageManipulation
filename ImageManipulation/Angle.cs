using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManipulation
{
    public struct Angle
    {
        public double deg { get; private set; }
        public double deg_q { get; private set; }
        public int q { get; private set; }

        Angle Init(double value_deg)
        {
            this.deg = value_deg;
            this.deg_q = value_deg % 90;
            this.q = (int) ((value_deg + 360) % 360) / 90;
            return this;
        }

        public static implicit operator Angle(double value_deg) => (new Angle()).Init(value_deg);

        public void RotateBy(double value)
        {
            Init((360 + deg + value) % 360);
        }

        public override string ToString()
        {
            return $"{deg}";
        }
    }
}
