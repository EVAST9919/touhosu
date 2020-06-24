namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public class DrawableReverseSliderBullet : DrawableMovingBullet
    {
        protected override float GetBaseSize() => base.GetBaseSize() / 2;

        public DrawableReverseSliderBullet(ReverseSliderBullet h)
            : base(h)
        {
        }
    }
}
