using System;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    public static class MovementAssist
    {    
        /// <summary>
        /// Rotate a vector through an angle in radians
        /// </summary>D:\jim\teaching materials\CS2S505 Team Project Game Dev\XNA Trials\WindowsGame1\WindowsGame1\WindowsGame1\MovementAssist.cs
        /// <param name="rotationRadians"></param>
        /// <param name="inVector"></param>
        /// <param name="resultVector"></param>
        /// <returns></returns>
        static public bool RotateVector2(float rotationRadians, Vector2 inVector, out Vector2 resultVector)
        {
            Matrix rotMatrix;
            Matrix.CreateRotationZ(rotationRadians, out rotMatrix);
            resultVector = Vector2.Transform(inVector, rotMatrix);
            return true;
        }

        static public double RotateHeadingRadians(double rateOfRotationRadiansSec, double headingRadians, double dtMS)
        {
            headingRadians = headingRadians + (rateOfRotationRadiansSec * dtMS) / 1000.0;

            while (headingRadians > GeneralMaths.TWO_PI)
                headingRadians -= GeneralMaths.TWO_PI;
            while (headingRadians < 0.0)
                headingRadians += GeneralMaths.TWO_PI;
            return headingRadians;
        }

        static public bool RotatePoint(Point2D rotationPoint, Point2D pointToRotate, double angleRads, out Point2D resultantPoint)
        {
            // translate to rotation point zero
            double dx = pointToRotate.X - rotationPoint.X;
            double dy = pointToRotate.Y - rotationPoint.Y;

            // rotate 
            double cosAngle = Math.Cos(angleRads);
            double sinAngle = Math.Sin(angleRads);
            double rotX = dx * cosAngle + dy * sinAngle;
            double rotY = dy * cosAngle - dx * sinAngle;

            resultantPoint = new Point2D();
            // translate back
            resultantPoint.X = rotationPoint.X + rotX;
            resultantPoint.Y = rotationPoint.Y + rotY;

            return true;
        }

        static public bool CalculateNewLocation(Point2D oldLocation, double speedMS, double directionOfTravelRads, double dtMilleSecs, out Point2D newLocation)
        {
            newLocation = new Point2D();
            double incrementalDist;
            if (CalculateIncrementalDistance(speedMS, dtMilleSecs, out incrementalDist))
            {
                Point2D rotPT = new Point2D();
                Point2D tempPT = new Point2D();
                Point2D resultPT;
                tempPT.Y = incrementalDist;

                if (RotatePoint(rotPT, tempPT, directionOfTravelRads, out resultPT))
                {
                    newLocation.X = oldLocation.X + resultPT.X;
                    newLocation.Y = oldLocation.Y + resultPT.Y;                
                    return true;
                }
                return true;
            }
            return false;
        }



        static public bool CalculateIncrementalDistance(double speedMS, double dtMilleSecs, out double incrementalDistance)
        {
            incrementalDistance = (speedMS * dtMilleSecs) / 1000.0;
            return true;
        }



    }
}
