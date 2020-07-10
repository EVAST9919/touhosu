using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Replays.Legacy;
using osu.Game.Rulesets.Replays;
using osu.Game.Rulesets.Replays.Types;
using osu.Game.Rulesets.Touhosu.UI;
using osuTK;

namespace osu.Game.Rulesets.Touhosu.Replays
{
    public class TouhosuReplayFrame : ReplayFrame, IConvertibleReplayFrame
    {
        public List<TouhosuAction> Actions = new List<TouhosuAction>();
        public Vector2 Position;
        public bool Focused;
        public bool Shooting;

        public TouhosuReplayFrame()
        {
        }

        public TouhosuReplayFrame(double time, Vector2? position = null, bool focused = false, bool shooting = false, TouhosuReplayFrame lastFrame = null)
            : base(time)
        {
            Position = position ?? new Vector2(TouhosuPlayfield.PLAYFIELD_SIZE.X / 2f, TouhosuPlayfield.PLAYFIELD_SIZE.Y + 100);
            Focused = focused;
            Shooting = shooting;

            if (Focused)
                Actions.Add(TouhosuAction.Focus);

            if (Shooting)
                Actions.Add(TouhosuAction.Shoot);

            if (lastFrame != null)
            {
                if (Position.X > lastFrame.Position.X)
                    lastFrame.Actions.Add(TouhosuAction.MoveRight);
                else if (Position.X < lastFrame.Position.X)
                    lastFrame.Actions.Add(TouhosuAction.MoveLeft);

                if (Position.Y > lastFrame.Position.Y)
                    lastFrame.Actions.Add(TouhosuAction.MoveDown);
                else if (Position.Y < lastFrame.Position.Y)
                    lastFrame.Actions.Add(TouhosuAction.MoveUp);
            }
        }

        public void FromLegacy(LegacyReplayFrame currentFrame, IBeatmap beatmap, ReplayFrame lastFrame = null)
        {
            Position = currentFrame.Position;
            Focused = currentFrame.ButtonState == ReplayButtonState.Left1;
            Shooting = currentFrame.ButtonState == ReplayButtonState.Left2;

            if (Focused)
                Actions.Add(TouhosuAction.Focus);

            if (Shooting)
                Actions.Add(TouhosuAction.Shoot);

            if (lastFrame is TouhosuReplayFrame lastTouhosuFrame)
            {
                if (Position.X > lastTouhosuFrame.Position.X)
                    lastTouhosuFrame.Actions.Add(TouhosuAction.MoveRight);
                else if (Position.X < lastTouhosuFrame.Position.X)
                    lastTouhosuFrame.Actions.Add(TouhosuAction.MoveLeft);

                if (Position.Y > lastTouhosuFrame.Position.Y)
                    lastTouhosuFrame.Actions.Add(TouhosuAction.MoveDown);
                else if (Position.Y < lastTouhosuFrame.Position.Y)
                    lastTouhosuFrame.Actions.Add(TouhosuAction.MoveUp);
            }
        }

        public LegacyReplayFrame ToLegacy(IBeatmap beatmap)
        {
            ReplayButtonState state = ReplayButtonState.None;

            if (Actions.Contains(TouhosuAction.Focus)) state |= ReplayButtonState.Left1;
            if (Actions.Contains(TouhosuAction.Shoot)) state |= ReplayButtonState.Left2;

            return new LegacyReplayFrame(Time, Position.X, Position.Y, state);
        }
    }
}
