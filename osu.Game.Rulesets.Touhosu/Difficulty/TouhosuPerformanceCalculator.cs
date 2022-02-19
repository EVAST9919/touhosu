using osu.Game.Rulesets.Difficulty;
using osu.Game.Scoring;

namespace osu.Game.Rulesets.Touhosu.Difficulty
{
    public class TouhosuPerformanceCalculator : PerformanceCalculator
    {
        public TouhosuPerformanceCalculator(Ruleset ruleset, DifficultyAttributes attributes, ScoreInfo score)
            : base(ruleset, attributes, score)
        {
        }
        public override PerformanceAttributes Calculate()
        {
            double accuracyValue = 0.0;
            return new TouhosuPerformanceAttributes
            {
                Accuracy = accuracyValue
            };
        }
    }
}
