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
using SFS_BattleTank.Network;
using Sfs2X.Entities.Data;
using System.Collections.Generic;

namespace SFS_BattleTank
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
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
            graphics.PreferredBackBufferWidth = Consts.VIEWPORT_WIDTH;
            graphics.PreferredBackBufferHeight = Consts.VIEWPORT_HEIGHT;            
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            graphics.IsFullScreen = false;
           
            network = new Connection();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            contents = Content;
            network.Init();
            network.Connect();

            sceneManager = new SceneManager(contents,this);
            sceneManager.Init();
            sceneManager.GotoScene(Consts.SCENE_LOGIN);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            if (graphics.IsFullScreen)
            {
                Consts.VIEWPORT_WIDTH = graphics.GraphicsDevice.Viewport.Width;
                Consts.VIEWPORT_HEIGHT = graphics.GraphicsDevice.Viewport.Height;
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Input.Update();
            network.Update(elapsedTime);
            // TODO: Add your update logic here
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
            
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            sceneManager.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
      
    }
}
