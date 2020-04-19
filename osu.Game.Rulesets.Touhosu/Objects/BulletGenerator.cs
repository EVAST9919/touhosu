using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Touhosu.Judgements;
using osu.Game.Rulesets.Judgements;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public class BulletGenerator : TouhosuHitObject
    {
        public float CircleSize { get; set; } = 1;

        public override Judgement CreateJudgement() => new TouhosuJudgement();

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);
            CircleSize = difficulty.CircleSize;
        }
    }
}
