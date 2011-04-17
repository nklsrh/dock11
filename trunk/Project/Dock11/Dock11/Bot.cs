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
    public class Bot : Microsoft.Xna.Framework.GameComponent
    {
        public Bot(Game game)
            : base(game)
        {
        }

        public Vector2 Position;
        public Vector2 Speed;
        public float SpeedPower;
        public int Target;
        public Animation Sprite;
        public float Scale;
        public TimeSpan BlastTimer;
        public Color TintColor;
        public bool Dropped;
        public bool Blasted;
        public Texture2D Shadow;
        public Random randomsssss;
        public int BotIndex;
        public BotBlast botBlast;
        public Vector2 blastSpeed;

        public void Initialize(Game1 game)
        {
            BlastTimer = new TimeSpan(0, 0, 5);
            Scale = 1f;
            TintColor = Color.White;
            randomsssss = new Random(123123);
            Position.X = (float)(randomsssss.NextDouble() * game.graphics.PreferredBackBufferWidth);
            Position.Y = 0;
            Dropped = false;
            SpeedPower = 3;
            base.Initialize();
        }

        public void LoadBotAnimation(string directory, ContentManager content, Game1 game)
        {
            Sprite = new Animation(game);
            Sprite.LoadAnimationData(directory, content);
        }

        public void LoadBlastAnimation(string directory, ContentManager content, Game1 game)
        {
            botBlast = new BotBlast(game);
            botBlast.Initialize();
            botBlast.LoadAnimation(directory, content);
        }


        public void Update(GameTime gameTime, Game1 game, Player[] targets)//, Blast blast)
        {
            if (Sprite.IsPlaying == false)
            {
                Sprite.Play();
            }

            Target = 0;

            for (int r = 0; r < targets.Length; r++)
            {
                if (Vector2.Distance(Position, targets[Target].Position) > Vector2.Distance(Position, targets[r].Position))
                {
                    Target = r;
                }
            }

            //Speed += (Vector2.SmoothStep(Position, targets[Target].Position, SpeedPower) * 0.02f);

            //if (targets[Target].Position.X == Position.X) { Speed.X = 0; }
            //if (targets[Target].Position.Y == Position.Y) { Speed.Y = 0; }

            Speed += (targets[Target].Position - Position);
            Speed.Normalize();
            Speed = Speed * SpeedPower;

            Speed += blastSpeed * 1.1f;
            blastSpeed = Vector2.Zero;

            Position += Speed;

            BlastTimer -= gameTime.ElapsedGameTime;

            try { TintColor.R = (byte)(255 - BlastTimer.Milliseconds /10); } catch { }

            if (Sprite.CurrentFrame == 300)
            {
                if (!Blasted)
                {
                    botBlast.Detonate(targets, Position);
                    Blasted = true;
                }
                if (BlastTimer <= TimeSpan.Zero)
                {
                    Dropped = false;
                    Blasted = false;
                    Initialize(game);
                }
            }

            Sprite.Update(Position);

            Speed = Vector2.Zero;

            base.Update(gameTime);
        }

        public void Drop(GameTime gameTime, Vector2 pos)
        {
            Position.X += (pos.X - Position.X) / 20f;
            Position.Y += (pos.Y - Position.Y) / 20f;
            Sprite.CurrentFrame = 0;
            Sprite.IsPlaying = false;
            if (Position.Y >= pos.Y - 4)
            {
                Dropped = true;
                Position = pos; 
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch sb, Player[] players)
        {
            sb.Begin();
            sb.Draw(Shadow, new Vector2(Position.X - 20, Position.Y - 10), null, Color.Black, 0f, new Vector2(22, 23), Scale/1.12f, SpriteEffects.None, 1f);
            if (Sprite.IsPlaying == false)
            {
                sb.Draw(Sprite.Images[0], Position, null, Color.White, 0f, new Vector2(Sprite.Images[0].Width / 2, Sprite.Images[0].Height / 2), 1f, SpriteEffects.None, 1f);
            }
                        
            Sprite.Draw(Position, sb);
            
            botBlast.Draw(players, sb);
            sb.End();
        }
    }
}