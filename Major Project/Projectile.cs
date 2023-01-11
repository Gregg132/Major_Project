using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;


namespace Major_Project
{
    public class Projectile
    {
        public Projectile(Vector2 position)
        {
            Position = position;
            Rectangle Hitbox = new Rectangle(new Point((int)X, (int)Y), new Point(5, 5));
            X = Position.X;
            Y = Position.Y;
            Colour = Color.Black;
        }
        
        public Vector2 Position { get; private set; }
        public float X { get; private set; }
        public float Y { get; private set; }
        public Color Colour { get; private set; }
        public Rectangle Hitbox { get; private set; }

        public bool Update(List<Rectangles> floor)
        {
            Position += new Vector2(0, 1);
            foreach (var item in floor)
            {
                if (Hitbox.Intersects(item.Piece))
                {
                    Position -= new Vector2(0, 5);
                    Colour = Color.Red;
                    X = Position.X;
                    Y = Position.Y;
                    Hitbox = new Rectangle(new Point((int)X, (int)Y), new Point(5, 5));
                    return true;
                }
            }
            X = Position.X;
            Y = Position.Y;
            Hitbox = new Rectangle(new Point((int)X, (int)Y), new Point(5, 5));
            return false;
        }
    }
}