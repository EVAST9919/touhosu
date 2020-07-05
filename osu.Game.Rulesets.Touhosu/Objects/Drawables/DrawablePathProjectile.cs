using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Touhosu.UI;
using osuTK;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public class DrawablePathProjectile : DrawableProjectile
    {
        private readonly IHasPathWithRepeats path;
        private readonly float pathTimeOffset;
        private readonly float intensity;

        public DrawablePathProjectile(PathProjectile h)
            : base(h)
        {
            path = h.Path;
            pathTimeOffset = h.TimeOffset;
            intensity = h.Intensity;
            Alpha = intensity;
        }

        protected override void Update()
        {
            base.Update();

            var currentTime = Time.Current - pathTimeOffset;

            Vector2 newPosition = (currentTime > HitObject.StartTime ? UpdatePosition(currentTime) : HitObject.Position) * new Vector2(TouhosuPlayfield.X_SCALE_MULTIPLIER, 0.5f);

            if (newPosition == Position)
                return;

            Position = newPosition;
        }

        protected virtual Vector2 UpdatePosition(double currentTime)
        {
            var elapsedTime = currentTime - HitObject.StartTime;

            if (elapsedTime > path.Duration)
                return HitObject.Position + path.CurvePositionAt(1);

            return HitObject.Position + path.CurvePositionAt(elapsedTime / path.Duration);
        }

        private double hitTime;

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (timeOffset > path.Duration)
            {
                hitTime = timeOffset;
                ApplyResult(r => r.Type = HitResult.Meh);
            }
        }

        protected override void UpdateInitialTransforms()
        {
            this.ScaleTo(intensity, HitObject.TimePreempt);
        }

        protected override void UpdateStateTransforms(ArmedState state)
        {
            switch (state)
            {
                case ArmedState.Hit:
                    this.Delay(hitTime).FadeOut(200 / intensity, Easing.Out);
                    break;
            }
        }
    }
}
