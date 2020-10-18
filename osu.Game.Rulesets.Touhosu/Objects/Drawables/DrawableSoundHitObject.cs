using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Touhosu.UI;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public class DrawableSoundHitObject : DrawableTouhosuHitObject
    {
        protected override float SamplePlaybackPosition => HitObject.X / TouhosuPlayfield.PLAYFIELD_SIZE.X;

        public DrawableSoundHitObject(SoundHitObject h)
            : base(h)
        {
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (timeOffset > 0)
                ApplyResult(r => r.Type = HitResult.IgnoreHit);
        }
    }
}
