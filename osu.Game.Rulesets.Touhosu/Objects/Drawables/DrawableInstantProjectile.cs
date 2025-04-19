using JetBrains.Annotations;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public partial class DrawableInstantProjectile : DrawableProjectile<InstantProjectile>
    {
        protected override bool CanHitPlayer => false;

        public DrawableInstantProjectile()
            : this(null)
        {
        }

        public DrawableInstantProjectile([CanBeNull] InstantProjectile h = null)
            : base(h)
        {
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            base.CheckForResult(userTriggered, timeOffset);

            if (timeOffset < 0)
                return;

            ApplyResult((r, u) => r.Type = HitResult.IgnoreHit);
        }

        protected override void UpdateHitStateTransforms(ArmedState state)
        {
            switch (state)
            {
                case ArmedState.Hit:
                    this.ScaleTo(0, 150).Then().FadeOut();
                    break;
            }
        }
    }
}
