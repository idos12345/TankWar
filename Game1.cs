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

namespace TankWar
{

    public delegate void DlgDraw();

    public delegate void DlgUpdate();

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        SpriteBatch spriteBatch;

        Matrix projection;
        SpriteFont font;
        UserKeys playerKeys;
        BotKeys computerKeys;
        SkyBox sky;

        int wins = 0;
        int loses = 0;
        int roundCount = 3;
        List<Round> RoundsList;

        Song song;

        public static event DlgDraw event_draw;
        public static event DlgUpdate event_update;

        public Game1()
        {

            Content.RootDirectory = "Content";
            S.graphics = new GraphicsDeviceManager(this);
            S.content = Content;
            S.graphics.PreferredBackBufferHeight = 1000;
            S.graphics.PreferredBackBufferWidth = 1700;
            S.graphics.IsFullScreen = true;

        }


        protected override void Initialize()
        {
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), S.graphics.GraphicsDevice.Viewport.AspectRatio, .1f, 30000f);
            S.CityMap = new Map();
            S.camera = new Camera(new Vector3(S.cityWidth / 2, 5000, S.cityHeight / 2), new Vector3(S.cityWidth / 2, 0, S.cityHeight / 2), projection);
            sky = new SkyBox();
            base.Initialize();

        }


        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            song = Content.Load<Song>("SoundEffects/300_Violin_Orchestra_-_Jorge_Quintero_High_Quality[www.MP3Fiber.com]");
            MediaPlayer.Play(song);
            font = Content.Load<SpriteFont>("Courier New");
            playerKeys = new UserKeys(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Left, Keys.Right, Keys.Space);
            computerKeys = new BotKeys();

            RoundsList = new List<Round>();
            RoundsList.Add(new Round(playerKeys, computerKeys));

        }


        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (roundCount > 0 && wins < 2 && loses < 2)
            {
                S.GameState gameState = RoundsList[RoundsList.Count - 1].CheckGameState();

                if (gameState != S.GameState.ON)
                {
                    if (gameState == S.GameState.COMPUTER_TANK_WON)
                    {
                        loses++;
                    }
                    else
                    {
                        wins++;
                    }

                    roundCount--;
                    event_update = null;
                    event_draw = null;
                    S.camera = new Camera(new Vector3(S.cityWidth / 2, 5000, S.cityHeight / 2), new Vector3(S.cityWidth / 2, 0, S.cityHeight / 2), projection);


                    if (roundCount > 0 && wins < 2 && loses < 2)
                    {
                        RoundsList.Add(new Round(playerKeys, computerKeys));
                    }
                    else
                    {
                        MediaPlayer.Stop();
                    }
                }
            }

            if (event_update != null) event_update();

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;



            sky.DrawSkyBox(gameTime, S.camera.view, S.camera.projection);
            if (event_draw != null) event_draw();

            spriteBatch.Begin();

            if (roundCount > 0)
            {
                spriteBatch.DrawString(font, "My Health: " + RoundsList[RoundsList.Count - 1].UserTank.health, new Vector2(50), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                spriteBatch.DrawString(font, "Computer Health: " + RoundsList[RoundsList.Count - 1].ComputerTank.health, new Vector2(S.graphics.PreferredBackBufferWidth - 400, 50), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            }
            spriteBatch.DrawString(font, wins + " / " + loses, new Vector2(S.graphics.PreferredBackBufferWidth / 2 - 100, 50), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 1);

            if (roundCount <= 0)
            {
                string EndMessage;
                if (wins > loses)
                {
                    EndMessage = "You Won!";                  
                }
                else
                { 
                    EndMessage = "You Lost...";
                }

                spriteBatch.DrawString(font, EndMessage, new Vector2(S.graphics.PreferredBackBufferWidth / 2 - 200, 500), Color.White, 0, Vector2.Zero, 6, SpriteEffects.None, 1);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
