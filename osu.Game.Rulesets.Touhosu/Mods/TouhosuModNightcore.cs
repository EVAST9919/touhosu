using osu.Framework.Bindables;
using osu.Game.Configuration;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Touhosu.Objects;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Touhosu.UI;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Touhosu.Mods
{
    public class TouhosuModNightcore : ModNightcore<TouhosuHitObject>
    {
        public override double ScoreMultiplier => 1.1f;

        [SettingSource("Preserve player speed", "Turn on to make player speed be unaffected by the song speed.", SettingControlType = typeof(SettingsCheckbox))]
        public BindableBool PreserveSpeed { get; } = new BindableBool();

        public new void ApplyToDrawableRuleset(DrawableRuleset<TouhosuHitObject> drawableRuleset)
        {
            base.ApplyToDrawableRuleset(drawableRuleset);

            if (!PreserveSpeed.Value)
                return;

            var playfiled = (TouhosuPlayfield)drawableRuleset.Playfield;
            playfiled.SpeedMultiplier = 1.0 / SpeedChange.Value;
        }
    }
}
