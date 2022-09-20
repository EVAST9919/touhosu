using osu.Framework.Bindables;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Touhosu.Judgements;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public class AngeledProjectile : ConstantMovingProjectile
    {
        public readonly Bindable<float> AngleBindable = new Bindable<float>();

        public float Angle
        {
            get => AngleBindable.Value;
            set => AngleBindable.Value = value;
        }

        public override Judgement CreateJudgement() => new TouhosuJudgement();
    }
}
