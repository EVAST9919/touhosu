
using osu.Game.Rulesets.Touhosu.Edit.Blueprints.Explosions.Components;
using osu.Game.Rulesets.Touhosu.Objects;

namespace osu.Game.Rulesets.Touhosu.Edit.Blueprints
{
    public class CircularExplosionPlacementBlueprint : TouhosuPlacementBlueprint<CircularExplosion>
    {
        private readonly ExplosionPiece piece;

        public CircularExplosionPlacementBlueprint()
            : base(new CircularExplosion())
        {
            InternalChild = piece = new ExplosionPiece();
        }

        protected override void Update()
        {
            base.Update();
            piece.UpdateFrom(HitObject);
        }
    }
}
