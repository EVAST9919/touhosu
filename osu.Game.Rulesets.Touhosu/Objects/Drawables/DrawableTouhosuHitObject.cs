using JetBrains.Annotations;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public abstract class DrawableTouhosuHitObject<T> : DrawableHitObject<TouhosuHitObject>
        where T : TouhosuHitObject
    {
        protected new T HitObject => (T)base.HitObject;

        protected DrawableTouhosuHitObject([CanBeNull] T h = null)
            : base(h)
        {
        }
    }
}
