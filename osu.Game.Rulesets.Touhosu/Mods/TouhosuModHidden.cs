using osu.Framework.Localisation;
using osu.Game.Rulesets.Touhosu.Objects.Drawables;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Touhosu.Mods
{
    public class TouhosuModHidden : ModHidden
    {
        public override double ScoreMultiplier => 1.06;

        public override LocalisableString Description => "Bullets will become invisible near you.";

        public override void ApplyToDrawableHitObject(DrawableHitObject dho)
        {
            base.ApplyToDrawableHitObject(dho);

            if (dho is DrawableAngeledProjectile p)
                p.HiddenApplied = true;
        }

        protected override void ApplyIncreasedVisibilityState(DrawableHitObject hitObject, ArmedState state)
        {
        }

        protected override void ApplyNormalVisibilityState(DrawableHitObject hitObject, ArmedState state)
        {
        }
    }
}
