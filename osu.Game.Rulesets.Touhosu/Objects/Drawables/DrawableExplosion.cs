namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public abstract class DrawableExplosion : DrawableGroupedTouhosuHitObject<AngeledProjectile>
    {
        public DrawableExplosion(Explosion h)
            : base(h)
        {
        }

        protected override DrawableProjectile CreateDrawableProjectile(AngeledProjectile projectile) => new DrawableAngeledProjectile(projectile);
    }
}
