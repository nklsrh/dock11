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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Panel : Microsoft.Xna.Framework.GameComponent
    {
        public Panel(Game game)
            : base(game)
        {
        }

        public bool isSteppedOn;
        public Vector2 Position;
        public Texture2D Sprite;
        public Rectangle Rectangle;
        public bool isVisible;
        public TimeSpan Time;

        public void Initialize(Game1 game)
        {
            Sprite = game.Content.Load<Texture2D>("LevelObjects\\Panel");
            base.Initialize();
        }


        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(Sprite, Rectangle, Color.White);
            sb.End();

            base.Update(gameTime);
        }
    }
}