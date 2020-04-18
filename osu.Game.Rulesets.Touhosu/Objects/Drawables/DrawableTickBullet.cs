namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public class DrawableTickBullet : DrawableMovingBullet
    {
        protected override float GetBaseSize() => 20;

        public DrawableTickBullet(TickBullet h)
            : base(h)
        {
        }
    }
}
