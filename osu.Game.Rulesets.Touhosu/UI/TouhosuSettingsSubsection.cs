using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Touhosu.Configuration;

namespace osu.Game.Rulesets.Touhosu.UI
{
    public partial class TouhosuSettingsSubsection : RulesetSettingsSubsection
    {
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
                    Current = config.GetBindable<double>(TouhosuRulesetSetting.PlayfieldDim),
                    KeyboardStep = 0.01f,
                    DisplayAsPercentage = true
                }
            };
        }
    }
}
