using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Touhosu.Objects;
using osuTK.Input;

namespace osu.Game.Rulesets.Touhosu.Edit.Blueprints
{
    public abstract class TouhosuPlacementBlueprint<T> : PlacementBlueprint
        where T : TouhosuHitObject
    {
        public new T HitObject => (T)base.HitObject;

        protected TouhosuPlacementBlueprint(T hitObject)
            : base(hitObject)
        {
            RelativeSizeAxes = Axes.None;
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (e.Button == MouseButton.Left)
            {
                EndPlacement(true);
                return true;
            }

            return base.OnMouseDown(e);
        }

        public override void UpdatePosition(SnapResult result)
        {
            base.UpdatePosition(result);
            HitObject.Position = ToLocalSpace(result.ScreenSpacePosition);
        }
    }
}
