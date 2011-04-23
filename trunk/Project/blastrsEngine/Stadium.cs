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
    public class Stadium : Microsoft.Xna.Framework.GameComponent
    {
        public Stadium(Game game)
            : base(game)
        {      
        }
        Game1 game = new Game1();
        public Vector2 CameraPosition;
        public float Scale;
        public Texture2D Sprite;
        public Texture2D CollisionMap;
        public Color bgColor;
        public Color[] bgColorArr;
        public Vector2[] StartPosition;
        public int LevelNumber;
        public int NumberOfPanels;
        public int NumberOfBoxes;
        public int NumberOfPlayersFinished;

        public void Initialize(GraphicsDeviceManager graphics)
        {
            //CollisionMap = new Texture2D(graphics.GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            StartPosition = new Vector2[2];
            UpdateStartPosition();

            base.Initialize();
        }

        private void UpdateStartPosition()
        {
            switch (LevelNumber)
            {
                case 1:
                    StartPosition[0] = new Vector2(395, 358);
                    StartPosition[1] = new Vector2(861, 232);
                    NumberOfPanels = 6;
                    NumberOfBoxes = 0;
                    break;
                case 2:
                    StartPosition[0] = new Vector2(183, 144);
                    StartPosition[1] = new Vector2(1138, 132);
                    NumberOfPanels = 6;
                    NumberOfBoxes = 2;
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
        }

        public void InitiatePanel(int PanelIndex, float PositionX, float PositionY, bool Visibility, Game1 game)
        {
            game.Panels[PanelIndex] = new Panel(game);
            game.Panels[PanelIndex].Initialize(game);
            game.Panels[PanelIndex].Position = new Vector2(PositionX, PositionY);
            game.Panels[PanelIndex].Rectangle = new Rectangle((int)game.Panels[PanelIndex].Position.X, (int)game.Panels[PanelIndex].Position.Y, game.Panels[PanelIndex].Sprite.Width, game.Panels[PanelIndex].Sprite.Height);
            game.Panels[PanelIndex].isVisible = Visibility;
        }
        public void InitiateBox(int BoxIndex, float PositionX, float PositionY, bool isActivated, Color color, Game1 game)
        {
            game.Boxes[BoxIndex] = new Box(game);
            game.Boxes[BoxIndex].Position = new Vector2(PositionX, PositionY);
            game.Boxes[BoxIndex].isActivated = isActivated;
            game.Boxes[BoxIndex].Colour = color;
            game.Boxes[BoxIndex].Initialize(game);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Level1(int NumberOfPlayers, Panel[] Panels, Player[] Player)
        {
            for (int s = 0; s < NumberOfPlayers; s++)
            {
                for (int r = 0; r < NumberOfPanels; r++)
                {
                    if (Panels[r].Rectangle.Contains((int)Player[s].Position.X, (int)Player[s].Position.Y) && Panels[r].isVisible)
                    {
                        Panels[r].isSteppedOn = true;
                        Player[s].onPlatform[r] = true;
                        try
                        {
                            Panels[r + 1].isVisible = true;
                        }
                        catch { }
                    }
                    else
                    {
                        Panels[r].isSteppedOn = false;
                        Player[s].onPlatform[r] = false;
                    }
                }
            }
        }
        public void Level2(int NumberOfPlayers, Panel[] Panels, Player[] Player, Box[] Boxes)
        {
            for (int s = 0; s < NumberOfPlayers; s++)
            {
                for (int r = 0; r < NumberOfPanels; r++)
                {
                    if (Panels[r].Rectangle.Contains((int)Player[s].Position.X, (int)Player[s].Position.Y) && Panels[r].isVisible)
                    {
                        Panels[r].isSteppedOn = true;
                        Player[s].onPlatform[r] = true;
                    }
                    else
                    {
                        Panels[r].isSteppedOn = false;
                        Player[s].onPlatform[r] = false;
                    }
                }
            }
            CheckBoxActivation(Boxes);
            if (Boxes[0].isActivated && Boxes[1].isActivated)
            {
                for (int x = 0; x < NumberOfPanels; x++)
                {
                    Panels[x].isVisible = true;
                }
            }
        }

        public void CheckBoxActivation(Box[] Boxes)
        {
            for (int g = 0; g < NumberOfBoxes; g++)
            {
                bgColorArr = new Color[1];
                CollisionMap.GetData<Color>(0, new Rectangle((int)Boxes[g].Position.X, (int)Boxes[g].Position.Y, 1, 1), bgColorArr, 0, 1);
                bgColor = bgColorArr[0];

                if (bgColor == Boxes[g].Colour)
                {
                    Boxes[g].isActivated = true;
                }
            }
        }

        public void CheckCollisionWithPlayer(Player Player, GameTime gameTime, int index, Game1 game1)
        {
            bgColorArr = new Color[1];
            CollisionMap.GetData<Color>(0, new Rectangle((int)Player.Position.X, (int)Player.Position.Y, 1, 1), bgColorArr, 0, 1);
            bgColor = bgColorArr[0];

            for (int r = 0; r < NumberOfPanels; r++)
            {
                if (Player.onPlatform[r])
                {
                    Player.onSomePlatform = true;
                }
           }
            if (bgColor == new Color(88, 88, 88) && !Player.onSomePlatform) //FALLS OFFFFFFFFFFF
            {
                Player.Position = Player.StartPosition;
                Player.Speed = Vector2.Zero;
            }

            if (bgColor == new Color(118, 236, 0)) //FALLS OFFFFFFFFFFF
            {
                Player.isFinished = true;
                if (game1.Player[1 - index].isFinished && Player.isFinished)
                {
                    LevelNumber += 1;
                    game1.NewGame();
                }
            }
            else
            {
                Player.isFinished = false;
            }

            Player.onSomePlatform = false;
            Player.TintColour = bgColor;
        }


        public void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(Sprite, Vector2.Zero, Color.White);
            sb.Draw(CollisionMap, Vector2.Zero, Color.White);
            sb.End();
        }
    }
}