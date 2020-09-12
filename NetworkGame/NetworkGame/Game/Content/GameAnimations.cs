using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using NetworkGame.Engine;
using NetworkGame.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkGame.Content
{
    public class GameAnimations
    {
        public const float DefaultDuration = 110;
        public const float DefaultDurationX2 = DefaultDuration * 2;

        public readonly RelativePercentScaleAnimation Press10;

        public readonly FixedScaleAnimation ZoomOut20;
        public readonly FixedScaleAnimation ZoomIn20;

        public readonly FixedFadeAnimation FadeIn;
        public readonly FixedFadeAnimation FirstScreenFadeIn;
        public readonly FixedFadeAnimation FadeToModal;
        public readonly FixedFadeAnimation FadeOutFromModal;

        public readonly InfinitySpinnerAnimation DisplaySpinner;

        public readonly RelativePercentScaleAnimation FieldPress10;
        public readonly RelativeRotateAnimation FieldRotate90;

        public readonly InfinitySpinnerAnimation FieldDisplaySpinner;
        public readonly InfinityFlipAnimation FieldFlip;

        public readonly FixedColorAnimation FieldWireToConnected;
        public readonly FixedColorAnimation FieldDisplayToConnected;

        public GameAnimations(GameCommon common, GameColors colors)
        {
            this.Press10 = new RelativePercentScaleAnimation(-0.1f, -0.1f, DefaultDurationX2, Functions.RoundtripQuad);

            this.ZoomOut20 = new FixedScaleAnimation(1f, 0.8f, 1f, 0.8f, DefaultDurationX2);
            this.ZoomIn20 = new FixedScaleAnimation(1f, 1.2f, 1f, 1.2f, DefaultDurationX2);

            this.FadeIn = new FixedFadeAnimation(0f, 1f, DefaultDurationX2);
            this.FirstScreenFadeIn = new FixedFadeAnimation(0f, 1f, DefaultDuration * 6);
            this.FadeToModal = new FixedFadeAnimation(1, 0.05f, DefaultDurationX2);
            this.FadeOutFromModal = new FixedFadeAnimation(0.05f, 0, DefaultDurationX2);

            this.DisplaySpinner = new InfinitySpinnerAnimation(MathHelper.TwoPi / 8, DefaultDuration);

            this.FieldPress10 = new RelativePercentScaleAnimation(-0.1f, -0.1f, DefaultDurationX2, Functions.RoundtripQuad, manager: Field.FieldManager);

            this.FieldRotate90 = new RelativeRotateAnimation(MathHelper.PiOver2, DefaultDurationX2, manager: Field.FieldManager);

            this.FieldDisplaySpinner = new InfinitySpinnerAnimation(MathHelper.TwoPi / 8, DefaultDuration, manager: Field.FieldManager);
            this.FieldFlip = new InfinityFlipAnimation(DefaultDurationX2, DefaultDurationX2 * 5, manager: Field.FieldManager);

            this.FieldWireToConnected = new FixedColorAnimation(colors.WireDisconnected, colors.WireConnected,
                DefaultDurationX2, containerType: ContainerType.Dictionary, manager: Field.FieldManager);
            this.FieldDisplayToConnected = new FixedColorAnimation(colors.DisplayDisconnected, colors.DisplayConnected,
                DefaultDurationX2, containerType: ContainerType.Dictionary, manager: Field.FieldManager);
        }

    }

}
