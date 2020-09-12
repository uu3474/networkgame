using Microsoft.Xna.Framework.Graphics;
using NetworkGame.Engine;
using NetworkGame.Engine.UI;
using NetworkGame.Logic;

namespace NetworkGame.UI
{
    public class LevelListViewItem : ListViewItem
    {
        TextSprite m_index;
        TextSprite m_desc;
        AtlasSprite m_difficultBack;

        public LevelDescriptor Descriptor { get; protected set; }

        public override float X
        {
            get { return base.X; }
            set
            {
                base.X = value;
                m_index.X = value;
                m_desc.X = value;
                m_difficultBack.X = value;
            }
        }
        public override float Y
        {
            get { return base.Y; }
            set
            {
                base.Y = value;
                m_index.Y = value;
                m_desc.Y = value;
                m_difficultBack.Y = value;
            }
        }
        public override int Width
        {
            get { return base.Width; }
            set
            {
                base.Width = value;
                m_index.OriginX = Width / 2 - (Height - m_index.Height) / 2 - m_index.Font.BaselineOffset;
                m_difficultBack.OriginX = -Width / 2 + (Height - m_difficultBack.Height) / 2 + m_difficultBack.Width;
            }
        }

        public LevelListViewItem(LevelDescriptor descriptor)
        {
            this.Height = Game.Content.GetSizeInDpi(40);
            this.Descriptor = descriptor;

            this.m_index = new TextSprite(Game.Content.Fonts.ButtonFont) { Text = (descriptor.Index + 1).ToString() };
            this.m_desc = new TextSprite(Game.Content.Fonts.ButtonFont) { Text = descriptor.Width + " x " + descriptor.Height };
            this.m_difficultBack = new AtlasSprite() { Frame = Game.Content.Textures.ButtonCircle };
            this.m_difficultBack.SetColor(Game.Content.Colors.DifficultColors[descriptor.Difficult]);
        }

        public override void Draw(SpriteBatch spriteBatch, BaseSprite viewport)
        {
            base.Draw(spriteBatch, viewport);
            m_index.Draw(spriteBatch, viewport);
            m_desc.Draw(spriteBatch, viewport);
            m_difficultBack.Draw(spriteBatch, viewport);
        }

        public override void Tap()
        {
            base.Tap();
            Game.Content.Animations.Press10.Apply(m_index);
            Game.Content.Animations.Press10.Apply(m_desc);
            Game.Content.Animations.Press10.Apply(m_difficultBack);
        }
    }
}
