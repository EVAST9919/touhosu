using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osuTK;

namespace osu.Game.Rulesets.Touhosu.UI.Objects
{
    public class CardsController : CompositeDrawable
    {
        private const float speed_per_field = 300;

        public override bool RemoveCompletedTransforms => true;

        public CardsController()
        {
            RelativeSizeAxes = Axes.Both;
        }

        public void Shoot(Vector2 position)
        {
            var cardStack = new FillFlowContainer
            {
                Position = new Vector2(position.X, position.Y - 20),
                AutoSizeAxes = Axes.Both,
                Origin = Anchor.BottomCentre,
                Direction = FillDirection.Horizontal,
                Spacing = new Vector2(5, 0),
                Children = new[]
                {
                    new Card(),
                    new Card()
                }
            };

            AddInternal(cardStack);

            cardStack.MoveToY(0, speed_per_field * (position.Y / TouhosuPlayfield.ACTUAL_SIZE.Y)).Expire();
        }
    }
}
