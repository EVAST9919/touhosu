using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Touhosu.Extensions;
using osuTK;
using System.Threading;

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

        public override Judgement CreateJudgement() => new IgnoreJudgement();

        protected override void CreateNestedHitObjects(CancellationToken cancellationToken)
        {
            base.CreateNestedHitObjects(cancellationToken);

            for (double i = 0; i < Duration; i += time_between_spawn)
            {
                AddNested(new AngeledProjectile
                {
                    Position = Vector2.Zero,
                    ParentPosition = Position,
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
