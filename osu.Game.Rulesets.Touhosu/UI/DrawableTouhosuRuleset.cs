using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Touhosu.Objects;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using System.Collections.Generic;
using osu.Game.Input.Handlers;
using osu.Game.Replays;
using osu.Game.Rulesets.Touhosu.Replays;
using osu.Game.Rulesets.Touhosu.Scoring;
using osu.Game.Scoring;

namespace osu.Game.Rulesets.Touhosu.UI
{
    public class DrawableTouhosuRuleset : DrawableRuleset<TouhosuHitObject>
    {
        public TouhosuHealthProcessor HealthProcessor
        {
            set
            {
                if (Playfield is TouhosuPlayfield p)
                    p.ApplyHealthProcessor(value);
            }
        }

        public DrawableTouhosuRuleset(Ruleset ruleset, IBeatmap beatmap, IReadOnlyList<Mod> mods = null)
            : base(ruleset, beatmap, mods)
        {
        }

        protected override PassThroughInputManager CreateInputManager() => new TouhosuInputManager(Ruleset.RulesetInfo);

        protected override Playfield CreatePlayfield() => new TouhosuPlayfield();

        public override PlayfieldAdjustmentContainer CreatePlayfieldAdjustmentContainer() => new TouhosuPlayfieldAdjustmentContainer();

        protected override ReplayRecorder CreateReplayRecorder(Score score) => new TouhosuReplayRecorder(score, (TouhosuPlayfield)Playfield);

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new TouhosuFramedReplayInputHandler(replay);

        public override DrawableHitObject<TouhosuHitObject> CreateDrawableRepresentation(TouhosuHitObject h) => null;
    }
}
