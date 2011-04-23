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
    public class Box : Microsoft.Xna.Framework.GameComponent
    {
        public Box(Game game)
            : base(game)
        {
        }

        public Vector2 Position;
        public Color Colour;
        public bool isActivated;
        public Texture2D Sprite;

        public void Initialize(Game1 game)
        {
            if (Colour == Color.Orange)
            {
                Sprite = game.Content.Load<Texture2D>("LevelObjects\\orangeBox");
            }
            if (Colour == Color.Blue)
            {
                Sprite = game.Content.Load<Texture2D>("LevelObjects\\blueBox");
            }

            base.Initialize();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(Sprite, Position, Color.White);
            sb.End();
        }
    }
}