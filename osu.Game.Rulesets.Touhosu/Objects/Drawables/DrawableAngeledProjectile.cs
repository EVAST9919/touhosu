using JetBrains.Annotations;
using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public class DrawableAngeledProjectile : DrawableConstantMovingProjectile<AngeledProjectile>
    {
        public readonly IBindable<float> AngleBindable = new Bindable<float>();

        protected override float GetTargetAngle() => AngleBindable.Value;

        public DrawableAngeledProjectile()
            : this(null)
        {
        }

        public DrawableAngeledProjectile([CanBeNull] AngeledProjectile h = null)
            : base(h)
        {
        }

        protected override void OnApply()
        {
            base.OnApply();

            AngleBindable.BindTo(HitObject.AngleBindable);
        }

        protected override void OnFree()
        {
            base.OnFree();

            AngleBindable.UnbindFrom(HitObject.AngleBindable);
        }
    }
}
