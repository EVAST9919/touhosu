using osu.Framework.Graphics.Primitives;
using osu.Game.Rulesets.Touhosu.Edit.Blueprints.Explosions.Components;
using osu.Game.Rulesets.Touhosu.Objects;
using osu.Game.Rulesets.Touhosu.Objects.Drawables;
using osuTK;

namespace osu.Game.Rulesets.Touhosu.Edit.Blueprints.Explosions
{
    public class ExplosionSelectionBlueprint : TouhosuSelectionBlueprint<Explosion>
    {
        protected new DrawableExplosion DrawableObject => (DrawableExplosion)base.DrawableObject;

        protected readonly ExplosionPiece Piece;

        public ExplosionSelectionBlueprint(DrawableExplosion drawable)
            : base(drawable)
        {
            InternalChild = Piece = new ExplosionPiece();
        }

        protected override void Update()
        {
            base.Update();

            Piece.UpdateFrom(HitObject);
        }

        public override Quad SelectionQuad => Piece.ScreenSpaceDrawQuad;

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => Piece.ReceivePositionalInputAt(screenSpacePos);
    }
}
