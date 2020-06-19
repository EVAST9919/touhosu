namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public class DrawableTickBullet : DrawableHomingBullet
    {
        protected override float GetBaseSize() => base.GetBaseSize() / 2;

        public DrawableTickBullet(TickBullet h)
            : base(h)
        {
        }
    }
}
