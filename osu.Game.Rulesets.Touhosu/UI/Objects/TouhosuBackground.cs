using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace osu.Game.Rulesets.Touhosu.UI.Objects
{
    public class TouhosuBackground : CompositeDrawable
    {
        private readonly Container bgContainer;

        public TouhosuBackground()
        {
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            InternalChildren = new Drawable[]
            {
                bgContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            setBackground();
        }

        private void setBackground()
        {
            Drawable newBackground;

            newBackground = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Children = new[]
                {
                    new Box
                    {
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.TopCentre,
                        RelativeSizeAxes = Axes.X,
                        Height = 1,
                        EdgeSmoothness = Vector2.One
                    },
                    new Box
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.BottomCentre,
                        RelativeSizeAxes = Axes.X,
                        Height = 1,
                        EdgeSmoothness = Vector2.One
                    },
                    new Box
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreRight,
                        RelativeSizeAxes = Axes.Y,
                        Width = 1,
                        EdgeSmoothness = Vector2.One
                    },
                    new Box
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreLeft,
                        RelativeSizeAxes = Axes.Y,
                        Width = 1,
                        EdgeSmoothness = Vector2.One
                    }
                }
            };

            bgContainer.Child = newBackground;
        }
    }
}
