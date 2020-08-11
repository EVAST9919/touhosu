using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Touhosu.Objects;
using osu.Game.Screens.Edit;

namespace osu.Game.Rulesets.Touhosu.Edit.Blueprints
{
    public abstract class TouhosuSelectionBlueprint<T> : OverlaySelectionBlueprint
        where T : TouhosuHitObject
    {
        protected new T HitObject => (T)DrawableObject.HitObject;

        protected override bool AlwaysShowWhenSelected => true;

        [Resolved(CanBeNull = true)]
        private EditorBeatmap editorBeatmap { get; set; }

        protected TouhosuSelectionBlueprint(DrawableHitObject drawableObject)
            : base(drawableObject)
        {
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            base.OnMouseUp(e);
            editorBeatmap.UpdateHitObject(HitObject);
        }
    }
}
