using osu.Game.Beatmaps;
using osu.Game.Online.API.Requests.Responses;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Touhosu.Objects;
using osu.Game.Rulesets.Touhosu.Replays;
using osu.Game.Scoring;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Touhosu.Mods
{
    public class TouhosuModCinema : ModCinema<TouhosuHitObject>
    {
        public override Score CreateReplayScore(IBeatmap beatmap, IReadOnlyList<Mod> mods) => new Score
        {
            ScoreInfo = new ScoreInfo
            {
                User = new APIUser { Username = "Auto" }
            },
            Replay = new TouhosuAutoGenerator(beatmap).Generate(),
        };
    }
}
