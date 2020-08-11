using NUnit.Framework;
using osu.Game.Tests.Visual;

namespace osu.Game.Rulesets.Touhosu.Tests
{
    [TestFixture]
    public class TestSceneEditor : EditorTestScene
    {
        protected override Ruleset CreateEditorRuleset() => new TouhosuRuleset();
    }
}
