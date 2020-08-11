using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Touhosu.Objects;
using osu.Game.Rulesets.Touhosu.UI;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osuTK;
using System;
using System.Collections.Generic;
using System.Threading;

namespace osu.Game.Rulesets.Touhosu.Extensions
{
    public static class ProjectileExtensions
    {
        public static List<TouhosuHitObject> ConvertSlider(HitObject obj, IBeatmap beatmap, IHasPathWithRepeats curve, int index)
        {
            double spanDuration = curve.Duration / (curve.RepeatCount + 1);
            bool isBuzz = spanDuration < 85 && curve.RepeatCount > 0;

            return generateBuzzSlider(obj, beatmap, curve, spanDuration, index);
        }

        private static List<TouhosuHitObject> generateBuzzSlider(HitObject obj, IBeatmap beatmap, IHasPathWithRepeats curve, double spanDuration, int index)
        {
            List<TouhosuHitObject> hitObjects = new List<TouhosuHitObject>();

            var objPosition = ((obj as IHasPosition)?.Position ?? Vector2.Zero);// * new Vector2(TouhosuPlayfield.X_SCALE_MULTIPLIER, 0.5f);
            var comboData = obj as IHasCombo;

            var difficulty = beatmap.BeatmapInfo.BaseDifficulty;

            var controlPointInfo = beatmap.ControlPointInfo;
            TimingControlPoint timingPoint = controlPointInfo.TimingPointAt(obj.StartTime);
            DifficultyControlPoint difficultyPoint = controlPointInfo.DifficultyPointAt(obj.StartTime);

            double scoringDistance = 100 * difficulty.SliderMultiplier * difficultyPoint.SpeedMultiplier;

            var velocity = scoringDistance / timingPoint.BeatLength;
            var tickDistance = scoringDistance / difficulty.SliderTickRate;

            var legacyLastTickOffset = (obj as IHasLegacyLastTickOffset)?.LegacyLastTickOffset ?? 0;

            var angleRandom = new Random((int)(obj.StartTime * 100));
            var deltaRandom = new Random((int)(curve.Duration * 100));

            foreach (var e in SliderEventGenerator.Generate(obj.StartTime, spanDuration, velocity, tickDistance, curve.Path.Distance, curve.RepeatCount + 1, legacyLastTickOffset, new CancellationToken()))
            {
                float angle, delta;

                switch (e.Type)
                {
                    case SliderEventType.Head:
                        angle = (float)(angleRandom.NextDouble() * 360f);
                        delta = MathExtensions.Map((float)deltaRandom.NextDouble(), 0, 1, 0.9f, 1.1f);

                        hitObjects.AddRange(new TouhosuHitObject[]
                        {
                            new BuzzSliderProjectile
                            {
                                Angle = angle,
                                DeltaMultiplier = delta,
                                StartTime = e.Time,
                                Position = objPosition,
                                NewCombo = comboData?.NewCombo ?? false,
                                ComboOffset = comboData?.ComboOffset ?? 0,
                                IndexInBeatmap = index
                            },
                            new BuzzSliderProjectile
                            {
                                Angle = angle + 180,
                                DeltaMultiplier = delta,
                                StartTime = e.Time,
                                Position = objPosition,
                                NewCombo = comboData?.NewCombo ?? false,
                                ComboOffset = comboData?.ComboOffset ?? 0,
                                IndexInBeatmap = index
                            },
                            new SoundHitObject
                            {
                                StartTime = e.Time,
                                Samples = obj.Samples,
                                Position = objPosition
                            }
                        });
                        break;

                    case SliderEventType.Repeat:
                        angle = (float)(angleRandom.NextDouble() * 360f);
                        delta = MathExtensions.Map((float)deltaRandom.NextDouble(), 0, 1, 0.9f, 1.1f);

                        hitObjects.AddRange(new TouhosuHitObject[]
                        {
                            new BuzzSliderProjectile
                            {
                                Angle = angle,
                                DeltaMultiplier = delta,
                                StartTime = e.Time,
                                Position = objPosition,
                                NewCombo = comboData?.NewCombo ?? false,
                                ComboOffset = comboData?.ComboOffset ?? 0,
                                IndexInBeatmap = index
                            },
                            new BuzzSliderProjectile
                            {
                                Angle = angle + 180,
                                DeltaMultiplier = delta,
                                StartTime = e.Time,
                                Position = objPosition,
                                NewCombo = comboData?.NewCombo ?? false,
                                ComboOffset = comboData?.ComboOffset ?? 0,
                                IndexInBeatmap = index
                            },
                            new SoundHitObject
                            {
                                StartTime = e.Time,
                                Samples = obj.Samples,
                                Position = objPosition
                            }
                        });
                        break;

                    case SliderEventType.Tail:
                        angle = (float)(angleRandom.NextDouble() * 360f);
                        delta = MathExtensions.Map((float)deltaRandom.NextDouble(), 0, 1, 0.9f, 1.1f);

                        hitObjects.AddRange(new TouhosuHitObject[]
                        {
                            new BuzzSliderProjectile
                            {
                                Angle = angle,
                                DeltaMultiplier = delta,
                                StartTime = e.Time,
                                Position = objPosition,
                                NewCombo = comboData?.NewCombo ?? false,
                                ComboOffset = comboData?.ComboOffset ?? 0,
                                IndexInBeatmap = index
                            },
                            new BuzzSliderProjectile
                            {
                                Angle = angle + 180,
                                DeltaMultiplier = delta,
                                StartTime = e.Time,
                                Position = objPosition,
                                NewCombo = comboData?.NewCombo ?? false,
                                ComboOffset = comboData?.ComboOffset ?? 0,
                                IndexInBeatmap = index
                            },
                            new SoundHitObject
                            {
                                StartTime = e.Time,
                                Samples = obj.Samples,
                                Position = objPosition
                            }
                        });
                        break;
                }
            }

            return hitObjects;
        }
    }
}
