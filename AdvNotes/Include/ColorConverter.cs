// Copyright (c) Adam Nathan.  All rights reserved.
//
// By purchasing the book that this source code belongs to, you may use and
// modify this code for commercial and non-commercial applications, but you
// may not publish the source code.
// THE SOURCE CODE IS PROVIDED "AS IS", WITH NO WARRANTIES OR INDEMNITIES.
using System;
using System.Windows.Media;

namespace AdvNotes.Include
{
    public struct HSVColor
    {
        public byte A; // Alpha
        public double H; // Hue
        public double S; // Saturation
        public double V; // Value

        public override string ToString()
        {
            string s = ColorConverter.HSVToRGB(this).ToString();
            // For simplicity, don't show the FF prefix for fully-opaque colors:
            if (s.StartsWith("#FF"))
                s = "#" + s.Substring(3);
            return s;
        }

        public static bool operator ==(HSVColor color1, HSVColor color2)
        {
            return (color1.A == color2.A &&
                    color1.H == color2.H &&
                    color1.S == color2.S &&
                    color1.V == color2.V);
        }

        public static bool operator !=(HSVColor color1, HSVColor color2)
        {
            return !(color1 == color2);
        }

        public override bool Equals(object obj)
        {
            if (obj is HSVColor)
            {
                HSVColor other = (HSVColor)obj;
                return (this == other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (this.A.GetHashCode() ^
                    this.H.GetHashCode() ^
                    this.S.GetHashCode() ^
                    this.V.GetHashCode());
        }
    }

    public static class ColorConverter
    {
        public static HSVColor RGBToHSV(Color rgbColor)
        {
            int max = Math.Max(rgbColor.R, Math.Max(rgbColor.G, rgbColor.B));
            int min = Math.Min(rgbColor.R, Math.Min(rgbColor.G, rgbColor.B));

            double hue = GetHue(rgbColor);
            double saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            double value = max / 255d;

            return new HSVColor { A = rgbColor.A, H = hue, S = saturation, V = value };
        }

        public static Color HSVToRGB(HSVColor hsvColor)
        {
            int hi = Convert.ToInt32(Math.Floor(hsvColor.H / 60)) % 6;
            double f = hsvColor.H / 60 - Math.Floor(hsvColor.H / 60);

            hsvColor.V = hsvColor.V * 255;
            byte v = Convert.ToByte(hsvColor.V);
            byte p = Convert.ToByte(hsvColor.V * (1 - hsvColor.S));
            byte q = Convert.ToByte(hsvColor.V * (1 - f * hsvColor.S));
            byte t = Convert.ToByte(hsvColor.V * (1 - (1 - f) * hsvColor.S));

            if (hi == 0)
                return Color.FromArgb(hsvColor.A, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(hsvColor.A, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(hsvColor.A, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(hsvColor.A, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(hsvColor.A, t, p, v);
            else
                return Color.FromArgb(hsvColor.A, v, p, q);
        }

        public static double GetHue(Color color)
        {
            float hue = 0;

            if (color.R == color.G && color.G == color.B)
                return hue;

            float r = ((float)color.R) / 255f;
            float g = ((float)color.G) / 255f;
            float b = ((float)color.B) / 255f;
            float max = Math.Max(r, Math.Max(g, b));
            float min = Math.Min(r, Math.Min(g, b));

            float delta = max - min;

            if (r == max)
                hue = (g - b) / delta;
            else if (g == max)
                hue = 2 + ((b - r) / delta);
            else if (b == max)
                hue = 4 + ((r - g) / delta);

            hue *= 60;
            if (hue < 0)
                hue += 360;

            return hue;
        }
    }
}