using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Touhosu.Judgements;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public abstract class StandaloneProjectile : AngeledProjectile
    {
        public override Judgement CreateJudgement() => new NullJudgement();

        protected override void CreateNestedHitObjects()
        {
            base.CreateNestedHitObjects();

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
