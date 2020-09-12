using Android.Content.Res;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace AndroidFrontend
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class AGame : Game
    {
        GraphicsDeviceManager m_graphics;
        NetworkGame.Game m_networkGame;

        public AGame()
        {
            this.m_graphics = new GraphicsDeviceManager(this);
            this.m_graphics.SynchronizeWithVerticalRetrace = true;
            this.m_graphics.IsFullScreen = true;
            this.m_graphics.SupportedOrientations = DisplayOrientation.Portrait;

            this.Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            m_networkGame = new NetworkGame.Game(GraphicsDevice, Content);

            base.Initialize();
        }

        public void SaveDataIfNeed()
        {
            m_networkGame?.SaveDataIfNeed();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            var _params = new NetworkGame.GameParams();
            _params.DPI = (int)Resources.System.DisplayMetrics.DensityDpi;
            _params.ProfileDir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            _params.TouchInput = true;
            _params.GamePadInput = true;
#if DEBUG
            _params.ShowFps = true;
#endif
            _params.ShowFps = true;
            m_networkGame.Init(_params);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            m_networkGame.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            m_networkGame.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
