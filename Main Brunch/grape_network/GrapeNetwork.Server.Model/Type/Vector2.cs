using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeNetwork.UnitWorld.Type
{
    public class Vector2
    {
        public double x; 
        public double y;

        public static Vector2 zero = new Vector2(0, 0);
        public static Vector2 one = new Vector2(1, 1);

        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public Vector2()
        {

        }
    }
}
