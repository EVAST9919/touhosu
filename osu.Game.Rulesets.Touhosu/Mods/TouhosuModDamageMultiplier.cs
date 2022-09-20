using System;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Configuration;
using osu.Game.Graphics;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Touhosu.Scoring;

namespace osu.Game.Rulesets.Touhosu.Mods
{
    public class TouhosuModDamageMultiplier : Mod, IApplicableToHealthProcessor
    {
        public override double ScoreMultiplier => 1.0;

        public override string Name => "Damage Multiplier";
        public override string Acronym => "DM";
        public override IconUsage? Icon => OsuIcon.ModHardRock;
        public override ModType Type => ModType.DifficultyIncrease;
        public override LocalisableString Description => "Bullet hits hurt more!";
        public override Type[] IncompatibleMods => new[] { typeof(ModEasy) };

        public void ApplyToHealthProcessor(HealthProcessor healthProcessor)
        {
            ((TouhosuHealthProcessor)healthProcessor).LossMultiplier = DamageMultiplier.Value;
        }

        [SettingSource("Damage increase", "Multiplier to damage taken when hitting a projectile")]
        public BindableNumber<double> DamageMultiplier { get; } = new BindableDouble
        {
            MinValue = 1.1,
            MaxValue = 5,
            Default = 3,
            Value = 3,
            Precision = 0.1
        };
    }
}
