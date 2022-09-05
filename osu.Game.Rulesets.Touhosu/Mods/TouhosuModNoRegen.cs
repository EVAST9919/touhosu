// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Localisation;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Touhosu.Beatmaps;
using osu.Game.Rulesets.Touhosu.Objects;

namespace osu.Game.Rulesets.Touhosu.Mods
{
    public class TouhosuModNoRegen : Mod, IApplicableToBeatmap
    {
        public override double ScoreMultiplier => UsesDefaultConfiguration ? 1.3 : 1;

        public override string Name => "No Regen";
        public override string Acronym => "NR";
        public override IconUsage? Icon => OsuIcon.HeartBreak;
        public override ModType Type => ModType.DifficultyIncrease;
        public override LocalisableString Description => "You cannot regenerate health.";

        public void ApplyToBeatmap(IBeatmap beatmap)
        {
            var touhosuBeatmap = (TouhosuBeatmap)beatmap;

            if (touhosuBeatmap.HitObjects.Count == 0) return;

            var hitObjects = touhosuBeatmap.HitObjects.Select(ho =>
                {
                    if (!(ho is AngeledProjectile projectile)) return ho;

                    projectile.PerfectHealthIncrease = 0;
                    return projectile;
                }
            ).ToList();
            touhosuBeatmap.HitObjects = hitObjects;
        }
    }
}
