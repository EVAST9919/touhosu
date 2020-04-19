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

        public static double GetDistance(Vector2 input, Vector2 comparison) => Math.Sqrt(Math.Pow(input.X - comparison.X, 2) + Math.Pow(input.Y - comparison.Y, 2));
    }
}
