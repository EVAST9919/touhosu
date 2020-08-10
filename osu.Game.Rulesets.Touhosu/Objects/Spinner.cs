﻿using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Touhosu.Extensions;
using osu.Game.Rulesets.Touhosu.Judgements;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public class Spinner : TouhosuHitObject, IHasDuration
    {
        private const int time_between_spawn = 13;

        public double EndTime { get; private set; }

        private double duration;

        public double Duration
        {
            get => duration;
            set
            {
                duration = value;
                EndTime = StartTime + value;
            }
        }

        public override Judgement CreateJudgement() => new NullJudgement();

        protected override void CreateNestedHitObjects()
        {
            base.CreateNestedHitObjects();

            for (double i = 0; i < Duration; i += time_between_spawn)
            {
                AddNested(new AngeledProjectile
                {
                    Position = Position,
                    StartTime = StartTime + i,
                    Angle = MathExtensions.GetSafeAngle((float)i),
                    NewCombo = NewCombo,
                    ComboOffset = ComboOffset,
                    IndexInBeatmap = IndexInBeatmap,
                    CustomTimePreempt = 0
                });
            }
        }
    }
}
