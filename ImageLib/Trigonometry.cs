using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageLib
{
    public struct Trigonometry
    {
        public double rad { get; private set; }
        public double sin { get; private set; }
        public double cos { get; private set; }
        public double tan { get; private set; }
        public double ctg { get; private set; }
        public double i_sin { get; private set; }
        public double i_cos { get; private set; }

        public static Trigonometry FromDeg(double deg) => (Trigonometry)(deg * Math.PI / 180);

        Trigonometry Init(double value_rad)
        {
            this.rad = value_rad;
            this.sin = Math.Round(Math.Sin(rad), 3);
            this.cos = Math.Round(Math.Cos(rad), 3);
            this.tan = this.sin / this.cos;
            this.ctg = this.cos / this.sin;
            this.i_sin = Math.Round(1 / Math.Sin(rad), 3);
            this.i_cos = Math.Round(1 / Math.Cos(rad), 3);
            return this;
        }

        public static implicit operator Trigonometry(double value_rad) => (new Trigonometry()).Init(value_rad);

        public override string ToString()
        {
            return $"{rad}";
        }
    }
}
