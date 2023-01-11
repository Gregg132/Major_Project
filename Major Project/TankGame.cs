using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;
using System.Collections.Generic;
using System.Collections;
using SharpDX.Direct2D1;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace Major_Project
{
    public class TankGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D line;
        public Vector2[] map;
        private int smoothness = 5;
        private float lastangle;
        private int lowest;
        public List<Rectangles> floorpoints = new List<Rectangles>();
        public List<Rectangles> floor = new List<Rectangles>();
        public List<Projectile> projectiles = new List<Projectile>();
        public List<Player> Players = new List<Player>();

        public TankGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            map = new Vector2[Window.ClientBounds.Width];
            line = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            line.SetData(new[] {Color.White});
            Genmap(300, 1, map, Color.Black);
            GenTerrain(Color.Black, 50);
            floor.Add(new Rectangles(new Rectangle(new Point(0, lowest), new Point(GraphicsDevice.Viewport.Width, 500)), 0f));
            Players.Add(new Player(true, 500, new Vector2(250, 250)));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                projectiles.Add(new Projectile(new Vector2(mouseState.X,mouseState.Y)));
            }
            List<Projectile> temp = new List<Projectile>(projectiles);
            foreach (var item in temp)
            {
                item.Update(floor);
            }
            foreach (var item in Players)
            {
                item.Update(true,floor);
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            
            Color colour = Color.Black;

            _spriteBatch.Begin();

            foreach (var item in Players)
            {
                _spriteBatch.Draw(line, new Rectangle(new Point((int)item.Position.X,(int)item.Position.Y), new Point(item.Size, item.Size)), null, item.Colour, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            }
            foreach (var item in projectiles)
            {
                _spriteBatch.Draw(line, new Rectangle(new Point((int)item.X,(int)item.Y), new Point(5,5)),null,item.Colour,0,new Vector2(0,0), SpriteEffects.None, 0);
            }
            foreach (var item in floor)
            {
                //_spriteBatch.Draw(line, item.Piece, null, colour, item.Angle, new Vector2(0, 0), SpriteEffects.None, 0);  //no hitboxes
                _spriteBatch.Draw(line, item.Piece, null, colour, 0, new Vector2(0, 0), SpriteEffects.None, 0);           //hitboxes
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
        
        public void CalcAngle(Rectangle rect1,Rectangle rect2, Color colour, int thickness)
        {
            float horiz = rect2.Location.X - rect1.Location.X;
            float vert = rect2.Location.Y - rect1.Location.Y;
            double dist = 1+Math.Sqrt(Math.Pow(horiz, 2) + Math.Pow(Math.Abs(vert), 2)) + 0.05;
            float angle = 0;

            if (vert > 0)
            {
                angle = (float)(Math.Atan(Math.Abs(vert) / horiz));
            }
            else if (vert < 0)
            {
                angle = (float)(2 * Math.PI) - (float)(Math.Atan(Math.Abs(vert) / horiz));
            }
            else
            {
                angle = 0;
            }
            if (angle > lastangle && lastangle != 0)
            {
                lastangle = angle;
                //DrawLine(new Vector2(rect1.Location.X,rect1.Location.Y) - new Vector2(thickness, 0), new Vector2(rect1.Location.X,rect1.Location.Y) + new Vector2(thickness, 0), colour, 500);
                floor.Add(new Rectangles(new Rectangle(new Point(rect1.Location.X - thickness, rect1.Location.Y), new Point((int)Math.Round(dist),thickness)),angle));
            }
            else if (lastangle > angle && lastangle != 0)
            {
                lastangle = angle;
                //DrawLine(new Vector2(rect1.Location.X,rect1.Location.Y) - new Vector2(2, 0), new Vector2(rect1.Location.X,rect1.Location.Y) + new Vector2(2, 0), colour, 500);
                floor.Add(new Rectangles(new Rectangle(new Point(rect1.Location.X - thickness, rect1.Location.Y), new Point((int)Math.Round(dist), thickness)), angle));
            }
            else if (angle == 0 && lastangle != 0)
            {
                lastangle = angle;
                //DrawLine(new Vector2(rect1.Location.X,rect1.Location.Y) - new Vector2(thickness, 0), new Vector2(rect1.Location.X,rect1.Location.Y) + new Vector2(thickness, 0), colour, 500);
                floor.Add(new Rectangles(new Rectangle(new Point(rect1.Location.X - thickness, rect1.Location.Y), new Point((int)Math.Round(dist), thickness)), angle));
            }
            else if (lastangle == 0 && angle > 0)
            {
                lastangle = angle;
                //DrawLine(new Vector2(rect1.Location.X,rect1.Location.Y) - new Vector2(thickness, 0), new Vector2(rect1.Location.X,rect1.Location.Y) + new Vector2(thickness, 0), colour, 500);
                floor.Add(new Rectangles(new Rectangle(new Point(rect1.Location.X - thickness, rect1.Location.Y), new Point((int)Math.Round(dist), thickness)), angle));
            }
            lastangle = angle;
            floor.Add(new Rectangles(new Rectangle(new Point(rect1.Location.X - thickness, rect1.Location.Y), new Point((int)Math.Round(dist), thickness)), angle));
        }
        public void Genmap(int H, int E, Vector2[] map, Color colour)
        {
            int thickness = 5;
            int x = (E * 5);
            Random r = new Random();
            int h = 0;
            for (int i = 0; i < map.Length; i += smoothness)
            {
                switch (r.Next(2))
                {
                    case 0:
                        h = H + r.Next(x);
                        break;
                    case 1:
                        h = H - r.Next(x);
                        break;
                    default:
                        break;
                }
                map[i] = new Vector2(i * smoothness, h);
                if (h > lowest)
                {
                    lowest = h;
                }
                
                if (i != 0)
                {
                    floorpoints.Add(new Rectangles(new Rectangle(new Point((int)map[i-smoothness].X, (int)map[i-smoothness].Y), new Point((int)Math.Round(Math.Sqrt(Math.Pow(map[i].X - map[i-smoothness].X, 2) + Math.Pow(map[i].Y - map[i-smoothness].Y, 2)), 0), thickness)),0f));
                }
            }
        }
        public void GenTerrain(Color colour, int thickness)
        {
            for (int i = 0; i < floorpoints.Count-1; i++)
            {
                CalcAngle(floorpoints[i].Piece, floorpoints[i+1].Piece,colour, thickness);
            }
        }
    }
}