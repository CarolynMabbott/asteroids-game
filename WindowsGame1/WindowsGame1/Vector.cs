using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    public class Vector
    {
        private double m_DirectionRads;
        public double DirectionRads
        {
            set
            {
                m_DirectionRads = value;
            }
            get
            {
                return m_DirectionRads;
            }
        }

        private double m_Magnitude;
        public double Magnitude
        {
            set
            {
                m_Magnitude = value;
            }
            get
            {
                return m_Magnitude;
            }
        }

        public Vector()
        {
            Magnitude = 0.0;
            DirectionRads = 0.0;
        }



        public static Vector AddVectors(Vector v1, Vector v2)
        {
          double dx1, dy1;
          double dx2, dy2;
          double newdx, newdy;

          dx1 = Math.Sin(v1.DirectionRads) * v1.Magnitude;
          dy1 = Math.Cos(v1.DirectionRads) * v1.Magnitude;

          dx2 = Math.Sin(v2.DirectionRads) * v2.Magnitude;
          dy2 = Math.Cos(v2.DirectionRads) * v2.Magnitude;

          newdx = dx1 + dx2;
          newdy = dy1 + dy2;

          Vector newv = new Vector();

          double magSqd = (newdx * newdx + newdy * newdy);

          if (magSqd > 0.0)
              newv.Magnitude = Math.Sqrt(magSqd);
          else
              newv.Magnitude = 0.0;

          newv.DirectionRads = GeneralMaths.ResolvedATan(newdx, newdy);

          return newv;
        }    
    }
}
