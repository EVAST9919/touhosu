using osu.Framework.Graphics;
using osuTK;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Framework.Utils;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public class DrawableBulletGenerator : DrawableTouhosuHitObject
    {
        private readonly Box box;

        private double missTime;
        private double hitTime;
        private int hp = 5;

        public DrawableBulletGenerator(BulletGenerator h)
            : base(h)
        {
            Origin = Anchor.Centre;
            Size = new Vector2(20);
            Position = getPosition();
            AlwaysPresent = true;
            AddRangeInternal(new Drawable[]
            {
                box = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0.5f
                }
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AccentColour.BindValueChanged(accent => box.Colour = accent.NewValue, true);
        }

        protected override void UpdateInitialTransforms()
        {
            this.MoveTo(HitObject.Position, HitObject.TimePreempt);
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (timeOffset > 0)
            {
                foreach (var card in Player.GetCards())
                {
                    if (!(card.Position.Y - card.DrawHeight > Position.Y + DrawHeight / 2f
                        || card.Position.Y < Position.Y - DrawHeight / 2f
                        || card.Position.X + card.DrawWidth / 2f < Position.X - DrawWidth / 2f
                        || card.Position.X - card.DrawWidth / 2f > Position.X + DrawWidth / 2f))
                    {
                        card.ClearTransforms();
                        card.Expire();
                        hp -= card.Strength;
                    }
                };

                foreach (var card in Player.GetSmartCards())
                {
                    if (Precision.AlmostEquals(card.Position.X, Position.X, 0.5f) && Precision.AlmostEquals(card.Position.Y, Position.Y, 0.5f))
                    {
                        card.ClearTransforms();
                        card.Expire();
                        hp--;
                    }
                }

                if (hp <= 0)
                {
                    hitTime = timeOffset;
                    ApplyResult(r => r.Type = HitResult.Perfect);
                }
            }
        }

        protected override void UpdateStateTransforms(ArmedState state)
        {
            base.UpdateStateTransforms(state);

            switch (state)
            {
                case ArmedState.Hit:
                    // Check DrawableHitCircle L#168
                    this.Delay(hitTime).FadeOut();
                    break;

                case ArmedState.Miss:
                    // Check DrawableHitCircle L#168
                    this.Delay(missTime).FadeOut();
                    break;
            }
        }

        private Vector2 getPosition()
        {
            return new Vector2(20);
        }
    }
}
