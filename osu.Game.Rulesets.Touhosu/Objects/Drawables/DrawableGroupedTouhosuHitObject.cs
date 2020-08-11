using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public abstract class DrawableGroupedTouhosuHitObject<T, U> : DrawableTouhosuHitObject
        where T : Projectile
        where U : TouhosuHitObject
    {
        private readonly Container<DrawableSoundHitObject> soundContainer;
        public readonly Container<DrawableProjectile> ProjectilesContainer;

        public DrawableGroupedTouhosuHitObject(U h)
            : base(h)
        {
            Origin = Anchor.Centre;
            Position = h.Position;
            AddRangeInternal(new Drawable[]
            {
                soundContainer = new Container<DrawableSoundHitObject>(),
                ProjectilesContainer = new Container<DrawableProjectile>()
            });
        }

        protected override void AddNestedHitObject(DrawableHitObject hitObject)
        {
            base.AddNestedHitObject(hitObject);

            switch (hitObject)
            {
                case DrawableSoundHitObject sound:
                    soundContainer.Child = sound;
                    break;

                case DrawableProjectile projectile:
                    ProjectilesContainer.Add(projectile);
                    break;
            }
        }

        protected override void ClearNestedHitObjects()
        {
            base.ClearNestedHitObjects();
            soundContainer.Clear();
            ProjectilesContainer.Clear();
        }

        protected override DrawableHitObject CreateNestedHitObject(HitObject hitObject)
        {
            switch (hitObject)
            {
                case SoundHitObject sound:
                    return new DrawableSoundHitObject(sound);

                case T projectile:
                    return CreateDrawableProjectile(projectile);
            }

            return base.CreateNestedHitObject(hitObject);
        }

        protected abstract DrawableProjectile CreateDrawableProjectile(T projectile);

        protected override void LoadSamples()
        {
            base.LoadSamples();

            foreach (var o in NestedHitObjects)
            {
                if (o is DrawableSoundHitObject)
                    o.HitObject.Samples = HitObject.Samples;
            }
        }

        public override void PlaySamples()
        {
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
