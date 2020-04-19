using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Touhosu.Objects.Drawables;
using osu.Game.Rulesets.Touhosu.Extensions;
using osu.Game.Rulesets.UI;
using System;

namespace osu.Game.Rulesets.Touhosu.UI.Objects
{
    public class SmartCard : Sprite
    {
        private const double base_speed = 1.0 / 2;

        public HitObjectContainer HitObjects { get; set; }

        private DrawableBulletGenerator currentTarget;

        public SmartCard(DrawableBulletGenerator initialTarget)
        {
            currentTarget = initialTarget;

            Origin = Anchor.Centre;
            Size = new Vector2(6, 8);
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            Texture = textures.Get("Player/smart-card");
        }

        protected override void Update()
        {
            base.Update();

            if (currentTarget?.IsHit ?? true)
            {
                setNewTarget();
                return;
            }

            onNewStep();
        }

        private bool shouldUpdateRotation;
        private float targetAngle;

        private void onNewStep()
        {
            if (shouldUpdateRotation)
            {
                targetAngle = getTargetAngle(currentTarget);
                Rotation = targetAngle;
            }

            var timeElapsed = Clock.ElapsedFrameTime;

            var xDelta = timeElapsed * base_speed * Math.Sin(targetAngle * Math.PI / 180);
            var yDelta = timeElapsed * base_speed * -Math.Cos(targetAngle * Math.PI / 180);

            Position = new Vector2(Position.X + (float)xDelta, Position.Y + (float)yDelta);

            shouldUpdateRotation = !shouldUpdateRotation;
        }

        private void setNewTarget()
        {
            currentTarget = BulletsExtensions.GetClosest(Position, HitObjects);

            if (currentTarget == null)
            {
                Expire();
                return;
            }
        }

        private float getTargetAngle(DrawableBulletGenerator target)
        {
            var angle = Math.Atan2(Position.Y - target.Y, Position.X - target.X) * 180 / Math.PI - 90;
            if (angle > 360)
                angle %= 360;
            return (float)angle;
        }
    }
}
