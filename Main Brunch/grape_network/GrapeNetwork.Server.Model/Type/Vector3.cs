using System;
using System.Collections.Generic;
using System.Text;

namespace GrapeNetwork.UnitWorld.Type
{
    public class Vector3
    {
        public double x;
        public double y;
        public double z;

        public static Vector3 zero = new Vector3(0,0,0);
        public static Vector3 one = new Vector3(1,1,1);

        public Vector3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vector3()
        {

        }
    }
}
