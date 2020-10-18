using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Touhosu.Judgements;
using System.Threading;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public abstract class StandaloneProjectile : AngeledProjectile
    {
        public override Judgement CreateJudgement() => new IgnoreJudgement();

        protected override void CreateNestedHitObjects(CancellationToken cancellationToken)
        {
            base.CreateNestedHitObjects(cancellationToken);

            AddNested(CreateProjectile());

            AddNested(new SoundHitObject
            {
                StartTime = StartTime,
                Position = Position,
                Samples = Samples
            });
        }

        protected abstract AngeledProjectile CreateProjectile();
    }
}
