using osu.Game.Rulesets.Touhosu.Objects;
using osu.Framework.Graphics;
using osuTK;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Allocation;
using osu.Game.Graphics;
using osu.Framework.Graphics.Containers;

namespace osu.Game.Rulesets.Touhosu.Edit.Blueprints.Explosions.Components
{
    public class ExplosionPiece : BlueprintPiece<TouhosuHitObject>
    {
        private readonly CircularContainer ring;

        public ExplosionPiece()
        {
            Origin = Anchor.Centre;
            Size = new Vector2(80);
            InternalChild = ring = new CircularContainer
            {
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                BorderThickness = 5,
                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0,
                    AlwaysPresent = true
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
