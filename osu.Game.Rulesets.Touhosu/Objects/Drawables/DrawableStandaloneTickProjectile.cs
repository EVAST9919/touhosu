namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public class DrawableStandaloneTickProjectile : DrawableStandaloneProjectile
    {
        public DrawableStandaloneTickProjectile(StandaloneTickProjectile h)
            : base(h)
        {
        }

        protected override DrawableProjectile CreateDrawableProjectile(Projectile projectile) => new DrawableTickProjectile((TickProjectile)projectile);
    }
}
