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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public Menu Menu;

        public Player Player1;
        public Stadium Stadium;
        public Blast Blast;

        public Input Input;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public void GameInitialize()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            Player1 = new Player(this);
            Stadium = new Stadium(this);
            Input = new Input(this);
            Menu = new Menu(this);
            Blast = new Blast(this);

            Player1.Initialize();
            Stadium.Initialize(graphics);
            Input.Initialize(this, Player1);

            Menu.CurrentScreen = Menu.Card.InGame;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();

            Player1.Sprite = Content.Load<Texture2D>("Characters//Bob");
            Stadium.Sprite = Content.Load<Texture2D>("Enviro//Level1");
            Stadium.CollisionMap = Content.Load<Texture2D>("Enviro//Level1Collision");
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            Input.Update(gameTime, Blast, spriteBatch, Menu, this, Content, Player1);
            Player1.Update(gameTime);
            //Stadium.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);

            Stadium.Draw(gameTime, spriteBatch);
            Player1.Draw(gameTime, spriteBatch);

            base.Draw(gameTime);
        }
    }
}
