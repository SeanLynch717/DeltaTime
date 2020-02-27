using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace deltaTime
{
    struct Circle
    {
        public float Radius { get; set; }
        public Vector2 CircleCenter { get; set; }

        public Circle (float radius, Vector2 center)
        {
            Radius = radius;
            CircleCenter = center;
        }

        public bool Contains(Vector2 point)
        {
            if(Radius >= (point - CircleCenter).Length())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Intersects(Circle other)
        {
            if ((other.Radius - Radius) > (other.CircleCenter - CircleCenter).Length())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
