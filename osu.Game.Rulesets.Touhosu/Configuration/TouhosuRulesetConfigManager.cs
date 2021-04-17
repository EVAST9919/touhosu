using osu.Game.Configuration;
using osu.Game.Rulesets.Configuration;

namespace osu.Game.Rulesets.Touhosu.Configuration
{
    public class TouhosuRulesetConfigManager : RulesetConfigManager<TouhosuRulesetSetting>
    {
        public TouhosuRulesetConfigManager(SettingsStore settings, RulesetInfo ruleset, int? variant = null)
            : base(settings, ruleset, variant)
        {
        }

        protected override void InitialiseDefaults()
        {
            base.InitialiseDefaults();
            SetDefault(TouhosuRulesetSetting.PlayfieldDim, 0.5, 0, 1);
        }
    }

    public enum TouhosuRulesetSetting
    {
        PlayfieldDim
    }
}
