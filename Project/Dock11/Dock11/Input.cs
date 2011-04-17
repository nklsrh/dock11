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
    public class Input : Microsoft.Xna.Framework.GameComponent
    {
        public Input(Game game)
            : base(game)
        {
        }
        GamePadState previousGamePadState, currentGamePadState;

        public void Initialize(Game1 game, Player player)
        {
            previousGamePadState = GamePad.GetState(PlayerIndex.One);
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            base.Initialize();
        }

        public void Update(GameTime gameTime, Blast blast, SpriteBatch spriteBatch, Menu menu, Game1 game, ContentManager content, Player player)
        {
            #region Menus
                currentGamePadState = GamePad.GetState(PlayerIndex.One);
                if (currentGamePadState.Buttons.A == ButtonState.Pressed && previousGamePadState.Buttons.A == ButtonState.Released)
                {

                }

                if (currentGamePadState.Buttons.Start == ButtonState.Pressed && previousGamePadState.Buttons.Start == ButtonState.Released)
                {

                }

                if (currentGamePadState.Buttons.B == ButtonState.Pressed && previousGamePadState.Buttons.B == ButtonState.Released)
                {

                }
            #endregion Menus

            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.R))
            {
                game.GameInitialize();
            }

            #region GameControls
                if (menu.CurrentScreen == Dock11.Menu.Card.InGame)
                {
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.W))
                    {
                        player.Speed.Y -= player.SpeedPower;
                    }
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.S))
                    {
                        player.Speed.Y += player.SpeedPower;
                    }
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.A))
                    {
                        player.Speed.X -= player.SpeedPower;
                    }
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D))
                    {
                        player.Speed.X += player.SpeedPower;
                    }
                    if (currentGamePadState.ThumbSticks.Left.X != 0)
                    {
                        //player[i].Speed.X += player[i].SpeedPower * currentGamePadState.ThumbSticks.Left.X;
                    }
                    if (currentGamePadState.ThumbSticks.Left.Y != 0)
                    {
                        //player[i].Speed.Y -= player[i].SpeedPower * currentGamePadState.ThumbSticks.Left.Y;
                    }
                    if (currentGamePadState.IsButtonUp(Buttons.Start) && previousGamePadState.IsButtonDown(Buttons.Start))
                    {
                        
                    }
                    if (currentGamePadState.Triggers.Right < 0.3)
                    {

                    }
                }
                else
                {
                    if (game.Menu.CurrentScreen == Menu.Card.Paused)
                    {
                        if (currentGamePadState.IsButtonUp(Buttons.Start) && previousGamePadState.IsButtonDown(Buttons.Start))
                        {
                            game.Menu.CurrentScreen = Menu.Card.InGame;
                        }
                    }
                }
            #endregion GameControls

         
            previousGamePadState = currentGamePadState;

            base.Update(gameTime);
        }
    }
}