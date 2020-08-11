using osuTK;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public class StandaloneTickProjectile : StandaloneProjectile
    {
        protected override AngeledProjectile CreateProjectile() => new TickProjectile
        {
            Position = Vector2.Zero,
            ParentPosition = Position,
            StartTime = StartTime,
            NewCombo = NewCombo,
            ComboOffset = ComboOffset,
            IndexInBeatmap = IndexInBeatmap
        };
    }
}
