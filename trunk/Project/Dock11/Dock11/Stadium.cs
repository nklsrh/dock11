using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace Dock11
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Stadium : Microsoft.Xna.Framework.GameComponent
    {
        public Stadium(Game game)
            : base(game)
        {      
        }
        Game1 game = new Game1();
        public Vector2 CameraPosition = new Vector2(1280/2, 720/2);
        public float Scale;
        public Texture2D Sprite;
        public Texture2D CollisionMap;
        public Color bgColor;
        public Color[] bgColorArr;
        public Vector2 StartPosition;

        public void Initialize(GraphicsDeviceManager graphics)
        {
            CollisionMap = new Texture2D(graphics.GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void CheckCollisionWithPlayer(Player Player, GameTime gameTime)
        {
            bgColorArr = new Color[1];
            try
            {
                CollisionMap.GetData<Color>(0, new Rectangle((int)Player.Position.X, (int)Player.Position.Y, 1, 1), bgColorArr, 0, 1);
                bgColor = bgColorArr[0];
            }
            catch { }

            if (bgColor == Color.Black) 
            {
                //GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
                Player.Position = Player.PreviousPosition;
                Player.Speed = Vector2.Zero;
            }
            if (bgColor == Color.Cyan) 
            {
                Player.Friction(20);
            }
            if (bgColor == Color.Red)
            {
                Player.Speed = -(Player.Speed) * 40f;
                Player.Friction(400);
            }
            if (bgColor == Color.White) 
            {
                Player.Friction(20);
                //GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(Sprite, Vector2.Zero, Color.White);
            //sb.Draw(CollisionMap, Vector2.Zero, Color.White);
            sb.End();
        }
    }
}