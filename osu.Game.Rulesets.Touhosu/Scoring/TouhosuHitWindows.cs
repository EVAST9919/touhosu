using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Touhosu.Scoring
{
    public class TouhosuHitWindows : HitWindows
    {
        public override bool IsHitResultAllowed(HitResult result)
        {
            switch (result)
            {
                case HitResult.Perfect:
                case HitResult.Miss:
                    return true;
            }

            return false;
        }
    }
}
