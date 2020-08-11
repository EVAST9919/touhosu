using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.Touhosu.Edit.Blueprints;
using osu.Game.Rulesets.Touhosu.Objects;

namespace osu.Game.Rulesets.Touhosu.Edit
{
    public class AngeledProjectileCompositionTool : HitObjectCompositionTool
    {
        public AngeledProjectileCompositionTool()
            : base(nameof(AngeledProjectile))
        {
        }

        public override PlacementBlueprint CreatePlacementBlueprint() => new AngeledProjectilePlacementBlueprint();
    }
}
