using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phy.Entities
{
    public class Velocity
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Velocity(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
