using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetworkGame.Engine;
using NetworkGame.Engine.UI;
using NetworkGame.UI;

namespace NetworkGame.Menus
{
    public class SelectLevelMenu : DefaultCanvasScreen, IUpdatable
    {
        TextSprite m_caption;
        
        public ListView<LevelListViewItem> LevelsListView { get; protected set; }
        public TextButton BackButton { get; protected set; }

        public SelectLevelMenu(GraphicsDevice device)
            : base(device, (Game.Content.GetSizeInDpi(500) > device.Viewport.Width ? device.Viewport.Width : Game.Content.GetSizeInDpi(500)), device.Viewport.Height)
        {
            int spacing = Game.Content.GetSizeInDpi(20);

            float xCenter = Canvas.View.Width / 2;
            float listHeight = device.Viewport.Height;

            this.m_caption = new TextSprite(Game.Content.Fonts.ButtonFont) { Text = "Level select", X = xCenter };
            this.m_caption.Y = spacing + this.m_caption.Height / 2;
            this.Canvas.Add(this.m_caption);
            listHeight -= spacing * 2 + this.m_caption.Height;

            this.BackButton = new TextButton(ButtonType.MainMenuButton) { Text = "Back", X = xCenter };
            this.BackButton.Y = Canvas.View.Height - spacing - this.BackButton.Height / 2;
            this.BackButton.AddToCanvas(this.Canvas);
            listHeight -= spacing * 2 + this.BackButton.Height;

            float listY = spacing * 2 + this.m_caption.Height + listHeight / 2;
            this.LevelsListView = new ListView<LevelListViewItem>(device, Canvas.View.Width, (int)listHeight, spacing)
                { IsProcessInput = true, X = xCenter, Y = listY };
            this.LevelsListView.AddToCanvas(this.Canvas);
        }

        protected override void DrawCore(GameTime gameTime)
        {
            LevelsListView.Draw(gameTime);
        }

        protected override void BackCore()
        {
            BackButton.Click?.Invoke();
        }

        protected override void MouseCore(MouseHandlerParams _params)
        {
            LevelsListView.Mouse(_params);
            BackButton.Mouse(_params);
        }

        protected override void TouchCore(ref TouchHandlerParams _params)
        {
            LevelsListView.Touch(ref _params);
            BackButton.Touch(ref _params);
        }

        public void Update(GameTime gameTime)
        {
            if (!Visible)
                return;

            LevelsListView.Update(gameTime);
        }
    }
}
