using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Touhosu.Judgements
{
    public class TickJudgement : TouhosuJudgement
    {
        protected override double HealthIncreaseFor(HitResult result)
        {
            switch (result)
            {
                case HitResult.Miss:
                    return -0.05f;
            }

            return base.HealthIncreaseFor(result);
        }
    }
}
