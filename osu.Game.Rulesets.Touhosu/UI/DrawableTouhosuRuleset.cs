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
                case CircularExplosion cExplosion:
                    return new DrawableCircularExplosion(cExplosion);

                case ShapedExplosion sExplosion:
                    return new DrawableShapedExplosion(sExplosion);

                    //case SoundHitObject sound:
                    //    return new DrawableSoundHitObject(sound);

                    //case TickProjectile tick:
                    //    return new DrawableTickProjectile(tick);

                    //case HomingProjectile homing:
                    //    return new DrawableHomingProjectile(homing);

                    //case BuzzSliderProjectile rSlider:
                    //    return new DrawableBuzzSliderProjectile(rSlider);

                    //case AngeledProjectile moving:
                    //    return new DrawableAngeledProjectile(moving);

                    //case PathProjectile path:
                    //    return new DrawablePathProjectile(path);
            }

            return null;
        }
    }
}
