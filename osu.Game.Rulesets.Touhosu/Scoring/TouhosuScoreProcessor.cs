using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Touhosu.Scoring
{
    public class TouhosuScoreProcessor : ScoreProcessor
    {
        public override HitWindows CreateHitWindows() => new TouhosuHitWindows();
    }
}
