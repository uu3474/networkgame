using Microsoft.Xna.Framework;
using NetworkGame.Engine;
using NetworkGame.Engine.UI;
using NetworkGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGame.UI
{
    public class FieldStatus
    {
        int m_spacing;
        AutoSizeLabel m_levelIndex;
        AutoSizeLabel m_levelSizeLabel;
        AtlasSprite m_levelDifficult;
        AutoSizeLabel m_conectedLabel;

        public FieldStatus()
        {
            m_spacing = Game.Content.GetSizeInDpi(10);

            var font = Game.Content.Fonts.FieldStatusFont;
            m_levelIndex = new AutoSizeLabel(font) { Fixed = true };
            m_levelSizeLabel = new AutoSizeLabel(font) { Fixed = true };
            m_levelDifficult = new AtlasSprite() { Frame = Game.Content.Textures.ButtonCircle, Fixed = true };
            m_conectedLabel = new AutoSizeLabel(font) { Fixed = true };
        }

        public void AddToCanvas(DefaultCanvas canvas)
        {
            m_levelIndex.AddToCanvas(canvas);
            m_levelSizeLabel.AddToCanvas(canvas);
            canvas.Add(m_levelDifficult);
            m_conectedLabel.AddToCanvas(canvas);
        }

        public void SetInitialStatus(LevelDescriptor descriptor, int connectedDisplaysCount, int displaysCount)
        {
            float scale = 1f;
            float difficultScale = scale / 2;
            var padding = new Vector2(8, 2);

            m_levelIndex.SetParams('#' + (descriptor.Index + 1).ToString(), scale, padding);
            m_levelSizeLabel.SetParams(descriptor.Width + " x " + descriptor.Height, scale, padding);
            m_levelDifficult.ScaleX = difficultScale;
            m_levelDifficult.ScaleY = difficultScale;
            m_levelDifficult.SetColor(Game.Content.Colors.DifficultColors[descriptor.Difficult]);
            m_conectedLabel.SetParams(connectedDisplaysCount.ToString() + " / " + displaysCount.ToString(), scale, padding);

            m_levelIndex.X = m_levelIndex.Width / 2 + m_spacing;
            m_levelIndex.Y = m_levelIndex.Height / 2 + m_spacing;

            m_levelSizeLabel.X = m_levelIndex.X + m_levelIndex.Width / 2 + m_levelSizeLabel.Width / 2 + m_spacing;
            m_levelSizeLabel.Y = m_levelIndex.Y;

            m_levelDifficult.X = m_levelSizeLabel.X + m_levelSizeLabel.Width / 2 + m_levelDifficult.Width * m_levelDifficult.ScaleX / 2 + m_spacing;
            m_levelDifficult.Y = m_levelIndex.Y;

            m_conectedLabel.X = m_conectedLabel.Width / 2 + m_spacing;
            m_conectedLabel.Y = m_levelIndex.Y + m_levelIndex.Height / 2 + m_conectedLabel.Height / 2 + m_spacing;
        }

        public void SetStatus(int connectedDisplaysCount, int displaysCount)
        {
            m_conectedLabel.Text = connectedDisplaysCount.ToString() + " / " + displaysCount.ToString();
            m_conectedLabel.X = m_conectedLabel.Width / 2 + m_spacing;
        }
    }
}
