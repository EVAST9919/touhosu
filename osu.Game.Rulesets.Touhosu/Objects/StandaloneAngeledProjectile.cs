using osuTK;

namespace osu.Game.Rulesets.Touhosu.Objects
{
    public class StandaloneAngeledProjectile : StandaloneProjectile
    {
        protected override AngeledProjectile CreateProjectile() => new AngeledProjectile
        {
            Angle = Angle,
            Position = Vector2.Zero,
            ParentPosition = Position,
            StartTime = StartTime,
            NewCombo = NewCombo,
            ComboOffset = ComboOffset,
            IndexInBeatmap = IndexInBeatmap
        };
    }
}
