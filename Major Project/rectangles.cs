using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Major_Project
{
    public struct Rectangles
    {
        public Rectangles(Rectangle rectangle, float angle)
        {
            Piece = rectangle;
            Angle = angle;
        }

        public Rectangle Piece { get; }
        public float Angle { get; }
    }
}