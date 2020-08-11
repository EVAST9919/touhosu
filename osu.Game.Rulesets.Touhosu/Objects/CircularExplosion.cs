using osu.Game.Rulesets.Touhosu.Extensions;
using osuTK;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public class CircularExplosion : Explosion
    {
        public int ProjectileCount { get; set; }

        protected override void CreateNestedHitObjects()
        {
            base.CreateNestedHitObjects();

            for (int i = 0; i < ProjectileCount; i++)
            {
                AddNested(new AngeledProjectile
                {
                    Position = Vector2.Zero,
                    ParentPosition = Position,
                    StartTime = StartTime,
                    Angle = MathExtensions.GetSafeAngle((float)i / ProjectileCount * 360 + AngleOffset),
                    NewCombo = NewCombo,
                    ComboOffset = ComboOffset,
                    IndexInBeatmap = IndexInBeatmap
                });
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
