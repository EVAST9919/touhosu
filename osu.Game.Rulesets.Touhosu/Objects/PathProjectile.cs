using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Touhosu.Judgements;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public class PathProjectile : Projectile
    {
        public override Judgement CreateJudgement() => new NullJudgement();

        public IHasPathWithRepeats Path { get; set; }

        public float TimeOffset { get; set; }

        public float Intensity { get; set; }
    }
}
