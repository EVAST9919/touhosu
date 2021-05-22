using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osuTK;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;

namespace osu.Game.Rulesets.Touhosu.UI.Objects
{
    public class FocusAnimation : CompositeDrawable
    {
        private readonly Sprite focus1;
        private readonly Sprite focus2;

        public FocusAnimation()
        {
            AddRangeInternal(new Drawable[]
            {
                focus1 = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(60),
                    Scale = new Vector2(0.7f),
                    Alpha = 0,
                    AlwaysPresent = true,
                },
                focus2 = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(60),
                    Scale = new Vector2(0.7f),
                    Alpha = 0,
                    AlwaysPresent = true,
                }
            });
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            focus1.Texture = focus2.Texture = textures.Get("Player/focus");
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            focus1.Spin(4000, RotationDirection.Clockwise);
            focus2.Spin(4000, RotationDirection.Counterclockwise);
        }

        public void Focus()
        {
            focus1.ScaleTo(1, 200, Easing.Out);
            focus1.FadeIn(200);

            focus2.ScaleTo(1, 200, Easing.Out);
            focus2.FadeIn(200);
        }

        public void FocusLost()
        {
            focus1.ScaleTo(0.7f, 200, Easing.Out);
            focus1.FadeOut(200);

            focus2.ScaleTo(0.7f, 200, Easing.Out);
            focus2.FadeOut(200);
        }
    }
}
