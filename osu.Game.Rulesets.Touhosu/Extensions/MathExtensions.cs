using osu.Game.Rulesets.Touhosu.UI.Objects;
using osuTK;
using System;

namespace osu.Game.Rulesets.Touhosu.Extensions
{
    public static class MathExtensions
    {
        public static float Map(float value, float lowerCurrent, float upperCurrent, float lowerTarget, float upperTarget)
        {
            return (value - lowerCurrent) / (upperCurrent - lowerCurrent) * (upperTarget - lowerTarget) + lowerTarget;
        }

        public static float BulletDistribution(int bulletsPerObject, float angleRange, int index, float angleOffset = 0)
        {
            var angle = getAngleBuffer(bulletsPerObject, angleRange) + index * getPerBulletAngle(bulletsPerObject, angleRange) + angleOffset;

            if (angle > 360)
                angle %= 360;

            return angle;

            static float getAngleBuffer(int bulletsPerObject, float angleRange) => (360 - angleRange + getPerBulletAngle(bulletsPerObject, angleRange)) / 2f;

            static float getPerBulletAngle(int bulletsPerObject, float angleRange) => angleRange / bulletsPerObject;
        }

        public static double Distance(Vector2 input, Vector2 comparison) => Math.Sqrt(Pow(input.X - comparison.X) + Pow(input.Y - comparison.Y));

        public static double Pow(double input) => input * input;

        public static float GetSafeAngle(float angle)
        {
            if (angle < 0)
            {
                while (angle < 0)
                    angle += 360;

                return angle;
            }

            if (angle > 360)
            {
                angle %= 360f;
                return angle;
            }

            return angle;
        }

        public static float GetPlayerAngle(TouhosuPlayer player, Vector2 input)
        {
            var playerPosition = player.PlayerPosition();
            var angle = (float)(Math.Atan2(input.Y - playerPosition.Y, input.X - playerPosition.X) * 180 / Math.PI) + 270;

            if (angle > 360)
                angle %= 360f;

            return angle;
        }

        public static bool GetRandomTimedBool(double time)
        {
            var random = new Random((int)Math.Round(time * 100));
            return random.NextDouble() > 0.5f;
        }

        public static float GetRandomTimedAngleOffset(double time)
        {
            var random = new Random((int)Math.Round(time * 100));
            return (float)random.NextDouble() * 360f;
        }
    }
}
