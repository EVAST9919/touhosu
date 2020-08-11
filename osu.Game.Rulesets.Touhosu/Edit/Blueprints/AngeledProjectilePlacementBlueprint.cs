using osu.Game.Rulesets.Touhosu.Edit.Blueprints.Projectiles.Components;
using osu.Game.Rulesets.Touhosu.Objects;

namespace osu.Game.Rulesets.Touhosu.Edit.Blueprints
{
    public class AngeledProjectilePlacementBlueprint : TouhosuPlacementBlueprint<StandaloneAngeledProjectile>
    {
        private readonly ProjectilePiece piece;

        public AngeledProjectilePlacementBlueprint()
            : base(new StandaloneAngeledProjectile())
        {
            InternalChild = piece = new ProjectilePiece();
        }

        protected override void Update()
        {
            base.Update();
            piece.UpdateFrom(HitObject);
        }
    }
}
