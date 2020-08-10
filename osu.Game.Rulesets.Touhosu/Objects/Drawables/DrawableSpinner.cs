using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public class DrawableSpinner : DrawableTouhosuHitObject
    {
        public readonly Container<DrawableAngeledProjectile> ProjectilesContainer;

        public DrawableSpinner(Spinner h)
            : base(h)
        {
            AddInternal(ProjectilesContainer = new Container<DrawableAngeledProjectile>());
        }

        protected override void AddNestedHitObject(DrawableHitObject hitObject)
        {
            base.AddNestedHitObject(hitObject);

            switch (hitObject)
            {
                case DrawableAngeledProjectile projectile:
                    ProjectilesContainer.Add(projectile);
                    break;
            }
        }

        protected override void ClearNestedHitObjects()
        {
            base.ClearNestedHitObjects();
            ProjectilesContainer.Clear();
        }

        protected override DrawableHitObject CreateNestedHitObject(HitObject hitObject)
        {
            switch (hitObject)
            {
                case AngeledProjectile projectile:
                    return new DrawableAngeledProjectile(projectile);
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
