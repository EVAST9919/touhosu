using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Touhosu.Objects;
using osu.Game.Screens.Edit.Compose.Components;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Touhosu.Edit
{
    public class TouhosuHitObjectComposer : HitObjectComposer<TouhosuHitObject>
    {
        public TouhosuHitObjectComposer(Ruleset ruleset)
            : base(ruleset)
        {
        }

        protected override IReadOnlyList<HitObjectCompositionTool> CompositionTools => new HitObjectCompositionTool[]
        {
            new TickCompositionTool(),
            new AngeledProjectileCompositionTool(),
            new CircularExplosionCompositionTool(),
            new ShapedExplosionCompositionTool(),
            new SpinnerCompositionTool()
        };

        protected override ComposeBlueprintContainer CreateBlueprintContainer(IEnumerable<DrawableHitObject> hitObjects)
            => new TouhosuBlueprintContainer(hitObjects);
    }
}
