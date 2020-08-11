using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Rulesets.Touhosu.Objects;
using osuTK;

namespace osu.Game.Rulesets.Touhosu.Edit.Blueprints.Projectiles.Components
{
    public class ProjectilePiece : BlueprintPiece<AngeledProjectile>
    {
        private readonly CircularContainer ring;

        public ProjectilePiece()
        {
            Origin = Anchor.Centre;
            Size = new Vector2(50);
            InternalChild = ring = new CircularContainer
            {
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                BorderThickness = 3,
                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    AlwaysPresent = true,
                    Alpha = 0
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            ring.BorderColour = colours.Yellow;
        }
    }
}
