using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UniversalDesktopFrontend
{
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

        protected override void Initialize()
        {
            m_networkGame = new NetworkGame.Game(GraphicsDevice, Content);
            base.Initialize();
        }

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

        protected override void UnloadContent()
        {
            m_networkGame.SaveDataIfNeed();
        }

        protected override void Update(GameTime gameTime)
        {
            m_networkGame.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            m_networkGame.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
