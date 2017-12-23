using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SFS_BattleTank.Bases;
using SFS_BattleTank.Constants;
using SFS_BattleTank.GameObjCtrl;
using SFS_BattleTank.GameScenes;
using SFS_BattleTank.InputControl;
using SFS_BattleTank.Managers;
using SFS_BattleTank.Maps;
using SFS_BattleTank.Network;
using Sfs2X.Entities.Data;
using System.Collections.Generic;
using System.Diagnostics;

namespace SFS_BattleTank
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Connection network;
        public static ContentManager contents;

        public static SceneManager sceneManager;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

        }
        protected override void Initialize()
        {
            contents = Content;
            network = new Connection();
            network.Init();

            sceneManager = new SceneManager(contents, this);
            sceneManager.Init();
            sceneManager.GotoScene(Consts.SCENE_LOGIN);
            if (graphics.IsFullScreen)
            {
                graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
                graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;
                Consts.VIEWPORT_WIDTH = graphics.GraphicsDevice.DisplayMode.Width;
                Consts.VIEWPORT_HEIGHT = graphics.GraphicsDevice.DisplayMode.Height;
                Consts.VIEWPORT_SCALE_RATE_WIDTH = (float)graphics.GraphicsDevice.DisplayMode.Width / (float)Consts.VIEWPORT_WIDTH;
                Consts.VIEWPORT_SCALE_RATE_HEIGHT = (float)graphics.GraphicsDevice.DisplayMode.Height / (float)Consts.VIEWPORT_HEIGHT;
                graphics.GraphicsDevice.Viewport = new Viewport(0, 0, Consts.VIEWPORT_WIDTH, Consts.VIEWPORT_HEIGHT);

                graphics.ApplyChanges();
            }
            else
            {
                graphics.PreferredBackBufferWidth = Consts.VIEWPORT_WIDTH;
                graphics.PreferredBackBufferHeight = Consts.VIEWPORT_HEIGHT;
                graphics.GraphicsDevice.Viewport = new Viewport(0, 0, Consts.VIEWPORT_WIDTH, Consts.VIEWPORT_HEIGHT);
                graphics.ApplyChanges();
            }
            base.Initialize();

        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Input.Update();
            network.Update(elapsedTime);
            sceneManager.Update(elapsedTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            sceneManager.Draw(spriteBatch);
            base.Draw(gameTime);
        }

    }
}
