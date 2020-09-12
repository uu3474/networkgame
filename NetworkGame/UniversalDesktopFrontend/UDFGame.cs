using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UniversalDesktopFrontend
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class UDFGame : Game
    {
        GraphicsDeviceManager m_graphics;
        NetworkGame.Game m_networkGame;

        public UDFGame()
        {
            this.m_graphics = new GraphicsDeviceManager(this);
            this.m_graphics.SynchronizeWithVerticalRetrace = true;
            this.m_graphics.IsFullScreen = false;
#if DEBUG
            //windowed
            this.m_graphics.PreferredBackBufferWidth = 600;
            this.m_graphics.PreferredBackBufferHeight = 600;
            this.Window.IsBorderless = false;
#else
            //fullscreen borderless
            this.m_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            this.m_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            this.Window.IsBorderless = true;
#endif
            this.Content.RootDirectory = "Content";

            this.IsMouseVisible = true;
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

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            var _params = new NetworkGame.GameParams();
            _params.DPI = 160;
            _params.ProfileDir = "Saves";
            _params.Exit = (() => Exit());
            _params.UseAdvancedEffects = true;
            _params.MouseInput = true;
            _params.KeyboardInput = true;
#if DEBUG
            _params.ShowFps = true;
#endif
            m_networkGame.Init(_params);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            m_networkGame.SaveDataIfNeed();
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
