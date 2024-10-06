using osu.Framework.Bindables;
using osu.Game.Configuration;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Touhosu.Objects;
using osu.Game.Rulesets.Touhosu.UI;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Touhosu.Mods
{
    public class TouhosuModHalfTime : ModHalfTime, IApplicableToDrawableRuleset<TouhosuHitObject>
    {
        public override double ScoreMultiplier => 0.3;

        [SettingSource("Preserve player speed", "Turn on to make player speed be unaffected by the song speed.", SettingControlType = typeof(SettingsCheckbox))]
        public BindableBool PreserveSpeed { get; } = new BindableBool();

        public void ApplyToDrawableRuleset(DrawableRuleset<TouhosuHitObject> drawableRuleset)
        {
            if (!PreserveSpeed.Value)
                return;

            var playfiled = (TouhosuPlayfield)drawableRuleset.Playfield;
            playfiled.SpeedMultiplier = 1.0 / SpeedChange.Value;
        }
    }
}
