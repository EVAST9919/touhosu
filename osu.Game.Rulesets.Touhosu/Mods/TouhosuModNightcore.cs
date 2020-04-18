using osu.Game.Rulesets.Touhosu.Objects;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.Touhosu.Mods
{
    public class TouhosuModNightcore : ModNightcore<TouhosuHitObject>
    {
        public override double ScoreMultiplier => 1.1f;
    }
}
