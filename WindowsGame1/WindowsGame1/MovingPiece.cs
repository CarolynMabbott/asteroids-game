using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    public class MovingPiece
    {
        public Point2D CurrentLocation = new Point2D();


        double m_SpeedMS;
        public double SpeedMS
        {
            set
            {
                m_SpeedMS = value;
            }
            get
            {
                return m_SpeedMS;
            }
        }

        double m_TravelDirectionRads;
        public double TravelDirectionRads
        {
            set
            {
                m_TravelDirectionRads = value;
            }
            get
            {
                return m_TravelDirectionRads;
            }
        }

        double m_RateOfRotationRadsSec;
        public double RateOfRotationRadsSec
        {
           set
            {
                m_RateOfRotationRadsSec = value;
            }
            get
            {
                return m_RateOfRotationRadsSec;
            }
        }

        public MovingPiece()
        {

        }

        public virtual bool Move(double dtMS)
        {

            // do incremental move of the piece
            Point2D tempLocation;

            if (MovementAssist.CalculateNewLocation(CurrentLocation, SpeedMS, TravelDirectionRads, dtMS, out tempLocation))
            {
                CurrentLocation = tempLocation;
                return true;
            }
            
            return false;
        }
    }
}
