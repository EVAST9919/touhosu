using osu.Game.Rulesets.Touhosu.UI.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osuTK.Graphics;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public abstract class DrawableTouhosuHitObject : DrawableHitObject<TouhosuHitObject>
    {
        protected override Color4 GetComboColour(IReadOnlyList<Color4> comboColours) =>
            comboColours[(HitObject.IndexInBeatmap + 1) % comboColours.Count];

        protected TouhosuPlayer Player;

        protected DrawableTouhosuHitObject(TouhosuHitObject hitObject)
            : base(hitObject)
        {
        }

        protected sealed override double InitialLifetimeOffset => HitObject.TimePreempt;

        public void GetPlayerToTrace(TouhosuPlayer player) => Player = player;

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
