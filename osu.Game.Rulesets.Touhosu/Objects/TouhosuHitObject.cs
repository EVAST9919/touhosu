using osu.Framework.Bindables;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Scoring;
using osuTK;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public class TouhosuHitObject : HitObject, IHasPosition
    {
        public readonly Bindable<Vector2> PositionBindable = new Bindable<Vector2>();

        public virtual Vector2 Position
        {
            get => PositionBindable.Value;
            set => PositionBindable.Value = value;
        }

        public float X => Position.X;
        public float Y => Position.Y;

        protected override HitWindows CreateHitWindows() => HitWindows.Empty;
    }
}
