namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public class DrawableReverseSliderBullet : DrawableMovingBullet
    {
        protected override float GetBaseSize() => base.GetBaseSize() * 0.65f;

        protected override string ProjectileName() => "Grain";

        public DrawableReverseSliderBullet(ReverseSliderBullet h)
            : base(h)
        {
        }
    }
}
