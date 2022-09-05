using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Touhosu.Judgements
{
    public class TouhosuJudgement : Judgement
    {
        public double MissHealthIncrease = -0.1;
        public double PerfectHealthIncrease = 0.0002f;

        public override HitResult MaxResult => HitResult.Perfect;

        protected override double HealthIncreaseFor(HitResult result)
        {
            return result switch
            {
                HitResult.Perfect => PerfectHealthIncrease,
                HitResult.Miss => MissHealthIncrease,
                _ => base.HealthIncreaseFor(result)
            };
        }
    }
}
