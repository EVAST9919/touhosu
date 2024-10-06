using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using System;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.Touhosu.Scoring;
using osu.Game.Rulesets.Touhosu.Difficulty;
using osu.Game.Rulesets.Touhosu.Beatmaps;
using osu.Game.Rulesets.Touhosu.Mods;
using osu.Game.Rulesets.Touhosu.UI;
using osu.Game.Rulesets.Configuration;
using osu.Game.Configuration;
using osu.Game.Rulesets.Touhosu.Configuration;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Replays.Types;
using osu.Game.Rulesets.Touhosu.Replays;
using osu.Framework.Allocation;
using osu.Framework.Platform;

namespace osu.Game.Rulesets.Touhosu
{
    public partial class TouhosuRuleset : Ruleset
    {
        private DrawableTouhosuRuleset ruleset;

        public override IRulesetConfigManager CreateConfig(SettingsStore settings) => new TouhosuRulesetConfigManager(settings, RulesetInfo);

        public override RulesetSettingsSubsection CreateSettings() => new TouhosuSettingsSubsection(this);

        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod> mods = null) => ruleset = new DrawableTouhosuRuleset(this, beatmap, mods);

        public override HealthProcessor CreateHealthProcessor(double drainStartTime)
        {
            var hp = new TouhosuHealthProcessor();
            ruleset.HealthProcessor = hp;
            return hp;
        }

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) => new TouhosuBeatmapConverter(beatmap, this);

        public override IBeatmapProcessor CreateBeatmapProcessor(IBeatmap beatmap) => new BeatmapProcessor(beatmap);

        public override IConvertibleReplayFrame CreateConvertibleReplayFrame() => new TouhosuReplayFrame();

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0) => new[]
        {
            new KeyBinding(InputKey.Left, TouhosuAction.MoveLeft),
            new KeyBinding(InputKey.Right, TouhosuAction.MoveRight),
            new KeyBinding(InputKey.Up, TouhosuAction.MoveUp),
            new KeyBinding(InputKey.Down, TouhosuAction.MoveDown),
            new KeyBinding(InputKey.Z, TouhosuAction.Shoot),
            new KeyBinding(InputKey.Shift, TouhosuAction.Focus),
        };

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            switch (type)
            {
                case ModType.DifficultyReduction:
                    return new Mod[]
                    {
                        new TouhosuModEasy(),
                        new TouhosuModNoFail(),
                        new MultiMod(new TouhosuModHalfTime(), new TouhosuModDaycore())
                    };

                case ModType.DifficultyIncrease:
                    return new Mod[]
                    {
                        new TouhosuModDamageMultiplier(),
                        new TouhosuModNoRegen(),
                        new TouhosuModSuddenDeath(),
                        new MultiMod(new TouhosuModDoubleTime(), new TouhosuModNightcore()),
                        new TouhosuModHidden()
                    };

                case ModType.Automation:
                    return new Mod[]
                    {
                        new TouhosuModAutoplay()
                    };

                case ModType.Fun:
                    return new Mod[]
                    {
                        new MultiMod(new ModWindUp(), new ModWindDown()),
                    };

                default:
                    return Array.Empty<Mod>();
            }
        }

        public override string Description => "Touhosu";

        public override string ShortName => "Touhosu";

        public override string PlayingVerb => "Avoiding bullets";

        public override Drawable CreateIcon() => new TouhosuIcon(this);

        protected override IEnumerable<HitResult> GetValidHitResults() => new[]
        {
            HitResult.Perfect
        };

        public override DifficultyCalculator CreateDifficultyCalculator(IWorkingBeatmap beatmap) => new TouhosuDifficultyCalculator(RulesetInfo, beatmap);

        private partial class TouhosuIcon : Sprite
        {
            private readonly TouhosuRuleset ruleset;

            public TouhosuIcon(TouhosuRuleset ruleset)
            {
                this.ruleset = ruleset;
            }

            [BackgroundDependencyLoader]
            private void load(GameHost host)
            {
                Texture = new TextureStore(host.Renderer, new TextureLoaderStore(ruleset.CreateResourceStore()), false).Get("Textures/logo");
            }
        }
    }
}
