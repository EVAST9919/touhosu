using osu.Game.Rulesets.Touhosu.Extensions;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public class DrawableHomingBullet : DrawableMovingBullet
    {
        public DrawableHomingBullet(HomingBullet h)
            : base(h)
        {
        }

        protected override float GetAngle() => MathExtensions.GetPlayerAngle(Player, HitObject.Position);
    }
}
