using osu.Game.Rulesets.Touhosu.Objects;
using osu.Framework.Graphics;
using osuTK;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Allocation;
using osu.Game.Graphics;

namespace osu.Game.Rulesets.Touhosu.Edit.Blueprints.Explosions.Components
{
    public class ExplosionPiece : BlueprintPiece<TouhosuHitObject>
    {
        public ExplosionPiece()
        {
            Origin = Anchor.Centre;
            Size = new Vector2(100);
            InternalChild = new Circle
            {
                RelativeSizeAxes = Axes.Both,
                Masking = true,
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Colour = colours.Yellow;
        }
    }
}
