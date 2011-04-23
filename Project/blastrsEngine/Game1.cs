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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public Player[] Player;
        public int NumberOfPlayers;

        Texture2D[] ScoreBar = new Texture2D[2];
        Texture2D PausedScreen;

        public Stadium Stadium;
        Input Input;
        Blast[] Blast = new Blast[10];
        public Panel[] Panels = new Panel[20];
        public Box[] Boxes = new Box[3];

        public SpriteFont Font, BoldFont;
        public Menu Menu;
        public TimeSpan CountDownTime;

        public int[] myint;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            //if (!graphics.IsFullScreen)
            //{
            //    //graphics.ToggleFullScreen();
            //}
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            for (int r = 0; r < 10; r++)
            {
                Blast[r] = new Blast(this);
            }

            Stadium = new Stadium(this);
            Input = new Input(this);         

            Stadium.LevelNumber = 3;
            Stadium.Initialize(graphics);

            NumberOfPlayers = 2;
            Player = new Player[NumberOfPlayers];

            for (int r = 0; r < NumberOfPlayers; r++)
            {
                Player[r] = new Player(this);
                Player[r].StartPosition = Stadium.StartPosition[r];
            }

            NewGame();

            base.Initialize();
        }
        public void GameInitialize()
        {
            Initialize();
        }
        public void NewGame()
        {
            for (int r = 0; r < 10; r++)
            {
                Blast[r].Initialize();
            }

            Stadium.Sprite = Content.Load<Texture2D>("Levels\\Level" + Stadium.LevelNumber);
            Stadium.CollisionMap = Stadium.Sprite;
            Stadium.CameraPosition = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2); //STILL CAMERA FOR NOW         
            if (Stadium.LevelNumber > 1)
            {
                Stadium.Initialize(graphics);
            }

            for (int r = 0; r < NumberOfPlayers; r++)
            {
                Player[r].StartPosition = Stadium.StartPosition[r];
                Player[r].Initialize();
            }

            Input.Initialize(this, Player);

            DistributePanels();
            DistributeBoxes();

            CountDownTime = new TimeSpan(0, 1, 0);
        }

        public void DistributePanels()
        {
            switch (Stadium.LevelNumber)
            {
                case 1:
                    Stadium.InitiatePanel(0, 460, 368, true, this);
                    Stadium.InitiatePanel(1, 901, 227, false, this);
                    Stadium.InitiatePanel(2, 540, 388, false, this);
                    Stadium.InitiatePanel(3, 921, 307, false, this);
                    Stadium.InitiatePanel(4, 620, 408, false, this);
                    Stadium.InitiatePanel(5, 901, 387, false, this);
                    break;
                case 2:
                    Stadium.InitiatePanel(0, 310, 100, false, this);
                    Stadium.InitiatePanel(3, 910, 100, false, this);
                    Stadium.InitiatePanel(1, 390, 100, false, this);
                    Stadium.InitiatePanel(4, 830, 100, false, this);
                    Stadium.InitiatePanel(2, 470, 100, false, this);
                    Stadium.InitiatePanel(5, 750, 100, false, this);
                    break;
                case 3:
                    Stadium.InitiatePanel(0, 607, 221, true, this);
                    Stadium.InitiatePanel(1, 687, 221, false, this);
                    Stadium.InitiatePanel(2, 767, 221, false, this);
                    Stadium.InitiatePanel(3, 607, 270, false, this);
                    Stadium.InitiatePanel(4, 687, 320, false, this);
                    Stadium.InitiatePanel(5, 767, 320, false, this);
                    break;
            }
        }
        public void DistributeBoxes()
        {
            switch (Stadium.LevelNumber)
            {
                case 1:
                    break;
                case 2:
                    Stadium.InitiateBox(0, 183, 427, false, Color.Orange, this);
                    Stadium.InitiateBox(1, 1140, 427, false, Color.Blue, this);
                    break;
                case 3:
                    break;
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            try
            {
                Player[0].Sprite = Content.Load<Texture2D>("Characters\\redPlayer");
                Player[0].ShadowSprite = Content.Load<Texture2D>("Characters\\shadow");
            }
            catch { }

            try
            {
                Player[1].Sprite = Content.Load<Texture2D>("Characters\\bluePlayer");
                Player[1].ShadowSprite = Content.Load<Texture2D>("Characters\\shadow");
            }
            catch {}

            ScoreBar[0] = Content.Load<Texture2D>("redBar");
            ScoreBar[1] = Content.Load<Texture2D>("blueBar");
            

            
            for (int r = 0; r < 10; r++)
            {
                Blast[r].Initialize();
                Blast[r].Sprite = Content.Load<Texture2D>("PlayerBlast");
            }

            Font = Content.Load<SpriteFont>("font");
            BoldFont = Content.Load<SpriteFont>("BoldFont");

            PausedScreen = Content.Load<Texture2D>("Paused");
        //--------------------------------------------------------------------------------MENU SELECTLOLOLOL
            Menu = new Menu(this);
            Menu.CurrentScreen = Menu.Card.InGame;
            Menu.Initialize(this, spriteBatch, Content);

        }
        protected override void Update(GameTime gameTime)
        {
            Input.Update(gameTime, Blast, spriteBatch, Menu, this, Content, Player);

            if (Menu.CurrentScreen == Menu.Card.InGame)
            {
                for (int r = 0; r < NumberOfPlayers; r++)
                {
                    Player[r].Update(this, gameTime);
                    Stadium.CheckCollisionWithPlayer(Player[r], gameTime, r, this);

                    for (int b = 0; b < 10; b++)
                    {
                        Blast[b].Update(gameTime, Player[r]);
                        Blast[b].PlayerImpact(gameTime, Player[r], r);
                        for (int g = 0; g < Stadium.NumberOfBoxes; g++)
                        {
                            Blast[b].BoxImpact(gameTime, Boxes[g]);
                        }
                    }

                }

                switch (Stadium.LevelNumber)
                {
                    case 1:
                        Stadium.Level1(NumberOfPlayers, Panels, Player);
                        break;
                    case 2:
                        Stadium.Level2(this, NumberOfPlayers, Panels, Player, Boxes);
                        break;
                    case 3:
                        Stadium.Level3(NumberOfPlayers, Panels, Player);
                        break;
                }

                Timer(gameTime);
            }


            base.Update(gameTime);
        }

        public void Timer(GameTime gameTime)
        {
            CountDownTime -= gameTime.ElapsedGameTime;
            if (CountDownTime <= TimeSpan.Zero)
            {
                //Menu.CurrentScreen = Menu.Card.Scoreboard;
                //Menu.Initialize(this, spriteBatch, Content);
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Menu.Draw(this, gameTime, spriteBatch);

            if (Menu.CurrentScreen == Menu.Card.InGame || Menu.CurrentScreen == Menu.Card.Paused)
            {
                Stadium.Draw(gameTime, spriteBatch);

                for (int r = 0; r < Stadium.NumberOfPanels; r++)
                {
                    if (Panels[r].isVisible)
                    {
                        Panels[r].Draw(gameTime, spriteBatch);
                    }
                }
                for (int r = 0; r < Stadium.NumberOfBoxes; r++)
                {
                    Boxes[r].Draw(gameTime, spriteBatch);
                }
                for (int r = 0; r < NumberOfPlayers; r++)
                {
                    Player[r].Draw(gameTime, spriteBatch);
                }
                for (int r = 0; r < 10; r++)
                {
                    Blast[r].Draw(spriteBatch);
                } 

                //DrawScore();

                if (Menu.CurrentScreen == Menu.Card.Paused)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(PausedScreen, Vector2.Zero, Color.White);
                    spriteBatch.End();
                }
            }


            base.Draw(gameTime);
        }
        public void DrawScore()
        {
                spriteBatch.Begin();
                try
                {
                    spriteBatch.Draw(ScoreBar[0], new Rectangle(42, (int)(622.5f - ((float)(Player[0].Score / 1000f) * 379f)), 85, (int)(((float)(Player[0].Score / 1000f) * 379f))), Color.White);
                    //spriteBatch.DrawString(Font, Player[0].Score.ToString(), new Vector2(160, 460), new Color(232, 156, 54));
                }
                catch (Exception e) { }
                try
                {
                    spriteBatch.Draw(ScoreBar[1], new Rectangle(165, (int)(622.5f - ((float)(Player[1].Score / 1000f) * 379f)), 85, (int)(((float)(Player[1].Score / 1000f) * 379f))), Color.White);
                    //spriteBatch.DrawString(Font, Player[1].Score.ToString(), new Vector2(1130, 460), new Color(179, 194, 219));
                } 
                catch (Exception e) { }
                try
                {
                    spriteBatch.Draw(ScoreBar[2], new Rectangle(1087, (int)(622.5f - ((float)(Player[2].Score / 1000f) * 379f)), 85, (int)(((float)(Player[2].Score / 1000f) * 379f))), Color.White);
                    //spriteBatch.DrawString(Font, Player[2].Score.ToString(), new Vector2(80, 560), new Color(179, 219, 189));
                }
                catch (Exception e) { }
                try
                {
                    spriteBatch.Draw(ScoreBar[3], new Rectangle(1223, (int)(622.5f - ((float)(Player[3].Score / 1000f) * 379f)), 85, (int)(((float)(Player[3].Score / 1000f) * 379f))), Color.White);
                    //spriteBatch.DrawString(Font, Player[3].Score.ToString(), new Vector2(1200, 560), new Color(243, 237, 217));
                }
                catch (Exception e) { }

                spriteBatch.DrawString(Font, "seconds", new Vector2(40, 140), new Color(150, 150, 150));
                spriteBatch.DrawString(BoldFont, CountDownTime.Seconds.ToString(), new Vector2(40, 0), new Color(222, 222, 222));
                spriteBatch.End();
        }
        public void DrawScoreboard()
        {
            for (int r = 0; r < NumberOfPlayers; r++)
            {
                if (Player[r].Score > 1000)
                {
                    Player[r].Score = 1000;
                }
            }
            spriteBatch.Begin();
            spriteBatch.Draw(Menu.Screen, Vector2.Zero, Color.White);
            try
            {
                spriteBatch.Draw(ScoreBar[0], new Rectangle(442, (int)(554f - ((float)(Player[0].Score / 1000f) * 324f)), 85, (int)(((float)(Player[0].Score / 1000f) * 324f))), Color.White);
            }
            catch (Exception e) { }
            try
            {
                spriteBatch.Draw(ScoreBar[1], new Rectangle(570, (int)(554f - ((float)(Player[1].Score / 1000f) * 324f)), 85, (int)(((float)(Player[1].Score / 1000f) * 324f))), Color.White);   
            }
            catch (Exception e) { }
            try
            {
                spriteBatch.Draw(ScoreBar[2], new Rectangle(724, (int)(554f - ((float)(Player[2].Score / 1000f) * 324f)), 85, (int)(((float)(Player[2].Score / 1000f) * 324f))), Color.White);   
            }
            catch (Exception e) { }
            try
            {
                spriteBatch.Draw(ScoreBar[3], new Rectangle(860, (int)(554f - ((float)(Player[3].Score / 1000f) * 324f)), 85, (int)(((float)(Player[3].Score / 1000f) * 324f))), Color.White);
            }
            catch (Exception e) { }
            spriteBatch.End();
        }
    }
}
