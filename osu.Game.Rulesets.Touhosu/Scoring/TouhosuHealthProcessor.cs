using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Touhosu.Scoring
{
    public class TouhosuHealthProcessor : HealthProcessor
    {
        public bool NoRegen { get; set; }

        public double LossMultiplier { get; set; } = 1.0;

        protected override double GetHealthIncreaseFor(JudgementResult result)
        {
            double adjustment = 1.0;

            switch (result.Type)
            {
                case HitResult.Perfect:
                    if (NoRegen)
                        adjustment = 0;
                    break;

                case HitResult.Miss:
                    adjustment *= LossMultiplier;
                    break;
            }

            return base.GetHealthIncreaseFor(result) * adjustment;
        }
    }
}
