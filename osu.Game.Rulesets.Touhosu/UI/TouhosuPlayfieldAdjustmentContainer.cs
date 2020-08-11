using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.UI;
using osuTK;

namespace osu.Game.Rulesets.Touhosu.UI
{
    public class TouhosuPlayfieldAdjustmentContainer : PlayfieldAdjustmentContainer
    {
        protected override Container<Drawable> Content => content;
        private readonly Container content;

        private const float playfield_size_adjust = 0.83f;

        public TouhosuPlayfieldAdjustmentContainer()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            Size = new Vector2(playfield_size_adjust);

            InternalChild = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                FillMode = FillMode.Fit,
                FillAspectRatio = 4f / 5,
                Child = content = new ScalingContainer { RelativeSizeAxes = Axes.Both }
            };
        }

        /// <summary>
        /// A <see cref="Container"/> which scales its content relative to a target width.
        /// </summary>
        private class ScalingContainer : Container
        {
            protected override void Update()
            {
                base.Update();
                Scale = new Vector2(Parent.ChildSize.X / TouhosuPlayfield.PLAYFIELD_SIZE.X);
                Size = Vector2.Divide(Vector2.One, Scale);
            }
        }
    }
}
