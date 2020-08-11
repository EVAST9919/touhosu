namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public class DrawableStandaloneAngeledProjectile : DrawableStandaloneProjectile
    {
        public DrawableStandaloneAngeledProjectile(StandaloneAngeledProjectile h)
            : base(h)
        {
        }

        protected override DrawableProjectile CreateDrawableProjectile(Projectile projectile) => new DrawableAngeledProjectile((AngeledProjectile)projectile);
    }
}
