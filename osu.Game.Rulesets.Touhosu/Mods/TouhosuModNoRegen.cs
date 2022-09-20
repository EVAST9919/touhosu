using System;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Touhosu.Scoring;

namespace osu.Game.Rulesets.Touhosu.Mods
{
    public class TouhosuModNoRegen : Mod, IApplicableToHealthProcessor
    {
        public override double ScoreMultiplier => 1.0;
        public override string Name => "No Regen";
        public override string Acronym => "NR";
        public override IconUsage? Icon => OsuIcon.HeartBreak;
        public override ModType Type => ModType.DifficultyIncrease;
        public override LocalisableString Description => "No health regeneration.";
        public override Type[] IncompatibleMods => new[] { typeof(ModEasy) };

        public void ApplyToHealthProcessor(HealthProcessor healthProcessor)
        {
            ((TouhosuHealthProcessor)healthProcessor).NoRegen = true;
        }
    }
}
