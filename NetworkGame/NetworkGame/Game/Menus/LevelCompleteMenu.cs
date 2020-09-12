using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetworkGame.Content;
using NetworkGame.Engine;
using NetworkGame.Engine.UI;
using NetworkGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGame.Menus
{
    public class LevelCompleteMenu : DefaultCanvasScreen
    {
        TextSprite completeSprite;
        TextSprite timeSprite;
        TextSprite rotatesCountSprite;
        public TextButton NextButton { get; protected set; }
        public TextButton RestartButton { get; protected set; }
        public TextButton LevelSelectButton { get; protected set; }
        public TextButton MainMenuButton { get; protected set; }
        public TextButton ExitButton { get; protected set; }

        public LevelCompleteMenu(GraphicsDevice device)
            : base(device, Game.Content.GetSizeInDpi(240), Game.Content.GetSizeInDpi(Game.Content.Common.ShowExitButton ? 500 : 440))
        {
            this.Canvas.View.SetColor(Game.Content.Colors.ButtonBack);
            this.Canvas.View.Alpha = Game.Content.Colors.ButtonBackAlpha;

            int buttonSpacing = Game.Content.GetSizeInDpi(20);
            int defaultTextSpriteHeight = Game.Content.GetSizeInDpi(40);

            var coords = new Vector2(this.Canvas.View.Width / 2, buttonSpacing);

            this.completeSprite = new TextSprite(Game.Content.Fonts.ButtonFont) { Text = "Level complete", X = coords.X };
            coords.Y += defaultTextSpriteHeight / 2;
            this.completeSprite.Y = coords.Y;
            this.Canvas.Add(this.completeSprite);
            coords.Y += defaultTextSpriteHeight / 2 + buttonSpacing;

            this.rotatesCountSprite = new TextSprite(Game.Content.Fonts.ButtonFont) { Text = "- rotate(s)", X = coords.X };
            coords.Y += defaultTextSpriteHeight / 2;
            this.rotatesCountSprite.Y = coords.Y;
            this.Canvas.Add(this.rotatesCountSprite);
            coords.Y += defaultTextSpriteHeight / 2 + buttonSpacing;

            this.timeSprite = new TextSprite(Game.Content.Fonts.ButtonFont) { Text = "-h -m -s", X = coords.X };
            coords.Y += defaultTextSpriteHeight / 2;
            this.timeSprite.Y = coords.Y;
            this.Canvas.Add(this.timeSprite);
            coords.Y += defaultTextSpriteHeight / 2 + buttonSpacing;

            this.NextButton = new TextButton(ButtonType.MainMenuButton) { Text = "Next", X = coords.X };
            coords.Y += NextButton.Height / 2;
            this.NextButton.Y = coords.Y;
            this.NextButton.AddToCanvas(this.Canvas);
            coords.Y += NextButton.Height / 2 + buttonSpacing;

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
            }
        }

        public void SetCompleteInfo(CompleteBundle bundle)
        {
            rotatesCountSprite.Text = bundle.RotatesCount.ToString() + " rotate(s)";
            timeSprite.Text = string.Format("{0}h {1}m {2}s", bundle.Time.Hours * 24, bundle.Time.Minutes, bundle.Time.Seconds);
        }

        protected override void BackCore()
        {
            LevelSelectButton.Click?.Invoke();
        }

        protected override void MouseCore(MouseHandlerParams _params)
        {
            NextButton.Mouse(_params);
            RestartButton.Mouse(_params);
            LevelSelectButton.Mouse(_params);
            MainMenuButton.Mouse(_params);
            ExitButton?.Mouse(_params);
        }

        protected override void TouchCore(ref TouchHandlerParams _params)
        {
            NextButton.Touch(ref _params);
            RestartButton.Touch(ref _params);
            LevelSelectButton.Touch(ref _params);
            MainMenuButton.Touch(ref _params);
            ExitButton?.Touch(ref _params);
        }
    }
}
