using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public abstract class DrawableExplosion : DrawableTouhosuHitObject
    {
        public readonly Container<DrawableAngeledProjectile> ProjectilesContainer;
        private readonly Container<DrawableSoundHitObject> soundContainer;

        public DrawableExplosion(Explosion h)
            : base(h)
        {
            AddRangeInternal(new Drawable[]
            {
                ProjectilesContainer = new Container<DrawableAngeledProjectile>(),
                soundContainer = new Container<DrawableSoundHitObject>()
            });
        }

        protected override void AddNestedHitObject(DrawableHitObject hitObject)
        {
            base.AddNestedHitObject(hitObject);

            switch (hitObject)
            {
                case DrawableAngeledProjectile projectile:
                    ProjectilesContainer.Add(projectile);
                    break;

                case DrawableSoundHitObject sound:
                    soundContainer.Child = sound;
                    break;
            }
        }

        protected override void ClearNestedHitObjects()
        {
            base.ClearNestedHitObjects();
            ProjectilesContainer.Clear();
            soundContainer.Clear();
        }

        protected override DrawableHitObject CreateNestedHitObject(HitObject hitObject)
        {
            switch (hitObject)
            {
                case AngeledProjectile projectile:
                    return new DrawableAngeledProjectile(projectile);

                case SoundHitObject sound:
                    return new DrawableSoundHitObject(sound);
            }

            return base.CreateNestedHitObject(hitObject);
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            base.CheckForResult(userTriggered, timeOffset);

            if (timeOffset < 0)
                return;

            foreach (var p in ProjectilesContainer)
            {
                if (!p.Result.HasResult)
                    return;
            }

            ApplyResult(r => r.Type = HitResult.Meh);
        }
    }
}
