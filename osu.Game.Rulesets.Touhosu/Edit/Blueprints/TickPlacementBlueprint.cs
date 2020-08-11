using osu.Game.Rulesets.Touhosu.Edit.Blueprints.Projectiles.Components;
using osu.Game.Rulesets.Touhosu.Objects;

namespace osu.Game.Rulesets.Touhosu.Edit.Blueprints
{
    public class TickPlacementBlueprint : TouhosuPlacementBlueprint<TickProjectile>
    {
        private readonly ProjectilePiece piece;

        public TickPlacementBlueprint()
            : base(new TickProjectile())
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
