using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetworkGame.Engine;
using NetworkGame.Engine.UI;
using NetworkGame.Logic;

namespace NetworkGame.Menus
{
    public class MainMenu : DefaultCanvasScreen
    {
        Sprite m_mainLogoSprite;

        public Profile Profile { get; protected set; }
        public TextButton PlayOrContinueButton { get; protected set; }
        public TextButton LevelSelectButton { get; protected set; }
        //public TextButton OptionsButton { get; protected set; }
        public TextButton ExitButton { get; protected set; }

        public MainMenu(GraphicsDevice device, Profile profile)
            :base(device, Game.Content.GetSizeInDpi(284), Game.Content.GetSizeInDpi(Game.Content.Common.ShowExitButton ? 500 : 440))
        {
            this.Profile = profile;

            int mainLogoOffset = Game.Content.GetSizeInDpi(150);
            int buttonSpacing = Game.Content.GetSizeInDpi(20);

            var itemCoords = new Vector2(this.Canvas.View.Width / 2, this.Canvas.View.Height / 2 + buttonSpacing);

            this.m_mainLogoSprite = new Sprite() { Texture = Game.Content.Textures.MainLogo, X = itemCoords.X };
            this.m_mainLogoSprite.Y = itemCoords.Y - mainLogoOffset - this.m_mainLogoSprite.Height / 2;
            this.Canvas.Add(this.m_mainLogoSprite);

            this.PlayOrContinueButton = new TextButton(ButtonType.MainMenuButton) { Text = "Play or continue", X = itemCoords.X };
            itemCoords.Y += this.PlayOrContinueButton.Height / 2;
            this.PlayOrContinueButton.Y = itemCoords.Y;
            this.PlayOrContinueButton.AddToCanvas(this.Canvas);
            itemCoords.Y += this.PlayOrContinueButton.Height / 2 + buttonSpacing;

            this.LevelSelectButton = new TextButton(ButtonType.MainMenuButton) { Text = "Level select", X = itemCoords.X };
            itemCoords.Y += this.LevelSelectButton.Height / 2;
            this.LevelSelectButton.Y = itemCoords.Y;
            this.LevelSelectButton.AddToCanvas(this.Canvas);
            itemCoords.Y += this.LevelSelectButton.Height / 2 + buttonSpacing;

            /*this.OptionsButton = new Button(ButtonType.MainMenuButton) { Text = "Options", X = itemCoords.X };
            itemCoords.Y += this.OptionsButton.Height / 2;
            this.OptionsButton.Y = itemCoords.Y;
            this.OptionsButton.AddToCanvas(this.Canvas);
            itemCoords.Y += this.OptionsButton.Height / 2 + buttonSpacing;*/

            if (Game.Content.Common.ShowExitButton)
            {
                this.ExitButton = new TextButton(ButtonType.MainMenuButton) { Text = "Exit", X = itemCoords.X };
                itemCoords.Y += this.ExitButton.Height / 2;
                this.ExitButton.Y = itemCoords.Y;
                this.ExitButton.AddToCanvas(this.Canvas);
            }
        }

        public override void OnVisibleChange()
        {
            base.OnVisibleChange();

            if (!Visible)
                return;

            PlayOrContinueButton.Text = (Profile.IsFirstPlay ? "Play" : "Continue");
        }

        protected override void MouseCore(MouseHandlerParams _params)
        {
            PlayOrContinueButton.Mouse(_params);
            LevelSelectButton.Mouse(_params);
            //OptionsButton.Mouse(_params);
            ExitButton?.Mouse(_params);
        }

        protected override void TouchCore(ref TouchHandlerParams _params)
        {
            PlayOrContinueButton.Touch(ref _params);
            LevelSelectButton.Touch(ref _params);
            //OptionsButton.Touch(ref _params);
            ExitButton?.Touch(ref _params);
        }
    }
}
