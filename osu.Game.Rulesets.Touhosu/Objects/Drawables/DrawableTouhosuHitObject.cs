using osu.Game.Rulesets.Objects.Drawables;
using osuTK.Graphics;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public abstract class DrawableTouhosuHitObject : DrawableHitObject<TouhosuHitObject>
    {
        protected override Color4 GetComboColour(IReadOnlyList<Color4> comboColours) =>
            comboColours[(HitObject.IndexInBeatmap + 1) % comboColours.Count];

        protected DrawableTouhosuHitObject(TouhosuHitObject hitObject)
            : base(hitObject)
        {
        }

        protected sealed override double InitialLifetimeOffset => HitObject.TimePreempt;
    }
}
