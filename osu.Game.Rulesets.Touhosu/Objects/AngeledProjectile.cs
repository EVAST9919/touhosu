using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Touhosu.Judgements;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Touhosu.Extensions;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public class AngeledProjectile : Projectile
    {
        public float Speed { get; private set; }

        public float Angle { get; set; } = 180;

        public double DeltaMultiplier { get; set; } = 1;

        public double? CustomTimePreempt { get; set; }

        public override Judgement CreateJudgement() => new TouhosuJudgement();

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);
            Speed = (float)(MathExtensions.Map((float)controlPointInfo.DifficultyPointAt(StartTime).SpeedMultiplier, 0, 3, 0.4f, 0.5f) * DeltaMultiplier / 2.5f);

            if (CustomTimePreempt.HasValue)
                TimePreempt = CustomTimePreempt.Value;
        }
    }
}
