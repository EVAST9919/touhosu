using osuTK;
using System;
using osu.Game.Rulesets.Touhosu.Extensions;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public class DrawableMovingBullet : DrawableBullet
    {
        protected readonly float SpeedMultiplier;

        public DrawableMovingBullet(MovingBullet h)
            : base(h)
        {
            SpeedMultiplier = (float)(MathExtensions.Map((float)h.SpeedMultiplier, 0, 3, 0.4f, 0.7f) * h.DeltaMultiplier / 3f);
        }

        protected override bool AffectPlayer() => true;

        protected virtual float GetAngle() => MathExtensions.GetSafeAngle(((MovingBullet)HitObject).Angle);

        private float? angle;

        protected override void Update()
        {
            base.Update();

            Vector2 newPosition = (Time.Current > HitObject.StartTime) ? UpdatePosition(Time.Current) : HitObject.Position;

            if (newPosition == Position)
                return;

            Position = newPosition;
        }

        protected virtual Vector2 UpdatePosition(double currentTime)
        {
            if (angle == null)
                angle = GetAngle();

            var elapsedTime = Clock.CurrentTime - HitObject.StartTime;
            var xPosition = HitObject.Position.X + (elapsedTime * SpeedMultiplier * Math.Sin((angle ?? 0) * Math.PI / 180));
            var yPosition = HitObject.Position.Y + (elapsedTime * SpeedMultiplier * -Math.Cos((angle ?? 0) * Math.PI / 180));
            return new Vector2((float)xPosition, (float)yPosition);
        }
    }
}
