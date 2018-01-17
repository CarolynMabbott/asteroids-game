using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    public class Rocket: MovingPiece
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

        Thruster m_PortThruster = new Thruster();
        Thruster m_StarboardThruster = new Thruster();

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

        public Rocket()
        {
            // initial position
            CurrentLocation.X = 100;
            CurrentLocation.Y = 100;

            // temp location to set thruster positions relative to the rocket centre of mass
            Point2D locL = new Point2D();
            locL.X = -1.0;
            locL.Y = -0.2;
            m_PortThruster.RelativePosition = locL;
            Point2D locR = new Point2D();
            locR.X = -1.0;
            locR.Y = 0.2;
            m_StarboardThruster.RelativePosition = locR;

        }

        public Point2D GetRocketPosition()
        {
            return CurrentLocation;
        }

        // increases the time for both the thrusters to burn (E.G. when a key is pressed)
        public bool fireBothThrusters()
        {
            m_PortThruster.IncreaseBurnDuration(BURN_TIME_INCREMENT);
            m_StarboardThruster.IncreaseBurnDuration(BURN_TIME_INCREMENT);
            return true;
        }

        public bool firePortThruster()
        {
            m_PortThruster.IncreaseBurnDuration(BURN_TIME_INCREMENT);
            return true;
        }

        public bool fireStarboardThruster()
        {
            m_StarboardThruster.IncreaseBurnDuration(BURN_TIME_INCREMENT);
            return true;
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

        public bool CalculateForceAndTorque(double dt, out double populsiveForce, out double rotTorque)
        {
            populsiveForce = 0.0;
            rotTorque = 0.0;
            double portPower = m_PortThruster.GetCurrentPower(dt);
            double stbdPower = m_StarboardThruster.GetCurrentPower(dt);

            populsiveForce = portPower + stbdPower;

            double rotLeftTorque = stbdPower * m_StarboardThruster.RelativePosition.Y; 
            double rotRightTorque = portPower * m_PortThruster.RelativePosition.Y;

            rotTorque = rotLeftTorque + rotRightTorque;
            Debug.WriteLine("rotLeftY = " + m_StarboardThruster.RelativePosition.Y.ToString());
            Debug.WriteLine("rotRightY = " + m_PortThruster.RelativePosition.Y.ToString());
            Debug.WriteLine("rotLeftTorque = " + rotLeftTorque.ToString());
            Debug.WriteLine("rotRightTorque = " + rotRightTorque.ToString()); 
            Debug.WriteLine("rotTorque = " + rotTorque.ToString());
            return true;
        }



        static public bool CalculateResultantForceVector(   double powerDirectionRads, 
                                                            double powerMagnitude, 
                                                            double frictionDirectionRads, 
                                                            double frictionMagnitude, 
                                                            out Vector resultVector)
        {
            // REM remove next line when changing the code
            Vector powerVector = new Vector();
            powerVector.DirectionRads = powerDirectionRads;
            powerVector.Magnitude = powerMagnitude;

            Vector frictionVector = new Vector();
            frictionVector.DirectionRads = frictionDirectionRads;
            frictionVector.Magnitude = frictionMagnitude;
            resultVector = Vector.AddVectors(powerVector, frictionVector);
            return true;
        } 

        public override bool Move(double dt)
        {
            Debug.WriteLine("dt = " + dt.ToString());
            // currently just doing forward motion
            double forwardPropulsionForce;
            double rotationalTorque;

            if (CalculateForceAndTorque(dt, out forwardPropulsionForce, out  rotationalTorque) == false)
            {
                return false;
            }

            double I;

            if (PhysicsMaths.CalculateMomentOfInertia(DISK_INERTIA_CONSTANT, ROCKET_MASS_KG, ROCKET_RADIUS, out I) == false)
            {
                return false;
            }

            Debug.WriteLine("forwardPropulsionForce = " + forwardPropulsionForce.ToString());

            Debug.WriteLine("rotationalTorque = " + rotationalTorque.ToString());


            double rotationalFriction;

            if (PhysicsMaths.CalculateRotationalFrictionTorqueOfDisc(ROCKET_RADIUS, ROCKET_HEIGHT, RateOfRotationRadsSec, AIR_DENSITY, out rotationalFriction) == false)
            {
                return false;
            }

            Debug.WriteLine("rotationalFriction = " + rotationalFriction.ToString());
            Debug.WriteLine("RateOfRotationRadsSec = " + RateOfRotationRadsSec.ToString());


            rotationalTorque = rotationalTorque - rotationalFriction;
            double rotAccn;

            if (PhysicsMaths.CalculateRotationalAcceleration(rotationalTorque, I, out rotAccn) == false)
            {
                return false;
            }
            // calc rate of rotation
            RateOfRotationRadsSec += rotAccn;

            // calc new heading
            HeadingRads = MovementAssist.RotateHeadingRadians(RateOfRotationRadsSec, HeadingRads, dt);

            double friction = 0.0;
            double resultantForce = 0.0;
       
            if (PhysicsMaths.CalculateFriction(SpeedMS, CROSS_SECTION, Cd, AIR_DENSITY, out friction))
            {
                Vector powerAndFrictionVector;
                Debug.WriteLine("HeadingRads = " + HeadingRads.ToString());
                Debug.WriteLine(" TravelDirectionRads = " + TravelDirectionRads.ToString());
                // calculate the resultant force acting on the 
                if (CalculateResultantForceVector(HeadingRads, forwardPropulsionForce, TravelDirectionRads + Math.PI, friction, out powerAndFrictionVector) == false)
                {
                    return false;
                }


                Debug.WriteLine("powerAndFrictionVector.Magnitude = " + powerAndFrictionVector.Magnitude.ToString());

                if (CalculateNewCourseAndSpeed(powerAndFrictionVector, dt, ROCKET_MASS_KG) == false)
                {
                    return false;
                }
            }


            Debug.WriteLine("SpeedMS = " + SpeedMS.ToString());
            
            return base.Move(dt);
        }
    } 
}
