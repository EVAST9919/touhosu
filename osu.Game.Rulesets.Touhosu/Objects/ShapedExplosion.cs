using osu.Game.Rulesets.Touhosu.Extensions;
using osuTK;
using System;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public class ShapedExplosion : Explosion
    {
        public int ProjectilesPerSide { get; set; }

        public int SideCount { get; set; }

        protected override void CreateNestedHitObjects()
        {
            base.CreateNestedHitObjects();

            for (int i = 0; i < SideCount; i++)
            {
                var s = 1.0;
                var side = s / (2 * Math.Sin(360.0 / (2 * SideCount) * Math.PI / 180));
                var partDistance = s / ProjectilesPerSide;
                var partialAngle = 180 * (SideCount - 2) / (2 * SideCount);

                for (int j = 0; j < ProjectilesPerSide; j++)
                {
                    var c = (float)partDistance * j;
                    var length = Math.Sqrt(MathExtensions.Pow((float)side) + MathExtensions.Pow(c) - (2 * side * c * Math.Cos(partialAngle * Math.PI / 180)));
                    var missingAngle = c == 0 ? 0 : Math.Acos((MathExtensions.Pow((float)side) + MathExtensions.Pow((float)length) - MathExtensions.Pow(c)) / (2 * side * length)) * 180 / Math.PI;
                    var currentAngle = 180 + (90 - partialAngle) - missingAngle;

                    AddNested(new AngeledProjectile
                    {
                        Position = Vector2.Zero,
                        ParentPosition = Position,
                        StartTime = StartTime,
                        Angle = MathExtensions.GetSafeAngle((float)currentAngle + (i * (360f / SideCount) + AngleOffset)),
                        DeltaMultiplier = length / side * 1.2f,
                        NewCombo = NewCombo,
                        ComboOffset = ComboOffset,
                        IndexInBeatmap = IndexInBeatmap
                    });
                }
            }

            AddNested(new SoundHitObject
            {
                StartTime = StartTime,
                Position = Position,
                Samples = Samples
            });
        }
    }
}
