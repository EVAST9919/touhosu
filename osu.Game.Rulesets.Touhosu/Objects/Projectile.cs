using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Touhosu.Extensions;
using osuTK;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public abstract class Projectile : TouhosuHitObject
    {
        public float SizeAdjustValue { get; protected set; } = 1;

        public bool ExpireOnWallHit { get; protected set; } = true;

        public bool AffectPlayer { get; protected set; } = true;

        public Vector2 ParentPosition { get; set; } = Vector2.Zero;

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);
            SizeAdjustValue = MathExtensions.Map(difficulty.CircleSize, 0, 10, 0.2f, 1);
        }
    }
}
