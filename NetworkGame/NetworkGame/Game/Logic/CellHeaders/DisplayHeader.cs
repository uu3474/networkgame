using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetworkGame.Engine;

namespace NetworkGame.Logic
{
    public class DisplayHeader : BaseCellHeader
    {
        AtlasSprite m_displayBackSprite;
        AtlasSprite m_displayFrontSprite;
        AtlasSprite m_globeSprite;
        AtlasSprite m_spinnerSprite;
        Field m_field;

        public override float X
        {
            get { return base.X; }
            set
            {
                base.X = value;
                m_displayBackSprite.X = base.X;
                m_displayFrontSprite.X = base.X;
                m_spinnerSprite.X = base.X;
                m_globeSprite.X = base.X;
            }
        }
        public override float Y
        {
            get { return base.Y; }
            set
            {
                base.Y = value;
                m_displayBackSprite.Y = base.Y;
                m_displayFrontSprite.Y = base.Y;
                m_spinnerSprite.Y = base.Y;
                m_globeSprite.Y = base.Y;
            }
        }

        public DisplayHeader(Field field)
        {
            this.m_field = field;

            this.m_displayBackSprite = new AtlasSprite() { Frame = Game.Content.Textures.DisplayBack };

            this.m_displayFrontSprite = new AtlasSprite() { Frame = Game.Content.Textures.DisplayFront };

            this.m_spinnerSprite = new AtlasSprite() { Frame = Game.Content.Textures.DisplaySpinner };
            this.m_spinnerSprite.SetColor(Color.DarkGray);
            Game.Content.Animations.FieldDisplaySpinner.Apply(this.m_spinnerSprite);

            this.m_globeSprite = new AtlasSprite { Frame = Game.Content.Textures.DisplayGlobe };
            Game.Content.Animations.FieldFlip.Apply(this.m_globeSprite);

            this.UpdateSignalCore(false, true, false);
        }

        public override void InitRotate(float rotation)
        {
            m_displayBackSprite.Rotation = rotation;
            m_displayFrontSprite.Rotation = rotation;
            m_globeSprite.Rotation = rotation;
            m_spinnerSprite.Rotation = rotation;
        }

        public override void Draw(SpriteBatch spriteBatch, BaseSprite viewport)
        {
            m_displayBackSprite.Draw(spriteBatch, viewport);
            m_displayFrontSprite.Draw(spriteBatch, viewport);
            m_spinnerSprite.Draw(spriteBatch, viewport);
            m_globeSprite.Draw(spriteBatch, viewport);
        }

        public override void Rotate90(bool animate)
        {
            if (animate)
            {
                Game.Content.Animations.FieldRotate90.Apply(m_displayBackSprite);
                Game.Content.Animations.FieldRotate90.Apply(m_displayFrontSprite);
                Game.Content.Animations.FieldRotate90.Apply(m_spinnerSprite);
                Game.Content.Animations.FieldRotate90.Apply(m_globeSprite);

                Game.Content.Animations.FieldPress10.Apply(m_displayBackSprite);
                Game.Content.Animations.FieldPress10.Apply(m_displayFrontSprite);
                Game.Content.Animations.FieldPress10.Apply(m_spinnerSprite);
                Game.Content.Animations.FieldPress10.Apply(m_globeSprite);
            }
        }

        public override void UpdateSignalCore(bool signal, bool init, bool animate)
        {
            if (animate)
            {
                if (!Game.Content.Animations.FieldWireToConnected.Revert(m_displayBackSprite))
                    Game.Content.Animations.FieldWireToConnected.Apply(m_displayBackSprite, !signal);

                if (!Game.Content.Animations.FieldDisplayToConnected.Revert(m_displayFrontSprite))
                    Game.Content.Animations.FieldDisplayToConnected.Apply(m_displayFrontSprite, !signal);
            }
            else
            {
                m_displayBackSprite.SetColor(signal ? Game.Content.Colors.WireConnected : Game.Content.Colors.WireDisconnected);
                m_displayFrontSprite.SetColor(signal ? Game.Content.Colors.DisplayConnected : Game.Content.Colors.DisplayDisconnected);
            }
            
            m_globeSprite.Visible = signal;
            m_spinnerSprite.Visible = !signal;

            m_field.ConnectedDisplaysCount += (signal ? 1 : (init ? 0 : -1));
        }

    }

}
