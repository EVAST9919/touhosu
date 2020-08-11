using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.Touhosu.Edit.Blueprints;
using osu.Game.Rulesets.Touhosu.Objects;

namespace osu.Game.Rulesets.Touhosu.Edit
{
    public class SpinnerCompositionTool : HitObjectCompositionTool
    {
        public SpinnerCompositionTool()
            : base(nameof(Spinner))
        {
        }

        public override PlacementBlueprint CreatePlacementBlueprint() => new SpinnerPlacementBlueprint();
    }
}
