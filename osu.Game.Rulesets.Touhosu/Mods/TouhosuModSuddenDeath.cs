using System;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.Touhosu.Mods
{
    public class TouhosuModSuddenDeath : ModSuddenDeath
    {
        public override Type[] IncompatibleMods => new[] { typeof(TouhosuModNoRegen), typeof(TouhosuModDamageMultiplier) };
    }
}
