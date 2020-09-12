using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGame.Engine.UI
{
    public class TextButton : BaseButton<TextSprite>
    {
        public string Text
        {
            get { return m_content.Text; }
            set { m_content.Text = value; }
        }

        public TextButton(ButtonType buttonType = ButtonType.Empty, BaseAnimation clickAnimation = null)
            :base(buttonType, clickAnimation)
        {
            this.m_content = new TextSprite(Game.Content.Fonts.ButtonFont);
            this.m_content.SetColor(Game.Content.Colors.ButtonText);
        }
    }
}
