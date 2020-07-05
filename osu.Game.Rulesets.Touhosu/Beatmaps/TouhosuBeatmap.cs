using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Touhosu.Objects;

namespace osu.Game.Rulesets.Touhosu.Beatmaps
{
    public class TouhosuBeatmap : Beatmap<TouhosuHitObject>
    {
        public override IEnumerable<BeatmapStatistic> GetStatistics()
        {
            var totalCount = HitObjects.Count();
            var hitCount = HitObjects.Count(s => s is AngeledProjectile);

            return new[]
            {
                new BeatmapStatistic
                {
                    Name = @"Projectiles",
                    Content = hitCount.ToString(),
                    Icon = FontAwesome.Regular.Circle
                }
            };
        }
    }
}
