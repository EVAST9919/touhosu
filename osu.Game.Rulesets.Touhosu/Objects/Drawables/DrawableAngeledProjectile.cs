using osuTK;
using System;
using osu.Game.Rulesets.Touhosu.Extensions;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public class DrawableAngeledProjectile : DrawableProjectile
    {
        private const int hidden_distance = 70;
        private const int hidden_distance_buffer = 50;

        public bool HiddenApplied;

        protected readonly float SpeedMultiplier;

        public DrawableAngeledProjectile(AngeledProjectile h)
            : base(h)
        {
            SpeedMultiplier = (float)(MathExtensions.Map((float)h.SpeedMultiplier, 0, 3, 0.4f, 0.5f) * h.DeltaMultiplier / 2.8f);
        }

        protected virtual float GetAngle() => MathExtensions.GetSafeAngle(((AngeledProjectile)HitObject).Angle);

        private float? angle;

        protected override void Update()
        {
            base.Update();

            updateHidden();

            var time = Time.Current;

            Vector2 newPosition = (time > HitObject.StartTime) ? UpdatePosition(time) : HitObject.Position;

            if (newPosition == Position)
                return;

            Position = newPosition;
        }

        protected virtual Vector2 UpdatePosition(double currentTime)
        {
            if (angle == null)
            {
                angle = GetAngle();
                Rotation = angle ?? 0;
            }

            var elapsedTime = currentTime - HitObject.StartTime;
            var xPosition = HitObject.Position.X + (elapsedTime * SpeedMultiplier * Math.Sin((angle ?? 0) * Math.PI / 180));
            var yPosition = HitObject.Position.Y + (elapsedTime * SpeedMultiplier * -Math.Cos((angle ?? 0) * Math.PI / 180));
            return new Vector2((float)xPosition, (float)yPosition);
        }

        private void updateHidden()
        {
            if (!HiddenApplied)
                return;

            var distance = CheckDistance.Invoke(this);

            if (distance > hidden_distance + hidden_distance_buffer)
            {
                Alpha = 1;
                return;
            }

            if (distance < hidden_distance)
            {
                Alpha = 0;
                return;
            }

            Alpha = MathExtensions.Map(distance - hidden_distance, 0, hidden_distance_buffer, 0, 1);
        }
    }
}
