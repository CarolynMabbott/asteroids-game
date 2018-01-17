using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace WindowsGame1
{
    class Thruster
    {
        // a very simnple thruster
        // it would be possible to improve this by:
        //      permitting variable power; 
        //      adding rotational vectors to permit it to swivel
        //      changing the fuel consumption depending upon power;
        //      creating a separate fuel tank object;
        //      etc...
        double m_Fuel = 10000.0;
        double m_Fuel_Consumption = 0.000005;
        double m_Power = 30000.0;
        double m_CurrentBurnDurationMilleSecs = 0.0;
        Point2D m_RelativePosition = new Point2D();

        public Point2D RelativePosition
        {
            set
            {
                m_RelativePosition = value;
            }
            get
            {
                return m_RelativePosition;
            }
        }

        const double MAX_BURN_DURATION = 300.0; // if the thruster fire button is pressed and held down then released it will remain
                                                // on for 300 milleseconds

        void Init()
        {


        }

        // when the fire thruster is called: increase the time the thruster will burn for.
        public bool IncreaseBurnDuration(double milleSeconds)
        {

            if ((m_Fuel > 0.0) && (m_CurrentBurnDurationMilleSecs < MAX_BURN_DURATION))
            {
                m_CurrentBurnDurationMilleSecs += milleSeconds;
                if (m_CurrentBurnDurationMilleSecs > MAX_BURN_DURATION)
                {
                    m_CurrentBurnDurationMilleSecs = MAX_BURN_DURATION;
                }
                return true;
            }
            return false;
        }

        // reduce the fuel by the amount of power and time thruster is running
        bool BurnFuel(double power, double dtMilleSecs)
        {
             if (m_Fuel > 0.0)
             {
                 m_Fuel = m_Fuel - (power * m_Fuel_Consumption * dtMilleSecs);
                 Debug.WriteLine("fuel = " + m_Fuel.ToString());
                 return true;
             }
             return false;
        }

        // reduces burn time and fuel
        // if there is fuel and the thruster is firing then return power generated
        public double GetCurrentPower(double dtMilleSecs)
        {
            // if the thruster is firing
            if (m_CurrentBurnDurationMilleSecs > 0.0)
            {
                // if there is fuel then burn fuel for this time increment
                if (BurnFuel(m_Power, dtMilleSecs)) 
                {
                    // then reduce the burn time remaining
                    m_CurrentBurnDurationMilleSecs -= dtMilleSecs;
                    return m_Power;
                }
            }
            return 0.0; 
        }
    }
}
