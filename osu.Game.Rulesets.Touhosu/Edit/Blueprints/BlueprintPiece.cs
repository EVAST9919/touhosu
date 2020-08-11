using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Touhosu.Objects;

namespace osu.Game.Rulesets.Touhosu.Edit.Blueprints
{
    public abstract class BlueprintPiece<T> : CompositeDrawable
        where T : TouhosuHitObject
    {
        public virtual void UpdateFrom(T hitObject)
        {
            Position = hitObject.Position;
        }
    }
}
