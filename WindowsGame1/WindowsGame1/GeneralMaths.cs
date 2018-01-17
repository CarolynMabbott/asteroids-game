using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    class GeneralMaths
    {
        public const double TWO_PI = Math.PI * 2.0;
        public const double PI_OVER_TWO = Math.PI / 2.0;
        public const double THREE_PI_OVER_TWO = (3.0 * Math.PI) / 2.0;


        // REM the inverse tangent of an angle only gives 
        //     results from -90 to 90 degrees
        //	   this function calculates an angle between 
        //	   0 and 2 * PI radians (360 degrees)
        // NOTE tan = (opposite/ adjacent)
        //		therefore this needs to prevent division by zero 
        //		if the adjacent side is zero.
        // REM this function is working with double values
        //     so need to trap any values close to zero
        static public double ResolvedATan(double op, double adj)
        {
            // initialise result to zero
            double resultHdg = 0.0;

            // if the adjacent side is close to zero 
            // otherwise
            // the angle is 90 or 270 degrees (PI/2 or 3*PI/2)
            if (Math.Abs(adj) < 0.0000001)
            {
                // if the opposite side is NOT nearly zero
                if (Math.Abs(op) > 0.0000001)
                {
                    // if the opposite side is negative
                    if (op < 0.0)
                        resultHdg = THREE_PI_OVER_TWO; //270 degs
                    else // if the opposite side is positive
                        resultHdg = PI_OVER_TWO; // 90 degrees
                }
                // else the resultHdg is zero (already done)
            }
            else
            {
                // calculate the angle whose tangent = op/adj
                resultHdg = Math.Atan(op / adj);  // result is between -90..90 degrees
                // if the adjacent side is positive
                if (adj > 0)
                {
                    // if the opposite side is negative the result 
                    // is between (-90 .. 0) so add 360 degrees
                    if (op < 0.0)
                        resultHdg += TWO_PI;
                }
                else
                    // if the adjacent side is negative
                    resultHdg += Math.PI; // add 180 degrees (gives results from 90 .. 270)
            }
            // return the heading
            return resultHdg;
        }
    }
}
