using System;

namespace osu.Game.Rulesets.Touhosu.Extensions
{
    public static class MathExtensions
    {
        public static double Pow(double input) => input * input;

        public static float BulletDistribution(int bulletsPerObject, float angleRange, int index, float angleOffset = 0)
        {
            return GetSafeAngle(getAngleBuffer(bulletsPerObject, angleRange) + index * getPerBulletAngle(bulletsPerObject, angleRange) + angleOffset); ;

            static float getAngleBuffer(int bulletsPerObject, float angleRange) => (360 - angleRange + getPerBulletAngle(bulletsPerObject, angleRange)) / 2f;

            static float getPerBulletAngle(int bulletsPerObject, float angleRange) => angleRange / bulletsPerObject;
        }

        public static float GetRandomTimedAngleOffset(double time)
        {
            var random = new Random((int)Math.Round(time * 100));
            return (float)random.NextDouble() * 360f;
        }

        public static bool GetRandomTimedBool(double time)
        {
            var random = new Random((int)Math.Round(time * 100));
            return random.NextDouble() > 0.5f;
        }

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
    }
}
