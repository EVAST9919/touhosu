using NUnit.Framework;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Touhosu.Tests
{
    [TestFixture]
    public class TestSceneTouhosuPlayer : PlayerTestScene
    {
        protected override Ruleset CreatePlayerRuleset() => new TouhosuRuleset();
    }
}
