using osu.Game.Rulesets.Objects.Types;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public class PathBullet : Bullet
    {
        public IHasPathWithRepeats Path { get; set; }

        public float TimeOffset { get; set; }
    }
}
