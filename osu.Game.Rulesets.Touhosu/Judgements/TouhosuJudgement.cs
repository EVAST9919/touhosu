using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Touhosu.Judgements
{
    public class TouhosuJudgement : Judgement
    {
        public override HitResult MaxResult => HitResult.Perfect;

        protected override double HealthIncreaseFor(HitResult result)
        {
            if (result == HitResult.Perfect)
                return 0.0002f;

            return base.HealthIncreaseFor(result);
        }
    }
}
