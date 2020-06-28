using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Touhosu.Configuration;

namespace osu.Game.Rulesets.Touhosu.UI
{
    public class TouhosuSettingsSubsection : RulesetSettingsSubsection
    {
        protected override string Header => "Touhosu";

        public TouhosuSettingsSubsection(Ruleset ruleset)
            : base(ruleset)
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            var config = (TouhosuRulesetConfigManager)Config;

            Children = new Drawable[]
            {
                new SettingsSlider<double>
                {
                    LabelText = "Playfield dim",
                    Bindable = config.GetBindable<double>(TouhosuRulesetSetting.PlayfieldDim),
                    KeyboardStep = 0.01f,
                    DisplayAsPercentage = true
                }
            };
        }
    }
}
