using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.UI;
using System.ComponentModel;

namespace osu.Game.Rulesets.Touhosu
{
    public class TouhosuInputManager : RulesetInputManager<TouhosuAction>
    {
        public TouhosuInputManager(RulesetInfo ruleset)
            : base(ruleset, 0, SimultaneousBindingMode.Unique)
        {
        }
    }

    public enum TouhosuAction
    {
        [Description("Move Left")]
        MoveLeft,

        [Description("Move Right")]
        MoveRight,

        [Description("Move Up")]
        MoveUp,

        [Description("Move Down")]
        MoveDown,

        [Description("Shoot")]
        Shoot,

        [Description("Focus")]
        Focus
    }
}
