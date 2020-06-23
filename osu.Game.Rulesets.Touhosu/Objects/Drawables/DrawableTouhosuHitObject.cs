using osu.Game.Rulesets.Touhosu.UI.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osuTK.Graphics;
using System.Collections.Generic;
using osu.Framework.Allocation;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public abstract class DrawableTouhosuHitObject : DrawableHitObject<TouhosuHitObject>
    {
        protected override Color4 GetComboColour(IReadOnlyList<Color4> comboColours) =>
            comboColours[(HitObject.IndexInBeatmap + 1) % comboColours.Count];

        [Resolved]
        protected TouhosuPlayer Player { get; private set; }

        protected DrawableTouhosuHitObject(TouhosuHitObject hitObject)
            : base(hitObject)
        {
        }

        protected sealed override double InitialLifetimeOffset => HitObject.TimePreempt;

        protected override void UpdateStateTransforms(ArmedState state)
        {
            switch (state)
            {
                case ArmedState.Idle:
                    // Manually set to reduce the number of future alive objects to a bare minimum.
                    LifetimeStart = HitObject.StartTime - HitObject.TimePreempt;
                    break;
            }
        }
    }
}
