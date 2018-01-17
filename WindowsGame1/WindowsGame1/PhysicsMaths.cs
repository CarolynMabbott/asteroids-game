using System;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    public static class PhysicsMaths
    {

        /// <summary>
        /// Calculate the acceleration of an object using Newtons's second law F = MA
        /// </summary>
        /// Preconditions
        /// The mass in KG must be a positive value
        /// The result of the calculation must not result in an infinite value
        /// <param name="forceNewtons"> the resultant force in Newtons acting on the body</param>
        /// <param name="massKG"> the mass of the body in KG 
        /// NOTE: weight depends on gravity Mass does not. 
        /// The effect of the weight of an object should be applied to the force in Newtons
        /// before calling this function (if required).
        /// Similarly the effect of friction should have been applied to the force 
        /// before calling this function (if required)
        /// </param>
        /// <returns> The acceleration in Metres per second </returns>
        static public bool CalculateAcceleration(double forceNewtons, double massKG, out double accelerationMetresSecSqd)
        {
            // an out parameter must be assigned before returning
            // so set it to zero as a fail safe
            accelerationMetresSecSqd = 0.0;
            try
            {
                //Prevent illegeal values being used in the calculation
                //Also prevent the use of a massKG that is not positive
                //NOTE division by zero may not always cause an exception in all languages 
                if (Double.IsNaN(forceNewtons) || Double.IsNaN(massKG) || (massKG < Double.Epsilon))
                {
                    return false;
                }

                accelerationMetresSecSqd = forceNewtons / massKG;
                //if the result is illegal or infinity
                if (Double.IsNaN(accelerationMetresSecSqd) || Double.IsInfinity(accelerationMetresSecSqd))
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }

        static public bool CalculateSpeed(double currentSpeedMS, double accelerationMetresSecsSqd, double dt, out double newSpeed)
        {
            newSpeed = currentSpeedMS + ((accelerationMetresSecsSqd * dt)/1000.0);
            return true;
        }

        static public bool CalculateIncrementalDistance(double oldSpeedMS, double newSpeedMS, double dt, out double incrementalDistance)
        {
            incrementalDistance = ((oldSpeedMS + newSpeedMS)/2.0) * (dt/1000.0);
            return true;
        }




        // formula for drag is = .5 * fluid density * velocity squared  * reference area  * coefficient of drag
        // reference area is generally the cross sectional area
        static public bool CalculateFriction(double speedMS, double referenceAreaMSqd, double cd,
                                             double fluidDensity, out double frictionNewtons)
        {
            frictionNewtons = 0.0;
            try
            {
                double speedSquared = speedMS * speedMS;

                frictionNewtons = .5 * fluidDensity * speedSquared * referenceAreaMSqd * cd;

                if (Double.IsInfinity(frictionNewtons))
                {
                    return false;
                }
            }
            catch (Exception ex) //Defensive code
            {
                Debug.WriteLine("Friction calculation failed : " + ex.ToString());
                return false;
            }

            return true;
        }

        // REM this is a value based upon the flywheel information at:
        // NOTE: A more accurate solution would need to integrate the disc from the centre outwards
        //  http://www.nicholasmeeker.com/files/flywheel_report.pdf 
        // Pi * ρ * w^2 (2/5 r^5 + Dr^4)
 /*       static public bool CalculateRotationalFrictionTorqueOfDisc(double radius, double discHeight,  double rateOfRotation, double fluidDensity, out double frictionTorque)
        {
            frictionTorque = 0.0;

            double p1 = Math.PI * fluidDensity * rateOfRotation * rateOfRotation;

            double p2 = (2.0 / 5.0) * (Math.Pow(radius, 5) + discHeight * (Math.Pow(radius, 4)));

            frictionTorque = p1 * p2;

            return true;
        } */

        // REM this is a heuristic value 
       static public bool CalculateRotationalFrictionTorqueOfDisc(double radius, double discHeight,  double rateOfRotation, double fluidDensity, out double frictionTorque)
       {
           frictionTorque = 0.0;

           const double ROTATIONAL_FRICTION_CONSTANT = 1000.0;

           double rotRadeCubed = rateOfRotation * rateOfRotation * rateOfRotation;

           double p1 = fluidDensity * rotRadeCubed * Math.PI * radius * radius * discHeight * ROTATIONAL_FRICTION_CONSTANT;

           frictionTorque = p1;

           return true;
       } 


        // formula for moment of inertia of a homogeneous disk I = m * rsqd * InertiaKonstant (Varies for shape)
        static public bool CalculateMomentOfInertia(double konstantOfInertia, double mass, double radius, out double inertia)
        {
            inertia = 0.0;
            try
            {
                double radiusSquared = radius * radius;


                inertia = (mass * radiusSquared) * konstantOfInertia;

                if (Double.IsInfinity(inertia))
                {
                    return false;
                }
            }
            catch (Exception ex) //Defensive code
            {
                Debug.WriteLine("Inertia calculation failed : " + ex.ToString());
                return false;
            }

            return true;
        }

        static public bool CalculateRotationalAcceleration(double torqueNewtons, double momentOfInertia, out double accelerationRadsSecSqd)
        {
            // an out parameter must be assigned before returning
            // so set it to zero as a fail safe
            accelerationRadsSecSqd = 0.0;
            try
            {
                // Prevent illegal values being used in the calculation
                // Also prevent the use of a massKG that is not positive. 
                // NOTE division by zero may not always cause an exception in all languages (check compiler options)                
                if (Double.IsNaN(torqueNewtons) || Double.IsNaN(momentOfInertia) || (momentOfInertia < Double.Epsilon))
                {
                    return false;
                }

                accelerationRadsSecSqd = torqueNewtons / momentOfInertia;
                // if the result is illegal or Infinity
                if (Double.IsNaN(accelerationRadsSecSqd) || Double.IsInfinity(accelerationRadsSecSqd))
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }

    }
}
