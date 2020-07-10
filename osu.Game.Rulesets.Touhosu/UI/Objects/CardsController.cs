using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osuTK;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Touhosu.UI.Objects
{
    public class CardsController : CompositeDrawable
    {
        private const float speed_per_field = 300;

        public override bool RemoveCompletedTransforms => true;

        public HitObjectContainer HitObjects { get; set; }

        public CardsController()
        {
            RelativeSizeAxes = Axes.Both;
        }

        public void Shoot(Vector2 position, bool isFocused)
        {
            Card left;
            Card right;

            AddRangeInternal(new Drawable[]
            {
                left = new Card
                {
                    Position = new Vector2(position.X + 5, position.Y - 15),
                    Strength = isFocused ? 2 : 1
                },
                right = new Card
                {
                    Position = new Vector2(position.X - 5, position.Y - 15),
                    Strength = isFocused ? 2 : 1
                },
            });

            var time = speed_per_field * (position.Y / TouhosuPlayfield.PLAYFIELD_SIZE.Y);
            left.MoveToY(0, time).Expire();
            right.MoveToY(0, time).Expire();
        }

        public List<Card> GetCards() => InternalChildren.OfType<Card>().Where(c => c.IsAlive).ToList();
    }
}
