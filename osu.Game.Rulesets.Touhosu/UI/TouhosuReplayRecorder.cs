using System.Collections.Generic;
using osu.Game.Replays;
using osu.Game.Rulesets.Touhosu.Replays;
using osu.Game.Rulesets.Replays;
using osu.Game.Rulesets.UI;
using osuTK;

namespace osu.Game.Rulesets.Touhosu.UI
{
    public class TouhosuReplayRecorder : ReplayRecorder<TouhosuAction>
    {
        private readonly TouhosuPlayfield playfield;

        public TouhosuReplayRecorder(Replay target, TouhosuPlayfield playfield)
            : base(target)
        {
            this.playfield = playfield;
        }

        protected override ReplayFrame HandleFrame(Vector2 mousePosition, List<TouhosuAction> actions, ReplayFrame previousFrame)
            => new TouhosuReplayFrame(Time.Current, playfield.Player.PlayerPosition(), actions.Contains(TouhosuAction.Focus), actions.Contains(TouhosuAction.Shoot), previousFrame as TouhosuReplayFrame);
    }
}
