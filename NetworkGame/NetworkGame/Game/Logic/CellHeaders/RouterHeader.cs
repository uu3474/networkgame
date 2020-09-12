using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NetworkGame.Engine;

namespace NetworkGame.Logic
{
    public class RouterHeader : BaseCellHeader
    {
        AtlasSprite m_routerSprite;
        AtlasSprite m_globeSprite;

        public override float X
        {
            get { return base.X; }
            set
            {
                base.X = value;
                m_routerSprite.X = base.X;
                m_globeSprite.X = base.X;
            }
        }
        public override float Y
        {
            get { return base.Y; }
            set
            {
                base.Y = value;
                m_routerSprite.Y = base.Y;
                m_globeSprite.Y = base.Y;
            }
        }

        public override void InitRotate(float rotation)
        {
            m_routerSprite.Rotation = rotation;
            m_globeSprite.Rotation = rotation;
        }

        public RouterHeader()
        {
            this.m_routerSprite = new AtlasSprite() { Frame = Game.Content.Textures.DisplayFront };
            this.m_routerSprite.SetColor(Game.Content.Colors.Router);

            this.m_globeSprite = new AtlasSprite() { Frame = Game.Content.Textures.DisplayGlobe };
            this.m_globeSprite.SetColor(Game.Content.Colors.RouterIcon);

        }

        public override void Draw(SpriteBatch spriteBatch, BaseSprite viewport)
        {
            m_routerSprite.Draw(spriteBatch, viewport);
            m_globeSprite.Draw(spriteBatch, viewport);
        }

        public override void Rotate90(bool animate)
        {
            if (animate)
            {
                Game.Content.Animations.FieldRotate90.Apply(m_routerSprite);
                Game.Content.Animations.FieldRotate90.Apply(m_globeSprite);

                Game.Content.Animations.FieldPress10.Apply(m_routerSprite);
                Game.Content.Animations.FieldPress10.Apply(m_globeSprite);
            }
        }
    }
}
