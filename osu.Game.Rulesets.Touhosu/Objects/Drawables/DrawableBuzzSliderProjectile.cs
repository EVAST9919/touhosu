namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public class DrawableBuzzSliderProjectile : DrawableAngeledProjectile
    {
        protected override float BaseSize => base.BaseSize * 0.65f;

        protected override string ProjectileName => "Grain";

        public DrawableBuzzSliderProjectile(BuzzSliderProjectile h)
            : base(h)
        {
        }
    }
}
