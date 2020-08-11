using System;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public class DrawableHomingProjectile : DrawableAngeledProjectile
    {
        public DrawableHomingProjectile(HomingProjectile h)
            : base(h)
        {
        }

        public Func<DrawableHomingProjectile, float> PlayerAngle;

        protected override float GetAngle() => PlayerAngle?.Invoke(this) ?? 180;
    }
}
