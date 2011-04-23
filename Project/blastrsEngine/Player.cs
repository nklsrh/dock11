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


namespace blastrs
{
    public class Player : Microsoft.Xna.Framework.GameComponent
    {
        public Player(Game game)
            : base(game)
        {
        }

        public Vector2 Position;
        public Vector2 StartPosition;
        public Vector2 Speed;
        public Texture2D Sprite;
        public float SpeedPower;
        public int Score;
        public Color TintColour;
        public bool Blasting;
        public Texture2D ShadowSprite;
        public float tagOpacity = 6;
        public bool[] onPlatform;
        public bool onSomePlatform;
        public bool isFinished;

        public override void Initialize()
        {
            Blasting = false;
            Score = 50;
            SpeedPower = 0.3f;
            Speed = new Vector2(0.01f, 0.01f);
            onPlatform = new bool[10];
            isFinished = false;
            onSomePlatform = false;
            Position = StartPosition;

            base.Initialize();
        }

        public void Update(Game1 game, GameTime gameTime)
        {
            ClampSpeed();
            Position += Speed;

            if (Score >= 1000)
            {
                game.Menu.CurrentScreen = Menu.Card.Scoreboard;
                game.Menu.Initialize(game, game.spriteBatch, game.Content);
            }
            Friction(6);

            base.Update(gameTime);
        }

        void Friction(float Divisor)
        {
            Speed.X += (0 - Speed.X) / Divisor;
            Speed.Y += (0 - Speed.Y) / Divisor;
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

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(ShadowSprite, new Vector2(Position.X - 20, Position.Y), null, Color.White, 0f, new Vector2(Sprite.Width / 2, Sprite.Height / 2), 1f, SpriteEffects.None, 1f);
            sb.Draw(Sprite, Position, null, TintColour, 0f, new Vector2(Sprite.Width / 2, Sprite.Height / 2), 1f, SpriteEffects.None, 1f);
            sb.End();
        }
    }
}