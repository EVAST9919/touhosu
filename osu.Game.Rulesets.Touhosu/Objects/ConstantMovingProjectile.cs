using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public abstract class ConstantMovingProjectile : Projectile
    {
        public readonly Bindable<float> SpeedMultiplierBindable = new Bindable<float>(1);

        public float SpeedMultiplier
        {
            get => SpeedMultiplierBindable.Value;
            set => SpeedMultiplierBindable.Value = value;
        }
    }
}
