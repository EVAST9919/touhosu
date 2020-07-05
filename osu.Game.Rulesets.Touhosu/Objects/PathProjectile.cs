using osu.Game.Rulesets.Objects.Types;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public class PathProjectile : Projectile
    {
        public IHasPathWithRepeats Path { get; set; }

        public float TimeOffset { get; set; }

        public float Intensity { get; set; }
    }
}
