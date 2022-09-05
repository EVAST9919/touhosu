// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Graphics;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Touhosu.Beatmaps;
using osu.Game.Rulesets.Touhosu.Objects;

namespace osu.Game.Rulesets.Touhosu.Mods
{
    public class TouhosuModDamageMultiplier : Mod, IApplicableToBeatmap
    {
        public override double ScoreMultiplier => UsesDefaultConfiguration ? 1.2 : 1;

        public override string Name => "Damage Multiplier";
        public override string Acronym => "DM";
        public override IconUsage? Icon => OsuIcon.ModHardRock;
        public override ModType Type => ModType.DifficultyIncrease;
        public override LocalisableString Description => "Bullet hits hurt more!";
        public override Type[] IncompatibleMods => new[] { typeof(ModEasy) };

        public void ApplyToBeatmap(IBeatmap beatmap)
        {
            var touhosuBeatmap = (TouhosuBeatmap)beatmap;

            if (touhosuBeatmap.HitObjects.Count == 0) return;

            var hitObjects = touhosuBeatmap.HitObjects.Select(ho =>
                {
                    if (!(ho is AngeledProjectile projectile)) return ho;

                    projectile.MissHealthIncrease *= DamageMultiplier.Value;
                    return projectile;
                }
            ).ToList();
            touhosuBeatmap.HitObjects = hitObjects;
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
