using Microsoft.Xna.Framework.Graphics;

namespace NetworkGame.Content
{
    public class GameEffects
    {
        public Effect LongShadow { get; }

        public GameEffects(GameCommon common)
        {
            if(common.UseAdvancedEffects)
                this.LongShadow = common.Manager.Load<Effect>("common/long_shadow");
        }

    }

}
