using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    public class Point2D
    {
        private double m_X;

        public double X
        {
            set
            {
                m_X = value;
            }
            get
            {
                return m_X;
            }
        }

        private double m_Y;

        public double Y
        {
            set
            {
                m_Y = value;
            }
            get
            {
                return m_Y;
            }
        }
    }
}
