using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.Touhosu.Objects;
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
            new CircularExplosionCompositionTool(),
            new ShapedExplosionCompositionTool()
        };
    }
}
