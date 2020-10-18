using System.Collections.Generic;
using System.Linq;
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
            var homing = HitObjects.Count(s => s is HomingProjectile);
            var projectiles = HitObjects.Count(s => s is AngeledProjectile);

            return new[]
            {
                new BeatmapStatistic
                {
                    Name = @"Explosions",
                    Content = explosions.ToString()
                },
                new BeatmapStatistic
                {
                    Name = @"Spinners",
                    Content = spinners.ToString()
                },
                new BeatmapStatistic
                {
                    Name = @"Projectiles",
                    Content = projectiles.ToString()
                },
                new BeatmapStatistic
                {
                    Name = @"Homing projectiles",
                    Content = homing.ToString()
                }
            };
        }
    }
}
