using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Touhosu.Judgements;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public abstract class Explosion : TouhosuHitObject
    {
        public float AngleOffset { get; set; }

        public override Judgement CreateJudgement() => new NullJudgement();
    }
}
