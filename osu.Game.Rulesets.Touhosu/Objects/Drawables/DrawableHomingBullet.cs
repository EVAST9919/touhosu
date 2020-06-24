using System;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public class DrawableHomingBullet : DrawableMovingBullet
    {
        public DrawableHomingBullet(HomingBullet h)
            : base(h)
        {
        }

        public Func<DrawableHomingBullet, float> PlayerAngle;

        protected override float GetAngle() => PlayerAngle.Invoke(this);
    }
}
