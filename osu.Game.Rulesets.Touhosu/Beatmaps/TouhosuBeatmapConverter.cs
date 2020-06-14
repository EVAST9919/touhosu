using osu.Game.Beatmaps;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Touhosu.Objects;
using osu.Game.Rulesets.Touhosu.Extensions;

namespace osu.Game.Rulesets.Touhosu.Beatmaps
{
    public class TouhosuBeatmapConverter : BeatmapConverter<TouhosuHitObject>
    {
        public TouhosuBeatmapConverter(IBeatmap beatmap, Ruleset ruleset)
            : base(beatmap, ruleset)
        {
        }

        public override bool CanConvert() => Beatmap.HitObjects.All(h => h is IHasPosition);

        private int index = -1;

        protected override IEnumerable<TouhosuHitObject> ConvertHitObject(HitObject obj, IBeatmap beatmap)
        {
            var comboData = obj as IHasCombo;
            if (comboData?.NewCombo ?? false)
                index++;

            List<TouhosuHitObject> hitObjects = new List<TouhosuHitObject>();

            switch (obj)
            {
                case IHasPathWithRepeats curve:
                    hitObjects.AddRange(BulletsExtensions.ConvertSlider(obj, beatmap, curve, index));
                    break;

                case IHasDuration endTime:
                    hitObjects.AddRange(BulletsExtensions.ConvertSpinner(obj, endTime, index));
                    break;

                default:
                    hitObjects.AddRange(BulletsExtensions.ConvertHitCircle(obj, index));
                    break;
            }

            return hitObjects;
        }

        protected override Beatmap<TouhosuHitObject> CreateBeatmap() => new TouhosuBeatmap();
    }
}
