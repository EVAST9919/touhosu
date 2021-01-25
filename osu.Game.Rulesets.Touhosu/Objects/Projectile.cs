using osu.Framework.Bindables;
using osu.Game.Rulesets.Objects.Types;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public abstract class Projectile : TouhosuHitObject, IHasComboInformation
    {
        public double TimePreempt { get; set; } = 400;

        public readonly Bindable<int> IndexInBeatmapBindable = new Bindable<int>();

        public int IndexInBeatmap
        {
            get => IndexInBeatmapBindable.Value;
            set => IndexInBeatmapBindable.Value = value;
        }

        public Bindable<int> IndexInCurrentComboBindable { get; } = new Bindable<int>();

        public int IndexInCurrentCombo
        {
            get => IndexInCurrentComboBindable.Value;
            set => IndexInCurrentComboBindable.Value = value;
        }

        public Bindable<int> ComboIndexBindable { get; } = new Bindable<int>();

        public int ComboIndex
        {
            get => ComboIndexBindable.Value;
            set => ComboIndexBindable.Value = value;
        }

        public virtual bool NewCombo { get; set; }

        public Bindable<bool> LastInComboBindable { get; } = new Bindable<bool>();

        public virtual bool LastInCombo
        {
            get => LastInComboBindable.Value;
            set => LastInComboBindable.Value = value;
        }

        public int ComboOffset { get; set; }
    }
}
