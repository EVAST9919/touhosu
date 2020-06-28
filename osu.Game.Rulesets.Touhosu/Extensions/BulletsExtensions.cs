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
        private const int bullets_per_hitcircle = 5;
        private const int hitcircle_angle_offset = 5;

        private const int bullets_per_spinner_span = 20;
        private const float spinner_span_delay = 250f;
        private const float spinner_angle_per_span = 8f;

        public static List<TouhosuHitObject> ConvertSlider(HitObject obj, IBeatmap beatmap, IHasPathWithRepeats curve, int index)
        {
            double spanDuration = curve.Duration / (curve.RepeatCount + 1);
            bool isBuzz = spanDuration < 85 && curve.RepeatCount > 0;

            if (isBuzz)
                return generateBuzzSlider(obj, beatmap, curve, spanDuration, index);
            else
                return generateDefaultSlider(obj, beatmap, curve, spanDuration, index);
        }

        public static List<TouhosuHitObject> ConvertHitCircle(HitObject obj, int index, int indexInCurrentCombo)
        {
            if (indexInCurrentCombo == 0)
                return convertImpactCircle(obj, index);
            else
                return convertDefaultCircle(obj, index, indexInCurrentCombo);
        }

        private static List<TouhosuHitObject> convertDefaultCircle(HitObject obj, int index, int indexInCurrentCombo)
        {
            List<TouhosuHitObject> hitObjects = new List<TouhosuHitObject>();

            var circlePosition = (obj as IHasPosition)?.Position ?? Vector2.Zero;
            circlePosition *= new Vector2(TouhosuPlayfield.X_SCALE_MULTIPLIER, 0.5f);
            var comboData = obj as IHasCombo;

            hitObjects.AddRange(generateExplosion(
                obj.StartTime,
                bullets_per_hitcircle,
                circlePosition,
                comboData,
                index,
                hitcircle_angle_offset * indexInCurrentCombo));

            hitObjects.Add(new SoundHitObject
            {
                StartTime = obj.StartTime,
                Samples = obj.Samples,
                Position = circlePosition
            });

            return hitObjects;
        }

        private static List<TouhosuHitObject> convertImpactCircle(HitObject obj, int index)
        {
            List<TouhosuHitObject> hitObjects = new List<TouhosuHitObject>();

            var circlePosition = (obj as IHasPosition)?.Position ?? Vector2.Zero;
            circlePosition *= new Vector2(TouhosuPlayfield.X_SCALE_MULTIPLIER, 0.5f);
            var comboData = obj as IHasCombo;

            var randomBool = MathExtensions.GetRandomTimedBool(obj.StartTime);

            hitObjects.AddRange(generatePolygonExplosion(
                obj.StartTime,
                4,
                randomBool ? 3 : 4,
                circlePosition,
                comboData,
                index,
                MathExtensions.GetRandomTimedAngleOffset(obj.StartTime)));

            hitObjects.Add(new SoundHitObject
            {
                StartTime = obj.StartTime,
                Samples = obj.Samples,
                Position = circlePosition
            });

            return hitObjects;
        }

        private static List<TouhosuHitObject> generateDefaultSlider(HitObject obj, IBeatmap beatmap, IHasPathWithRepeats curve, double spanDuration, int index)
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

            double legacyLastTickOffset = (obj as IHasLegacyLastTickOffset)?.LegacyLastTickOffset ?? 0;

            foreach (var e in SliderEventGenerator.Generate(obj.StartTime, spanDuration, velocity, tickDistance, curve.Path.Distance, curve.RepeatCount + 1, legacyLastTickOffset, new CancellationToken()))
            {
                var sliderEventPosition = (curve.CurvePositionAt(e.PathProgress / (curve.RepeatCount + 1)) + objPosition) * new Vector2(TouhosuPlayfield.X_SCALE_MULTIPLIER, 0.5f);

                switch (e.Type)
                {
                    case SliderEventType.Head:

                        hitObjects.Add(new SoundHitObject
                        {
                            StartTime = obj.StartTime,
                            Samples = obj.Samples,
                            Position = sliderEventPosition
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
                            Samples = getTickSamples(obj.Samples),
                            Position = sliderEventPosition
                        });
                        break;

                    case SliderEventType.Repeat:

                        hitObjects.AddRange(generateExplosion(
                            e.Time,
                            Math.Clamp((int)curve.Distance / 15, 3, 15),
                            sliderEventPosition,
                            comboData,
                            index,
                            MathExtensions.GetRandomTimedAngleOffset(e.Time),
                            350f,
                            0));

                        hitObjects.Add(new SoundHitObject
                        {
                            StartTime = e.Time,
                            Samples = obj.Samples,
                            Position = sliderEventPosition
                        });
                        break;

                    case SliderEventType.Tail:

                        hitObjects.AddRange(generateExplosion(
                            e.Time,
                            Math.Clamp((int)curve.Distance * (curve.RepeatCount + 1) / 15, 5, 20),
                            sliderEventPosition,
                            comboData,
                            index,
                            MathExtensions.GetRandomTimedAngleOffset(e.Time),
                            360f,
                            0));

                        hitObjects.Add(new SoundHitObject
                        {
                            StartTime = curve.EndTime,
                            Samples = obj.Samples,
                            Position = sliderEventPosition
                        });
                        break;
                }
            }

            hitObjects.AddRange(generateSliderBody(obj.StartTime, objPosition, curve, index, comboData, 6, 50));

            return hitObjects;
        }

        private static List<TouhosuHitObject> generateBuzzSlider(HitObject obj, IBeatmap beatmap, IHasPathWithRepeats curve, double spanDuration, int index)
        {
            List<TouhosuHitObject> hitObjects = new List<TouhosuHitObject>();

            var objPosition = ((obj as IHasPosition)?.Position ?? Vector2.Zero) * new Vector2(TouhosuPlayfield.X_SCALE_MULTIPLIER, 0.5f);
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
                            new ReverseSliderBullet
                            {
                                Angle = angle,
                                DeltaMultiplier = delta,
                                StartTime = e.Time,
                                Position = objPosition,
                                NewCombo = comboData?.NewCombo ?? false,
                                ComboOffset = comboData?.ComboOffset ?? 0,
                                IndexInBeatmap = index
                            },
                            new ReverseSliderBullet
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
                            new ReverseSliderBullet
                            {
                                Angle = angle,
                                DeltaMultiplier = delta,
                                StartTime = e.Time,
                                Position = objPosition,
                                NewCombo = comboData?.NewCombo ?? false,
                                ComboOffset = comboData?.ComboOffset ?? 0,
                                IndexInBeatmap = index
                            },
                            new ReverseSliderBullet
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
                            new ReverseSliderBullet
                            {
                                Angle = angle,
                                DeltaMultiplier = delta,
                                StartTime = e.Time,
                                Position = objPosition,
                                NewCombo = comboData?.NewCombo ?? false,
                                ComboOffset = comboData?.ComboOffset ?? 0,
                                IndexInBeatmap = index
                            },
                            new ReverseSliderBullet
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

        private static IEnumerable<TouhosuHitObject> generateSliderBody(double startTime, Vector2 position, IHasPathWithRepeats curve, int index, IHasCombo comboData, int objectsCount, float timeOffset)
        {
            for (int i = 0; i < objectsCount; i++)
            {
                yield return new PathBullet
                {
                    StartTime = startTime,
                    TimeOffset = timeOffset * i,
                    Intensity = 1f - (float)i / (objectsCount + 1),
                    Position = position,
                    Path = curve,
                    NewCombo = comboData?.NewCombo ?? false,
                    ComboOffset = comboData?.ComboOffset ?? 0,
                    IndexInBeatmap = index
                };
            }
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

        private static IEnumerable<TouhosuHitObject> generateExplosion(double startTime, int bulletCount, Vector2 position, IHasCombo comboData, int index, float angleOffset = 0, float angleRange = 360f, double? timePreempt = null)
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
                    IndexInBeatmap = index,
                    CustomTimePreempt = timePreempt
                };
            }
        }

        private static IEnumerable<TouhosuHitObject> generatePolygonExplosion(double startTime, int bullets_per_side, int verticesCount, Vector2 position, IHasCombo comboData, int index, float angleOffset = 0)
        {
            List<TouhosuHitObject> hitObjects = new List<TouhosuHitObject>();

            for (int i = 0; i < verticesCount; i++)
                hitObjects.AddRange(generatePolygonLine(startTime, bullets_per_side, verticesCount, position, comboData, index, i * (360f / verticesCount) + angleOffset));

            return hitObjects;
        }

        private static IEnumerable<TouhosuHitObject> generatePolygonLine(double startTime, int bullets_per_side, int verticesCount, Vector2 position, IHasCombo comboData, int index, float additionalOffset = 0)
        {
            var s = 1.0;
            var side = s / (2 * Math.Sin(360.0 / (2 * verticesCount) * Math.PI / 180));
            var partDistance = s / bullets_per_side;
            var partialAngle = 180 * (verticesCount - 2) / (2 * verticesCount);

            for (int i = 0; i < bullets_per_side; i++)
            {
                var c = (float)partDistance * i;
                var length = Math.Sqrt(MathExtensions.Pow((float)side) + MathExtensions.Pow(c) - (2 * side * c * Math.Cos(partialAngle * Math.PI / 180)));
                var missingAngle = c == 0 ? 0 : Math.Acos((MathExtensions.Pow((float)side) + MathExtensions.Pow((float)length) - MathExtensions.Pow(c)) / (2 * side * length)) * 180 / Math.PI;
                var currentAngle = 180 + (90 - partialAngle) - missingAngle;

                yield return new MovingBullet
                {
                    Angle = (float)currentAngle + additionalOffset,
                    DeltaMultiplier = length / side * 1.2f,
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
