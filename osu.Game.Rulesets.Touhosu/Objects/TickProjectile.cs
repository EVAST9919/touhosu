using osu.Game.Rulesets.Touhosu.Judgements;
using osu.Game.Rulesets.Judgements;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public class TickProjectile : HomingProjectile
    {
        public override Judgement CreateJudgement() => new TickJudgement();
    }
}
