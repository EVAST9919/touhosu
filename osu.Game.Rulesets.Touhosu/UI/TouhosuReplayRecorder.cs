using System.Collections.Generic;
using osu.Game.Rulesets.Touhosu.Replays;
using osu.Game.Rulesets.Replays;
using osu.Game.Rulesets.UI;
using osuTK;
using osu.Game.Scoring;

namespace osu.Game.Rulesets.Touhosu.UI
{
    public class TouhosuReplayRecorder : ReplayRecorder<TouhosuAction>
    {
        private readonly TouhosuPlayfield playfield;

        public TouhosuReplayRecorder(Score score, TouhosuPlayfield playfield)
            : base(score)
        {
            this.playfield = playfield;
        }

        protected override ReplayFrame HandleFrame(Vector2 mousePosition, List<TouhosuAction> actions, ReplayFrame previousFrame)
            => new TouhosuReplayFrame(Time.Current, playfield.Player.PlayerPosition(), actions.Contains(TouhosuAction.Focus), actions.Contains(TouhosuAction.Shoot), previousFrame as TouhosuReplayFrame);
    }
}
