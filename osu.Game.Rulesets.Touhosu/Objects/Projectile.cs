using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public abstract class Projectile : TouhosuHitObject
    {
        public float CircleSize { get; set; } = 1;

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);
            CircleSize = difficulty.CircleSize;
        }
    }
}
