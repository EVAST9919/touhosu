using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Touhosu.Replays;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Touhosu.Mods
{
    public class TouhosuModAutoplay : ModAutoplay
    {
        public override ModReplayData CreateReplayData(IBeatmap beatmap, IReadOnlyList<Mod> mods)
            => new ModReplayData(new TouhosuAutoGenerator(beatmap).Generate(), new ModCreatedUser { Username = "Autoplay" });
    }
}
