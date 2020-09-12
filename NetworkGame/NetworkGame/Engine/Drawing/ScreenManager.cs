using System;

namespace NetworkGame.Engine
{
    public class ScreenManager
    {
        public BaseScreen ActiveScreen { get; protected set; }

        public void FirsScreenAppear(BaseScreen screen)
        {
            screen.ApplyAnimation(Game.Content.Animations.FirstScreenFadeIn, false, () => screen.IsProcessInput = true, () => screen.Visible = true);
            ActiveScreen = screen;
        }

        public void ScreenFront(BaseScreen next, BaseScreen prev, Action afterPrev = null)
        {
            prev.IsProcessInput = false;
            prev.ApplyAnimation(Game.Content.Animations.ZoomOut20);
            prev.ApplyAnimation(Game.Content.Animations.FadeIn, true, () =>
            {
                next.ApplyAnimation(Game.Content.Animations.ZoomIn20, true, () => next.IsProcessInput = true, () =>
                {
                    prev.Visible = false;
                    ActiveScreen = next;
                    afterPrev?.Invoke();
                    next.Visible = true;
                });
                next.ApplyAnimation(Game.Content.Animations.FadeIn);
            });
        }

        public void ScreenBack(BaseScreen next, BaseScreen prev, Action afterPrev = null)
        {
            prev.IsProcessInput = false;
            prev.ApplyAnimation(Game.Content.Animations.ZoomIn20);
            prev.ApplyAnimation(Game.Content.Animations.FadeIn, true, () =>
            {
                next.ApplyAnimation(Game.Content.Animations.ZoomOut20, true, () => next.IsProcessInput = true, () =>
                {
                    prev.Visible = false;
                    ActiveScreen = next;
                    afterPrev?.Invoke();
                    next.Visible = true;
                });
                next.ApplyAnimation(Game.Content.Animations.FadeIn);
            });
        }

        public void ModalScreenFront(BaseScreen next, BaseScreen prev)
        {
            prev.IsProcessInput = false;
            prev.ApplyAnimation(Game.Content.Animations.FadeToModal);

            next.Visible = true;
            next.ApplyAnimation(Game.Content.Animations.ZoomIn20, true, () => next.IsProcessInput = true);
            next.ApplyAnimation(Game.Content.Animations.FadeIn);
            ActiveScreen = next;
        }

        public void ModalScreenBack(BaseScreen next, BaseScreen prev)
        {
            prev.IsProcessInput = false;
            prev.ApplyAnimation(Game.Content.Animations.ZoomIn20);
            prev.ApplyAnimation(Game.Content.Animations.FadeIn, true, () => prev.Visible = false);

            next.ApplyAnimation(Game.Content.Animations.FadeToModal, true, () => next.IsProcessInput = true);
            ActiveScreen = next;
        }

        public void ModalScreenOut(BaseScreen next, Action afterPrevs = null, params BaseScreen[] prevs)
        {
            if (prevs.Length > 0)
            {
                var firstPrev = prevs[0];
                firstPrev.IsProcessInput = false;
                firstPrev.ApplyAnimation(Game.Content.Animations.ZoomOut20);
                firstPrev.ApplyAnimation(Game.Content.Animations.FadeIn, true, () =>
                {
                    next.ApplyAnimation(Game.Content.Animations.ZoomIn20, true, () => next.IsProcessInput = true, () =>
                    {
                        firstPrev.Visible = false;
                        ActiveScreen = next;
                        afterPrevs?.Invoke();
                        next.Visible = true;
                    });
                    next.ApplyAnimation(Game.Content.Animations.FadeIn);
                });

                for (int i = 1; i < prevs.Length; ++i)
                {
                    var prev = prevs[i];

                    prev.ApplyAnimation(Game.Content.Animations.ZoomOut20);
                    prev.ApplyAnimation(Game.Content.Animations.FadeOutFromModal, false, () => prev.Visible = false);
                }
            }
            else
            {
                next.ApplyAnimation(Game.Content.Animations.ZoomIn20, true, () => next.IsProcessInput = true, () => next.Visible = true);
                next.ApplyAnimation(Game.Content.Animations.FadeIn);
            }
        }

    }

}
