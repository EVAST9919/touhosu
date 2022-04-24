using osu.Game.Beatmaps;
using osu.Game.Replays;
using osu.Game.Rulesets.Touhosu.Beatmaps;
using osu.Game.Rulesets.Replays;

namespace osu.Game.Rulesets.Touhosu.Replays
{
    public class TouhosuAutoGenerator : AutoGenerator
    {
        public new TouhosuBeatmap Beatmap => (TouhosuBeatmap)base.Beatmap;

        public TouhosuAutoGenerator(IBeatmap beatmap)
            : base(beatmap)
        {
            Replay = new Replay();
        }

        protected Replay Replay;

        public override Replay Generate()
        {
            Replay.Frames.Add(new TouhosuReplayFrame(0));
            return Replay;
        }
    }
}
