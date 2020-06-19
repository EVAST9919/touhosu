using osu.Game.Rulesets.Touhosu.Judgements;
using osu.Game.Rulesets.Judgements;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Beatmaps;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public class TickBullet : HomingBullet
    {
        public override Judgement CreateJudgement() => new TickJudgement();

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);
            TimePreempt = 0;
        }
    }
}
