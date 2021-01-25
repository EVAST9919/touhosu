using osu.Game.Rulesets.Touhosu.Objects.Drawables;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Touhosu.Mods
{
    public class TouhosuModHidden : ModHidden
    {
        public override double ScoreMultiplier => 1.06;

        public override void ApplyToDrawableHitObjects(IEnumerable<DrawableHitObject> drawables)
        {
            base.ApplyToDrawableHitObjects(drawables);

            foreach (var d in drawables)
            {
                if (d is DrawableAngeledProjectile c)
                {
                    c.HiddenApplied = true;
                }
            }
        }
    }
}
