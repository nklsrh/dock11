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
    public class Player : Microsoft.Xna.Framework.GameComponent
    {
        public Player(Game game)
            : base(game)
        {
        }

        public Vector2 Position;
        public Vector2 Speed;
        public Texture2D Sprite;
        public float SpeedPower;
        public Color TintColour;
        public Texture2D StarImage;
        public bool Blasting;
        public Texture2D ShadowSprite;

        public void Initialize(Vector2 StartPosition)
        {
            Blasting = false;
            SpeedPower = 0.4f;
            Speed = new Vector2(0.01f, 0.01f);
            Position = StartPosition;

            base.Initialize();
        }

        public void Update(Game1 game, GameTime gameTime)
        {
            ClampSpeed();
            Position += Speed;

            base.Update(gameTime);
        }

        void ClampSpeed()
        {
            if (Speed.X > SpeedPower * 10)
            {
                Speed.X = SpeedPower * 10;
            }
            if (Speed.X < -SpeedPower * 10)
            {
                Speed.X = -SpeedPower * 10;
            }
            if (Speed.Y > SpeedPower * 10)
            {
                Speed.Y = SpeedPower * 10;
            }
            if (Speed.Y < -SpeedPower * 10)
            {
                Speed.Y = -SpeedPower * 10;
            }
        }

        public void Friction(float SurfaceFriction)
        {
            if (Speed.X > 0)
            {
                Speed.X -= SurfaceFriction / 100;
            }
            if (Speed.X < 0)
            {
                Speed.X += SurfaceFriction / 100;
            }
            if (Speed.Y > 0)
            {
                Speed.Y -= SurfaceFriction / 100;
            }
            if (Speed.Y < 0)
            {
                Speed.Y += SurfaceFriction / 100;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            //sb.Draw(ShadowSprite, new Vector2(Position.X - 20, Position.Y), null, TintColour, 0f, new Vector2(Sprite.Width / 2, Sprite.Height / 2), 1f, SpriteEffects.None, 1f);
            sb.Draw(Sprite, Position, null, Color.White, 0f, new Vector2(Sprite.Width / 2, Sprite.Height / 2), 1f, SpriteEffects.None, 1f);
            sb.End();
        }
    }
}