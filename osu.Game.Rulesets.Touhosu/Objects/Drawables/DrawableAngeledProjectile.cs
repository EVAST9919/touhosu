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

        protected readonly float Speed;

        public DrawableAngeledProjectile(AngeledProjectile h)
            : base(h)
        {
            Speed = h.Speed;
        }

        protected virtual float GetAngle() => MathExtensions.GetSafeAngle(((AngeledProjectile)HitObject).Angle);

        private float angle;

        protected override void LoadComplete()
        {
            base.LoadComplete();

            angle = GetAngle();
            Rotation = angle;
        }

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
            var elapsedTime = currentTime - HitObject.StartTime;
            var xPosition = HitObject.Position.X + (elapsedTime * Speed * Math.Sin(angle * Math.PI / 180));
            var yPosition = HitObject.Position.Y + (elapsedTime * Speed * -Math.Cos(angle * Math.PI / 180));
            return new Vector2((float)xPosition, (float)yPosition);
        }

        private void updateHidden()
        {
            if (!HiddenApplied)
                return;

            var distance = GetDistanceFromPlayer.Invoke(this);

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
