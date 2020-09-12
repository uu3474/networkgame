namespace NetworkGame.Engine.UI
{
    public class AtlasSpriteButton : BaseButton<AtlasSprite>
    {
        public Frame Frame
        {
            get { return m_content.Frame; }
            set { m_content.Frame = value; }
        }

        public AtlasSpriteButton(ButtonType buttonType = ButtonType.Empty, BaseAnimation clickAnimation = null)
            :base(buttonType, clickAnimation)
        {
            this.m_content = new AtlasSprite();
        }

    }

}
