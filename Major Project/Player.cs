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
    public class Player
    {
        public Rectangle hitbox { get; private set; }
        public bool Turn { get; private set; }
        public int Fuelmax { get; private set; }
        public Vector2 Position { get; private set; }
        public float X { get; private set; }
        public float Y { get; private set; }

        public Color Colour { get; private set; }
        public int Size { get; private set; }

        public Player(bool turn, int fuelmax, Vector2 position)
        {
            
            Turn = turn;
            Fuelmax = fuelmax;
            Position = position;
            hitbox = new Rectangle(new Point((int)X, (int)Y), new Point(15, 15));
            Colour = Color.Black;
            Size = 15;
        }

        public void Update(bool turn, List<Rectangle> floor)
        {
            Point cache = new Point(0,0);
            int collisions = 0;
            Vector2 gravitysize = new Vector2(0, 2);
            bool gravity = true;
            if (gravity)
            {
                Position += gravitysize;
                hitbox = new Rectangle(new Point((int)Position.X, (int)Position.Y), new Point(Size, Size));
            }
            foreach (var item in floor)
            {
                //gravity = true;
                while (hitbox.Intersects(item))// && !(Math.Abs(item.Piece.Location.Y-cache.Y)<2))
                {
                    Colour = Color.Red;
                    gravity = false;
                    collisions++;
                    Position -= gravitysize;
                    hitbox = new Rectangle(new Point((int)Position.X, (int)Position.Y), new Point(Size, Size));
                }

                {
                    Colour = Color.Black;
                }
                cache = item.Location;
            }


           

            KeyboardState state = Keyboard.GetState();

            if (Turn == true)
            {
                int fuel = Fuelmax;
                if (fuel > 0)
                {
                    if (state.IsKeyDown(Keys.A) && state.IsKeyDown(Keys.B))
                    {
                    }
                    if (state.IsKeyDown(Keys.A))
                    {
                        Position = Position + new Vector2(-2, 0);    
                        fuel--;
                        hitbox = new Rectangle(new Point((int)Position.X, (int)Position.Y), new Point(Size, Size));
                    }
                    if (state.IsKeyDown(Keys.D))
                    {
                        Position = Position + new Vector2(2, 0);
                        fuel--;
                        hitbox = new Rectangle(new Point((int)Position.X, (int)Position.Y), new Point(Size, Size));
                    }
                }
            }
        }
    }
}