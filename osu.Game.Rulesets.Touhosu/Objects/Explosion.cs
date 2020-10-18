using osu.Game.Rulesets.Judgements;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public abstract class Explosion : TouhosuHitObject
    {
        public float AngleOffset { get; set; }

        public override Judgement CreateJudgement() => new IgnoreJudgement();
    }
}
