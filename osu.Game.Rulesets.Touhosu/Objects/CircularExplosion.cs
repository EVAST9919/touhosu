using osu.Game.Audio;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Touhosu.Extensions;
using osu.Game.Rulesets.Touhosu.Judgements;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public class CircularExplosion : TouhosuHitObject
    {
        public int ProjectileCount { get; set; }

        public float AngleOffset { get; set; }

        public new IList<HitSampleInfo> Samples { get; set; }

        public override Judgement CreateJudgement() => new NullJudgement();

        protected override void CreateNestedHitObjects()
        {
            base.CreateNestedHitObjects();

            for (int i = 0; i < ProjectileCount; i++)
            {
                AddNested(new AngeledProjectile
                {
                    Position = Position,
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
