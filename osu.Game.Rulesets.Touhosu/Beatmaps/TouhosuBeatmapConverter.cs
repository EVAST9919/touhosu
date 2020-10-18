using osu.Game.Beatmaps;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Touhosu.Objects;
using osu.Game.Rulesets.Touhosu.Extensions;
using osuTK;
using osu.Game.Rulesets.Touhosu.UI;
using osu.Game.Beatmaps.ControlPoints;
using System.Threading;
using osu.Game.Audio;
using System;

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

        protected override IEnumerable<TouhosuHitObject> ConvertHitObject(HitObject obj, IBeatmap beatmap, CancellationToken token)
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
                    var objPosition = (obj as IHasPosition)?.Position ?? Vector2.Zero;
                    var difficulty = beatmap.BeatmapInfo.BaseDifficulty;

                    var controlPointInfo = beatmap.ControlPointInfo;
                    TimingControlPoint timingPoint = controlPointInfo.TimingPointAt(obj.StartTime);
                    DifficultyControlPoint difficultyPoint = controlPointInfo.DifficultyPointAt(obj.StartTime);

                    double scoringDistance = 100 * difficulty.SliderMultiplier * difficultyPoint.SpeedMultiplier;

                    var velocity = scoringDistance / timingPoint.BeatLength;
                    var tickDistance = scoringDistance / difficulty.SliderTickRate;

                    double legacyLastTickOffset = (obj as IHasLegacyLastTickOffset)?.LegacyLastTickOffset ?? 0;

                    double spanDuration = curve.Duration / curve.SpanCount();

                    foreach (var e in SliderEventGenerator.Generate(obj.StartTime, spanDuration, velocity, tickDistance, curve.Path.Distance, curve.SpanCount(), legacyLastTickOffset, new CancellationToken()))
                    {
                        var originalPosition = curve.CurvePositionAt(e.PathProgress / curve.SpanCount()) + objPosition;
                        var sliderEventPosition = convertPosition(originalPosition);

                        switch (e.Type)
                        {
                            case SliderEventType.Head:
                                if (objectIndexInCurrentCombo == 0)
                                {
                                    hitObjects.Add(new ShapedExplosion
                                    {
                                        Position = sliderEventPosition,
                                        StartTime = e.Time,
                                        ProjectilesPerSide = 3,
                                        SideCount = MathExtensions.GetRandomTimedBool(obj.StartTime) ? 3 : 4,
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
                                        Position = sliderEventPosition,
                                        StartTime = e.Time,
                                        ProjectileCount = 5,
                                        Samples = obj.Samples,
                                        NewCombo = comboData?.NewCombo ?? false,
                                        ComboOffset = comboData?.ComboOffset ?? 0,
                                        IndexInBeatmap = index,
                                        AngleOffset = hitcircle_angle_offset * objectIndexInCurrentCombo
                                    });
                                }

                                break;

                            case SliderEventType.Tick:
                                if (positionIsValid(sliderEventPosition))
                                {
                                    hitObjects.Add(new StandaloneTickProjectile
                                    {
                                        StartTime = e.Time,
                                        Position = sliderEventPosition,
                                        NewCombo = comboData?.NewCombo ?? false,
                                        ComboOffset = comboData?.ComboOffset ?? 0,
                                        IndexInBeatmap = index,
                                        Samples = getTickSamples(obj.Samples)
                                    });
                                }

                                break;

                            case SliderEventType.Repeat:
                                hitObjects.Add(new CircularExplosion
                                {
                                    Position = sliderEventPosition,
                                    StartTime = e.Time,
                                    ProjectileCount = Math.Clamp((int)curve.Distance / 15, 3, 15),
                                    Samples = obj.Samples,
                                    NewCombo = comboData?.NewCombo ?? false,
                                    ComboOffset = comboData?.ComboOffset ?? 0,
                                    IndexInBeatmap = index,
                                    AngleOffset = MathExtensions.GetRandomTimedAngleOffset(e.Time)
                                });

                                break;

                            case SliderEventType.Tail:
                                hitObjects.Add(new CircularExplosion
                                {
                                    Position = sliderEventPosition,
                                    StartTime = e.Time,
                                    ProjectileCount = Math.Clamp((int)curve.Distance * curve.SpanCount() / 15, 5, 20),
                                    Samples = obj.Samples,
                                    NewCombo = comboData?.NewCombo ?? false,
                                    ComboOffset = comboData?.ComboOffset ?? 0,
                                    IndexInBeatmap = index,
                                    AngleOffset = MathExtensions.GetRandomTimedAngleOffset(e.Time)
                                });

                                break;
                        }
                    }
                    break;

                case IHasDuration endTime:
                    hitObjects.Add(new Spinner
                    {
                        Position = convertPosition((obj as IHasPosition)?.Position ?? Vector2.Zero),
                        StartTime = obj.StartTime,
                        Duration = endTime.Duration,
                        NewCombo = comboData?.NewCombo ?? false,
                        ComboOffset = comboData?.ComboOffset ?? 0,
                        IndexInBeatmap = index
                    });
                    break;

                default:
                    if (objectIndexInCurrentCombo == 0)
                    {
                        var randomBool = MathExtensions.GetRandomTimedBool(obj.StartTime);

                        hitObjects.Add(new ShapedExplosion
                        {
                            Position = convertPosition((obj as IHasPosition)?.Position ?? Vector2.Zero),
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
                            Position = convertPosition((obj as IHasPosition)?.Position ?? Vector2.Zero),
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

        private static bool positionIsValid(Vector2 position)
        {
            if (position.X > TouhosuPlayfield.PLAYFIELD_SIZE.X || position.X < 0 || position.Y < 0 || position.Y > TouhosuPlayfield.PLAYFIELD_SIZE.Y)
                return false;

            return true;
        }

        private static List<HitSampleInfo> getTickSamples(IList<HitSampleInfo> objSamples) => objSamples.Select(s => new HitSampleInfo
        {
            Bank = s.Bank,
            Name = @"slidertick",
            Volume = s.Volume
        }).ToList();

        private static Vector2 convertPosition(Vector2 original)
        {
            var newXPosition = MathExtensions.Map(original.X, 0, TouhosuPlayfield.FULL_SIZE.X, 0, TouhosuPlayfield.PLAYFIELD_SIZE.X);
            var newYPosition = original.Y * 0.5f;
            return new Vector2(newXPosition, newYPosition);
        }
    }
}
