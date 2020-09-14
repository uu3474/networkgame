using Android.Content.Res;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace AndroidFrontend
{
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

        protected override void Initialize()
        {
            m_networkGame = new NetworkGame.Game(GraphicsDevice, Content);
            base.Initialize();
        }

        public void SaveDataIfNeed()
        {
            m_networkGame?.SaveDataIfNeed();
        }
        
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
            m_networkGame.Init(_params);
        }

        protected override void UnloadContent()
        {
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
