using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Touhosu.Objects;
using osu.Game.Rulesets.Touhosu.Replays;
using osu.Game.Scoring;
using osu.Game.Users;

namespace osu.Game.Rulesets.Touhosu.Mods
{
    public class TouhosuModAutoplay : ModAutoplay<TouhosuHitObject>
    {
        public override Score CreateReplayScore(IBeatmap beatmap) => new Score
        {
            ScoreInfo = new ScoreInfo { User = new User { Username = "Reimu" } },
            Replay = new TouhosuAutoGenerator(beatmap).Generate(),
        };
    }
}
