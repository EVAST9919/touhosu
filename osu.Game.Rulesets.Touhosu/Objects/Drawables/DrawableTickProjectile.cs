namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public class DrawableTickProjectile : DrawableHomingProjectile
    {
        protected override float BaseSize() => base.BaseSize() / 2;

        public DrawableTickProjectile(TickProjectile h)
            : base(h)
        {
        }
    }
}
