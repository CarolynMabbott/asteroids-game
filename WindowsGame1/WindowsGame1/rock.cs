using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    public class rock : MovingPiece
    {
        const double ROCKET_MASS_KG = 5000.0;
        const double ROCKET_RADIUS = 10.0;  // radius 3 metres
        const double ROCKET_HEIGHT = 3.0;
        const double BURN_TIME_INCREMENT = 100.0;
        const double DISK_INERTIA_CONSTANT = 0.5;
        const double CROSS_SECTION = 20.0;  // 20 sq metres
        const double Cd = 0.3;
        //   const double AIR_DENSITY = 1.225; // KG/M3 N.B. in earth's atmosphere
        const double AIR_DENSITY = 0.025; // KG/M3

        double m_HeadingRads;
        public double HeadingRads
        {
            set
            {
                m_HeadingRads = value;
            }

            get
            {
                return m_HeadingRads;
            }
        }

        public rock()
        {
            // initial position
            CurrentLocation.X = 50;
            CurrentLocation.Y = 50;
            SpeedMS = 50;
            TravelDirectionRads = 2;

        }

        public Point2D GetRocketPosition()
        {
            return CurrentLocation;
        }

        public bool CalculateNewCourseAndSpeed(Vector powerVector, double dt, double massKG)
        {
            double accn = 0.0;

            //calculate the acceleration
            if (PhysicsMaths.CalculateAcceleration(powerVector.Magnitude, massKG, out accn))
            {
                Vector accnVector = new Vector();
                accnVector.Magnitude = accn * dt / 1000.0;
                accnVector.DirectionRads = powerVector.DirectionRads;
                Debug.WriteLine("accn = " + accn.ToString());
                Vector currentVector = new Vector();
                currentVector.Magnitude = base.SpeedMS;
                currentVector.DirectionRads = base.TravelDirectionRads;
                Vector newMovementVector = Vector.AddVectors(currentVector, accnVector);
                base.SpeedMS = newMovementVector.Magnitude;
                base.TravelDirectionRads = newMovementVector.DirectionRads;
                return true;
            }

            return false;
        }

    }
}
