using Microsoft.Xna.Framework.Graphics;

namespace NetworkGame.Engine.UI
{
    public class SpriteButton : BaseButton<Sprite>
    {
        public Texture2D Texture
        {
            get { return m_content.Texture; }
            set { m_content.Texture = value; }
        }

        public SpriteButton(ButtonType buttonType = ButtonType.Empty, BaseAnimation clickAnimation = null)
            : base(buttonType, clickAnimation)
        {
            this.m_content = new Sprite();
        }

    }

}
