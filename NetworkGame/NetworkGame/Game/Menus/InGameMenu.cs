using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetworkGame.Content;
using NetworkGame.Engine;
using NetworkGame.Engine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGame.Menus
{
    public class InGameMenu : DefaultCanvasScreen
    {
        public TextButton RestartButton { get; protected set; }
        public TextButton LevelSelectButton { get; protected set; }
        public TextButton MainMenuButton { get; protected set; }
        public TextButton ExitButton { get; protected set; }
        public TextButton BackButton { get; protected set; }

        public InGameMenu(GraphicsDevice device)
            : base(device, Game.Content.GetSizeInDpi(240), Game.Content.GetSizeInDpi(Game.Content.Common.ShowExitButton ? 360 : 300))
        {
            this.Canvas.View.SetColor(Game.Content.Colors.ButtonBack);
            this.Canvas.View.Alpha = Game.Content.Colors.ButtonBackAlpha;

            int buttonSpacing = Game.Content.GetSizeInDpi(20);

            var coords = new Vector2(this.Canvas.View.Width / 2, buttonSpacing);

            this.RestartButton = new TextButton(ButtonType.MainMenuButton) { Text = "Restart", X = coords.X };
            coords.Y += RestartButton.Height / 2;
            this.RestartButton.Y = coords.Y;
            this.RestartButton.AddToCanvas(this.Canvas);
            coords.Y += RestartButton.Height / 2 + buttonSpacing;

            this.LevelSelectButton = new TextButton(ButtonType.MainMenuButton) { Text = "Level select", X = coords.X };
            coords.Y += LevelSelectButton.Height / 2;
            this.LevelSelectButton.Y = coords.Y;
            this.LevelSelectButton.AddToCanvas(this.Canvas);
            coords.Y += LevelSelectButton.Height / 2 + buttonSpacing;

            this.MainMenuButton = new TextButton(ButtonType.MainMenuButton) { Text = "Main menu", X = coords.X };
            coords.Y += MainMenuButton.Height / 2;
            this.MainMenuButton.Y = coords.Y;
            this.MainMenuButton.AddToCanvas(this.Canvas);
            coords.Y += MainMenuButton.Height / 2 + buttonSpacing;

            if (Game.Content.Common.ShowExitButton)
            {
                this.ExitButton = new TextButton(ButtonType.MainMenuButton) { Text = "Exit", X = coords.X };
                coords.Y += ExitButton.Height / 2;
                this.ExitButton.Y = coords.Y;
                this.ExitButton.AddToCanvas(this.Canvas);
                coords.Y += ExitButton.Height / 2 + buttonSpacing;
            }

            this.BackButton = new TextButton(ButtonType.MainMenuButton) { Text = "Back", X = coords.X };
            coords.Y += BackButton.Height * 1.5f;
            this.BackButton.Y = coords.Y;
            this.BackButton.AddToCanvas(this.Canvas);
        }

        protected override void BackCore()
        {
            BackButton.Click?.Invoke();
        }

        protected override void MouseCore(MouseHandlerParams _params)
        {
            RestartButton.Mouse(_params);
            LevelSelectButton.Mouse(_params);
            MainMenuButton.Mouse(_params);
            ExitButton?.Mouse(_params);
            BackButton.Mouse(_params);
        }

        protected override void TouchCore(ref TouchHandlerParams _params)
        {
            RestartButton.Touch(ref _params);
            LevelSelectButton.Touch(ref _params);
            MainMenuButton.Touch(ref _params);
            ExitButton?.Touch(ref _params);
            BackButton.Touch(ref _params);
        }
    }
}
