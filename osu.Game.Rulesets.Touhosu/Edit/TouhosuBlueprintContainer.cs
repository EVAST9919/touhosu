using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Touhosu.Edit.Blueprints.Explosions;
using osu.Game.Rulesets.Touhosu.Edit.Blueprints.Projectiles;
using osu.Game.Rulesets.Touhosu.Objects.Drawables;
using osu.Game.Screens.Edit.Compose.Components;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Touhosu.Edit
{
    public class TouhosuBlueprintContainer : ComposeBlueprintContainer
    {
        public TouhosuBlueprintContainer(IEnumerable<DrawableHitObject> drawableHitObjects)
            : base(drawableHitObjects)
        {
        }

        protected override SelectionHandler CreateSelectionHandler() => new TouhosuSelectionHandler();

        public override OverlaySelectionBlueprint CreateBlueprintFor(DrawableHitObject hitObject)
        {
            switch (hitObject)
            {
                case DrawableSpinner spinner:
                    return new SpinnerSelectionBlueprint(spinner);

                case DrawableExplosion explosion:
                    return new ExplosionSelectionBlueprint(explosion);

                case DrawableStandaloneProjectile projectile:
                    return new ProjectileSelectionBlueprint(projectile);
            }

            return base.CreateBlueprintFor(hitObject);
        }
    }
}
