using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Touhosu.Objects;

namespace osu.Game.Rulesets.Touhosu.Beatmaps
{
    public class TouhosuBeatmap : Beatmap<TouhosuHitObject>
    {
        public override IEnumerable<BeatmapStatistic> GetStatistics()
        {
            return new[]
            {
                new BeatmapStatistic
                {
                    Name = @"Projectile Count",
                    Content = HitObjects.Count.ToString(),
                    CreateIcon = () => new BeatmapStatisticIcon(BeatmapStatisticsIconType.Circles)
                },
            };
        }
    }
}
