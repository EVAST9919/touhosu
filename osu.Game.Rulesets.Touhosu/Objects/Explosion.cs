using osu.Game.Audio;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Touhosu.Judgements;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public abstract class Explosion : TouhosuHitObject
    {
        public float AngleOffset { get; set; }

        public new IList<HitSampleInfo> Samples { get; set; } = new List<HitSampleInfo>();

        public override Judgement CreateJudgement() => new NullJudgement();
    }
}
