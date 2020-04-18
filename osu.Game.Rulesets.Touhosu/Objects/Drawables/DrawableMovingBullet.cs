using osuTK;
using System;
using osu.Game.Rulesets.Touhosu.UI;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Touhosu.UI.Objects;
using osu.Game.Rulesets.Touhosu.Extensions;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public class DrawableMovingBullet : DrawableBullet
    {
        private const float speed_multiplier = 6f;

        private readonly float finalSize;
        private float duration;
        private float finalX;
        private float finalY;

        public DrawableMovingBullet(MovingBullet h)
            : base(h)
        {
            AlwaysPresent = true;
            finalSize = Size.X;
        }

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();

            var angle = GetAngle();
            var wall = selectWall(angle);

            switch (wall)
            {
                case Wall.Bottom:
                    finalY = TouhosuPlayfield.ACTUAL_SIZE.Y + (finalSize / 2f);
                    finalX = (float)getXPosition(Position, finalY, angle);
                    break;

                case Wall.Top:
                    finalY = -finalSize / 2f;
                    finalX = (float)getXPosition(Position, finalY, angle);
                    break;

                case Wall.Left:
                    finalX = -finalSize / 2f;
                    finalY = (float)getYPosition(Position, finalX, angle);
                    break;

                case Wall.Right:
                    finalX = TouhosuPlayfield.ACTUAL_SIZE.X + (finalSize / 2f);
                    finalY = (float)getYPosition(Position, finalX, angle);
                    break;
            }

            var distance = Math.Sqrt(((finalX - Position.X) * (finalX - Position.X)) + ((finalY - Position.Y) * (finalY - Position.Y)));
            duration = (float)distance / MathExtensions.Map((float)((MovingBullet)HitObject).SpeedMultiplier, 0, 3, 0.9f, 1.2f) * speed_multiplier;

            this.Delay(HitObject.TimePreempt).MoveTo(new Vector2(finalX, finalY), duration);
        }

        protected virtual float GetAngle() => ((MovingBullet)HitObject).Angle;

        private double missTime;

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (timeOffset > 0)
            {
                if (collidedWithPlayer(Player))
                {
                    Player.PlayMissAnimation();
                    missTime = timeOffset;
                    ApplyResult(r => r.Type = HitResult.Miss);
                    return;
                }

                if (timeOffset > duration)
                    ApplyResult(r => r.Type = HitResult.Perfect);
            }
        }

        private bool collidedWithPlayer(TouhosuPlayer player)
        {
            var playerPosition = player.PlayerPosition();
            var distance = Math.Sqrt(Math.Pow(playerPosition.X - Position.X, 2) + Math.Pow(playerPosition.Y - Position.Y, 2));
            var bulletRadius = finalSize / 2f;

            if (distance < bulletRadius)
                return true;

            return false;
        }

        protected override void UpdateStateTransforms(ArmedState state)
        {
            base.UpdateStateTransforms(state);

            switch (state)
            {
                case ArmedState.Miss:
                    // Check DrawableHitCircle L#168
                    this.Delay(missTime).FadeOut();
                    break;
            }
        }

        private Wall selectWall(float angle)
        {
            // Top/Right
            if (angle <= 90)
            {
                if (angle < getCornerAngle(Position, new Vector2(TouhosuPlayfield.ACTUAL_SIZE.X, 0)) - 360)
                    return Wall.Top;

                return Wall.Right;
            }

            // Right/Bottom
            if (angle <= 180)
            {
                if (angle < getCornerAngle(Position, new Vector2(TouhosuPlayfield.ACTUAL_SIZE.X, TouhosuPlayfield.ACTUAL_SIZE.Y)))
                    return Wall.Right;

                return Wall.Bottom;
            }

            // Bottom/Left
            if (angle <= 270)
            {
                if (angle < getCornerAngle(Position, new Vector2(0, TouhosuPlayfield.ACTUAL_SIZE.Y)))
                    return Wall.Bottom;

                return Wall.Left;
            }

            if (angle < getCornerAngle(Position, Vector2.Zero))
                return Wall.Left;

            return Wall.Top;
        }

        private static double getXPosition(Vector2 initialPosition, float finalY, float angle)
        {
            var time = (finalY - initialPosition.Y) / -Math.Cos(angle * Math.PI / 180);
            return initialPosition.X + (time * Math.Sin(angle * Math.PI / 180));
        }

        private static double getYPosition(Vector2 initialPosition, float finalX, float angle)
        {
            var time = (finalX - initialPosition.X) / Math.Sin(angle * Math.PI / 180);
            return initialPosition.Y + (time * -Math.Cos(angle * Math.PI / 180));
        }

        private static float getCornerAngle(Vector2 objectPosition, Vector2 cornerPosition)
        {
            return (float)(Math.Atan2(objectPosition.Y - cornerPosition.Y, objectPosition.X - cornerPosition.X) * 180 / Math.PI) + 270;
        }

        public enum Wall
        {
            Top,
            Right,
            Left,
            Bottom
        }
    }
}
