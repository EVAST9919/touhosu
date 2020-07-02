using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Touhosu.Objects;
using osu.Game.Rulesets.Touhosu.Objects.Drawables;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using System.Collections.Generic;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.Touhosu.Replays;

namespace osu.Game.Rulesets.Touhosu.UI
{
    public class DrawableTouhosuRuleset : DrawableRuleset<TouhosuHitObject>
    {
        public DrawableTouhosuRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            : base(ruleset, beatmap, mods)
        {
        }

        protected override PassThroughInputManager CreateInputManager() => new TouhosuInputManager(Ruleset.RulesetInfo);

        protected override Playfield CreatePlayfield() => new TouhosuPlayfield((TouhosuRuleset)Ruleset);

        public override PlayfieldAdjustmentContainer CreatePlayfieldAdjustmentContainer() => new TouhosuPlayfieldAdjustmentContainer();

        protected override ReplayRecorder CreateReplayRecorder(Replay replay) => new TouhosuReplayRecorder(replay, (TouhosuPlayfield)Playfield);

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new TouhosuFramedReplayInputHandler(replay);

        public override DrawableHitObject<TouhosuHitObject> CreateDrawableRepresentation(TouhosuHitObject h)
        {
            switch (h)
            {
                case SoundHitObject sound:
                    return new DrawableSoundHitObject(sound);

                case TickBullet tick:
                    return new DrawableTickBullet(tick);

                case HomingBullet homing:
                    return new DrawableHomingBullet(homing);

                case ReverseSliderBullet rSlider:
                    return new DrawableReverseSliderBullet(rSlider);

                case MovingBullet moving:
                    return new DrawableMovingBullet(moving);

                case PathBullet path:
                    return new DrawablePathBullet(path);
            }

            return null;
        }
    }
}
