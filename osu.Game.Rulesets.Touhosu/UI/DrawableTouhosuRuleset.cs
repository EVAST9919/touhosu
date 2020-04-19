using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Touhosu.Objects;
using osu.Game.Rulesets.Touhosu.Objects.Drawables;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using System.Collections.Generic;

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

        public override DrawableHitObject<TouhosuHitObject> CreateDrawableRepresentation(TouhosuHitObject h)
        {
            switch (h)
            {
                case SoundHitObject sound:
                    return new DrawableSoundHitObject(sound);

                case SliderPartBullet sliderPart:
                    return new DrawableSliderPartBullet(sliderPart);

                case TickBullet tick:
                    return new DrawableTickBullet(tick);

                case MovingBullet moving:
                    return new DrawableMovingBullet(moving);
            }

            return null;
        }
    }
}
