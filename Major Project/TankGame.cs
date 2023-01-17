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
        public List<Rectangle> FloorRectangles = new List<Rectangle>();
        public List<Vector2> TerrainPoints= new List<Vector2>();
        public List<Projectile> projectiles = new List<Projectile>();
        public List<Player> Players = new List<Player>();

        public TankGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.HardwareModeSwitch = false;
            //_graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public float Interpolate(float a, float b, float v) {
            return (b - a) * ((v * (v * 6f - 15f) + 10f) * v * v * v) + a;
        }

        public float Lerp(float a, float b, float v) => a * (1f - v) + b * v;

        public Vector2 InterpolateY(Vector2 a, Vector2 b, float v)
        {
            return new Vector2((int)Math.Round(Lerp(a.X,b.X,v),0), Interpolate(a.Y,b.Y,v));
        }
        
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            line = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            line.SetData(new[] {Color.White});
            CreateFloorPoints(Window.ClientBounds.Height-300,20,Window.ClientBounds.Width);
            CreateFloorRectangles();
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
            
            // TODO: Add your update logic here
            
            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                projectiles.Add(new Projectile(new Vector2(mouseState.X,mouseState.Y)));
            }
            List<Projectile> temp = new List<Projectile>(projectiles);
            foreach (var item in temp)
            {
                item.Update(FloorRectangles);
            }
            foreach (var item in Players)
            {
                item.Update(true,FloorRectangles);
            }
            

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
            for (int i = 0; i < FloorRectangles.Count; i++)
            {
                _spriteBatch.Draw(line,FloorRectangles[i],null,Color.Black);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public void CreateFloorPoints(int floorheight,int verticalextremity,int width)
        {
            float increment = width / verticalextremity;
            Random r = new Random();
            for (int i = 0; i <= width; i += (int)increment)
            {
                TerrainPoints.Add(
                    new Vector2(i, floorheight + r.Next(-20, 20))
                );
            }

            for (int i = 0; i <= width/increment-1; i++)
            {
                for (float j = 0f; j < 1f; j+=1f/increment)
                {
                    TerrainPoints.Add(
                        InterpolateY(
                            TerrainPoints[i],
                            TerrainPoints[i+1],
                            j
                        )
                    );
                }
            }
        }

        public void CreateFloorRectangles()
        {
            foreach (var item in TerrainPoints)
            {
                FloorRectangles.Add
                (
                    new Rectangle(
                        new Point(
                            (int)item.X,
                            (int)item.Y),
                        new Point
                        (
                            1,
                            Window.ClientBounds.Height)
                        )
                    );
            }
        }
    }
}