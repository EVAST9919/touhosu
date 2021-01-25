using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Judgements;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public class InstantProjectile : Projectile
    {
        public override Judgement CreateJudgement() => new IgnoreJudgement();

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);

            TimePreempt = 800;
        }
    }
}
