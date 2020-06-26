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

namespace osu.Game.Rulesets.Touhosu
{
    public class TouhosuRuleset : Ruleset
    {
        public TouhosuHealthProcessor HealthProcessor;

        public TouhosuScoreProcessor ScoreProcessor;

        public override DrawableRuleset CreateDrawableRulesetWith(IBeatmap beatmap, IReadOnlyList<Mod> mods = null) => new DrawableTouhosuRuleset(this, beatmap, mods);

        public override ScoreProcessor CreateScoreProcessor() => ScoreProcessor = new TouhosuScoreProcessor();

        public override HealthProcessor CreateHealthProcessor(double drainStartTime) => HealthProcessor = new TouhosuHealthProcessor();

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) => new TouhosuBeatmapConverter(beatmap, this);

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
                        new TouhosuModSuddenDeath(),
                        new MultiMod(new TouhosuModDoubleTime(), new TouhosuModNightcore()),
                        new TouhosuModHidden()
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

        public override Drawable CreateIcon() => new Sprite
        {
            Texture = new TextureStore(new TextureLoaderStore(CreateResourceStore()), false).Get("Textures/logo"),
        };

        public override DifficultyCalculator CreateDifficultyCalculator(WorkingBeatmap beatmap) => new TouhosuDifficultyCalculator(this, beatmap);
    }
}
