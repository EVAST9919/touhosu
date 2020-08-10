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
            var explosions = HitObjects.Count(s => s is Explosion);
            var spinners = HitObjects.Count(s => s is Spinner);

            return new[]
            {
                new BeatmapStatistic
                {
                    Name = @"Explosions",
                    Content = explosions.ToString(),
                    Icon = FontAwesome.Regular.Circle
                },
                new BeatmapStatistic
                {
                    Name = @"Spinners",
                    Content = spinners.ToString(),
                    Icon = FontAwesome.Regular.Circle
                }
            };
        }
    }
}
