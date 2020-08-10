using osu.Game.Beatmaps;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Touhosu.Objects;
using osu.Game.Rulesets.Touhosu.Extensions;
using osuTK;
using osu.Game.Rulesets.Touhosu.UI;

namespace osu.Game.Rulesets.Touhosu.Beatmaps
{
    public class TouhosuBeatmapConverter : BeatmapConverter<TouhosuHitObject>
    {
        private const int hitcircle_angle_offset = 5;

        public TouhosuBeatmapConverter(IBeatmap beatmap, Ruleset ruleset)
            : base(beatmap, ruleset)
        {
        }

        public override bool CanConvert() => Beatmap.HitObjects.All(h => h is IHasPosition);

        private int index = -1;
        private int objectIndexInCurrentCombo = 0;

        protected override IEnumerable<TouhosuHitObject> ConvertHitObject(HitObject obj, IBeatmap beatmap)
        {
            var comboData = obj as IHasCombo;
            if (comboData?.NewCombo ?? false)
            {
                objectIndexInCurrentCombo = 0;
                index++;
            }

            List<TouhosuHitObject> hitObjects = new List<TouhosuHitObject>();

            switch (obj)
            {
                case IHasPathWithRepeats curve:
                    //hitObjects.AddRange(ProjectileExtensions.ConvertSlider(obj, beatmap, curve, index));
                    break;

                case IHasDuration endTime:
                    //hitObjects.AddRange(ProjectileExtensions.ConvertSpinner(obj, endTime, index));
                    break;

                default:

                    if (objectIndexInCurrentCombo == 0)
                    {
                        var randomBool = MathExtensions.GetRandomTimedBool(obj.StartTime);

                        hitObjects.Add(new ShapedExplosion
                        {
                            Position = ((obj as IHasPosition)?.Position ?? Vector2.Zero) * new Vector2(TouhosuPlayfield.X_SCALE_MULTIPLIER, 0.5f),
                            StartTime = obj.StartTime,
                            ProjectilesPerSide = 3,
                            SideCount = randomBool ? 3 : 4,
                            Samples = obj.Samples,
                            NewCombo = comboData?.NewCombo ?? false,
                            ComboOffset = comboData?.ComboOffset ?? 0,
                            IndexInBeatmap = index,
                            AngleOffset = MathExtensions.GetRandomTimedAngleOffset(obj.StartTime)
                        });
                    }
                    else
                    {
                        hitObjects.Add(new CircularExplosion
                        {
                            Position = ((obj as IHasPosition)?.Position ?? Vector2.Zero) * new Vector2(TouhosuPlayfield.X_SCALE_MULTIPLIER, 0.5f),
                            StartTime = obj.StartTime,
                            ProjectileCount = 5,
                            Samples = obj.Samples,
                            NewCombo = comboData?.NewCombo ?? false,
                            ComboOffset = comboData?.ComboOffset ?? 0,
                            IndexInBeatmap = index,
                            AngleOffset = hitcircle_angle_offset * objectIndexInCurrentCombo
                        });
                    }
                    break;
            }

            objectIndexInCurrentCombo++;

            return hitObjects;
        }

        protected override Beatmap<TouhosuHitObject> CreateBeatmap() => new TouhosuBeatmap();
    }
}
