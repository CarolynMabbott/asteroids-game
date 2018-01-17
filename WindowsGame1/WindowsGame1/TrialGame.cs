using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame1
{
    public class TrialGame
    {
        Rocket testRocket = new Rocket();
        rock testRock = new rock();

        public bool MakeOneMove(double dt)
        {
            testRocket.Move(dt);
            testRock.Move(dt);
            return true;
        }

        public Rocket GetRocket()
        {
            return testRocket;
        }
        public rock getRock()
        {
            return testRock;
        }
    }
}
