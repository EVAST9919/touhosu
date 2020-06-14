using osu.Game.Audio;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Touhosu.Objects;
using osu.Game.Rulesets.Touhosu.UI;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osuTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace osu.Game.Rulesets.Touhosu.Extensions
{
    public static class BulletsExtensions
    {
        private const int bullets_per_hitcircle = 4;

        private const int bullets_per_slider_reverse = 5;

        private const float slider_angle_per_span = 2f;
        private const int max_visuals_per_slider_span = 150;

        private const int bullets_per_spinner_span = 20;
        private const float spinner_span_delay = 250f;
        private const float spinner_angle_per_span = 8f;

        public static List<TouhosuHitObject> ConvertSlider(HitObject obj, IBeatmap beatmap, IHasPathWithRepeats curve, int index)
        {
            List<TouhosuHitObject> hitObjects = new List<TouhosuHitObject>();

            var objPosition = (obj as IHasPosition)?.Position ?? Vector2.Zero;
            var comboData = obj as IHasCombo;
            var difficulty = beatmap.BeatmapInfo.BaseDifficulty;

            var controlPointInfo = beatmap.ControlPointInfo;
            TimingControlPoint timingPoint = controlPointInfo.TimingPointAt(obj.StartTime);
            DifficultyControlPoint difficultyPoint = controlPointInfo.DifficultyPointAt(obj.StartTime);

            double scoringDistance = 100 * difficulty.SliderMultiplier * difficultyPoint.SpeedMultiplier;

            var velocity = scoringDistance / timingPoint.BeatLength;
            var tickDistance = scoringDistance / difficulty.SliderTickRate;

            double spanDuration = curve.Duration / (curve.RepeatCount + 1);
            double legacyLastTickOffset = (obj as IHasLegacyLastTickOffset)?.LegacyLastTickOffset ?? 0;

            foreach (var e in SliderEventGenerator.Generate(obj.StartTime, spanDuration, velocity, tickDistance, curve.Path.Distance, curve.RepeatCount + 1, legacyLastTickOffset, new CancellationToken()))
            {
                Vector2 sliderEventPosition;
                var isRepeatSpam = false;

                // Don't take into account very small sliders. There's a chance that they will contain reverse spam, and offset looks ugly
                if (spanDuration < 75 && curve.RepeatCount > 0)
                {
                    sliderEventPosition = objPosition * new Vector2(TouhosuPlayfield.X_SCALE_MULTIPLIER, 0.5f);
                    isRepeatSpam = true;
                }
                else
                    sliderEventPosition = (curve.CurvePositionAt(e.PathProgress / (curve.RepeatCount + 1)) + objPosition) * new Vector2(TouhosuPlayfield.X_SCALE_MULTIPLIER, 0.5f);

                switch (e.Type)
                {
                    case SliderEventType.Head:

                        hitObjects.Add(new SoundHitObject
                        {
                            StartTime = obj.StartTime,
                            Samples = obj.Samples
                        });

                        break;

                    case SliderEventType.Tick:

                        if (positionIsValid(sliderEventPosition))
                        {
                            hitObjects.Add(new TickBullet
                            {
                                Angle = 180,
                                StartTime = e.Time,
                                Position = sliderEventPosition,
                                NewCombo = comboData?.NewCombo ?? false,
                                ComboOffset = comboData?.ComboOffset ?? 0,
                                IndexInBeatmap = index
                            });
                        }

                        hitObjects.Add(new SoundHitObject
                        {
                            StartTime = e.Time,
                            Samples = getTickSamples(obj.Samples)
                        });
                        break;

                    case SliderEventType.Repeat:

                        if (positionIsValid(sliderEventPosition))
                        {
                            hitObjects.AddRange(generateExplosion(
                                obj.StartTime + (e.SpanIndex + 1) * spanDuration,
                                bullets_per_slider_reverse,
                                sliderEventPosition,
                                comboData,
                                index,
                                slider_angle_per_span * e.SpanIndex));
                        }

                        hitObjects.Add(new SoundHitObject
                        {
                            StartTime = e.Time,
                            Samples = obj.Samples
                        });
                        break;

                    case SliderEventType.Tail:

                        if (positionIsValid(sliderEventPosition))
                        {
                            hitObjects.AddRange(generateExplosion(
                                e.Time,
                                Math.Clamp((int)curve.Distance / 15, 5, 20),
                                sliderEventPosition,
                                comboData,
                                index,
                                isRepeatSpam ? (slider_angle_per_span * curve.RepeatCount) : 0));
                        }

                        hitObjects.Add(new SoundHitObject
                        {
                            StartTime = curve.EndTime,
                            Samples = obj.Samples
                        });
                        break;
                }
            }

            //body

            var bodyCherriesCount = Math.Min(curve.Distance * (curve.RepeatCount + 1) / 10, max_visuals_per_slider_span * (curve.RepeatCount + 1));

            for (int i = 0; i < bodyCherriesCount; i++)
            {
                var progress = (float)i / bodyCherriesCount;
                var position = (curve.CurvePositionAt(progress) + objPosition) * new Vector2(TouhosuPlayfield.X_SCALE_MULTIPLIER, 0.5f);

                if (positionIsValid(position))
                {
                    hitObjects.Add(new SliderPartBullet
                    {
                        StartTime = obj.StartTime + curve.Duration * progress,
                        Position = position,
                        NewCombo = comboData?.NewCombo ?? false,
                        ComboOffset = comboData?.ComboOffset ?? 0,
                        IndexInBeatmap = index
                    });
                }
            }

            return hitObjects;
        }

        public static List<TouhosuHitObject> ConvertHitCircle(HitObject obj, int index)
        {
            List<TouhosuHitObject> hitObjects = new List<TouhosuHitObject>();

            var objPosition = (obj as IHasPosition)?.Position ?? Vector2.Zero;
            var comboData = obj as IHasCombo;

            hitObjects.AddRange(generateExplosion(
                obj.StartTime,
                bullets_per_hitcircle,
                objPosition * new Vector2(TouhosuPlayfield.X_SCALE_MULTIPLIER, 0.5f),
                comboData,
                index,
                0,
                120));

            hitObjects.Add(new SoundHitObject
            {
                StartTime = obj.StartTime,
                Samples = obj.Samples
            });

            return hitObjects;
        }

        public static List<TouhosuHitObject> ConvertSpinner(HitObject obj, IHasDuration endTime, int index)
        {
            List<TouhosuHitObject> hitObjects = new List<TouhosuHitObject>();

            var objPosition = (obj as IHasPosition)?.Position ?? Vector2.Zero;
            var comboData = obj as IHasCombo;

            var spansPerSpinner = endTime.Duration / spinner_span_delay;

            for (int i = 0; i < spansPerSpinner; i++)
            {
                hitObjects.AddRange(generateExplosion(
                    obj.StartTime + i * spinner_span_delay,
                    bullets_per_spinner_span,
                    objPosition * new Vector2(TouhosuPlayfield.X_SCALE_MULTIPLIER, 0.5f),
                    comboData,
                    index,
                    i * spinner_angle_per_span));
            }

            return hitObjects;
        }

        private static IEnumerable<MovingBullet> generateExplosion(double startTime, int bulletCount, Vector2 position, IHasCombo comboData, int index, float angleOffset = 0, float angleRange = 360f)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                yield return new MovingBullet
                {
                    Angle = MathExtensions.BulletDistribution(bulletCount, angleRange, i, angleOffset),
                    StartTime = startTime,
                    Position = position,
                    NewCombo = comboData?.NewCombo ?? false,
                    ComboOffset = comboData?.ComboOffset ?? 0,
                    IndexInBeatmap = index
                };
            }
        }

        private static List<HitSampleInfo> getTickSamples(IList<HitSampleInfo> objSamples) => objSamples.Select(s => new HitSampleInfo
        {
            Bank = s.Bank,
            Name = @"slidertick",
            Volume = s.Volume
        }).ToList();

        private static bool positionIsValid(Vector2 position)
        {
            if (position.X > TouhosuPlayfield.ACTUAL_SIZE.X || position.X < 0 || position.Y < 0 || position.Y > TouhosuPlayfield.ACTUAL_SIZE.Y)
                return false;

            return true;
        }
    }
}
