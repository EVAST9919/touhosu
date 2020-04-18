using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Touhosu.Judgements
{
    public class NullJudgement : Judgement
    {
        public override bool AffectsCombo => false;

        public override bool IsBonus => false;

        public override HitResult MaxResult => HitResult.Miss;

        protected override double HealthIncreaseFor(HitResult result) => 0;

        protected override int NumericResultFor(HitResult result) => 0;
    }
}
