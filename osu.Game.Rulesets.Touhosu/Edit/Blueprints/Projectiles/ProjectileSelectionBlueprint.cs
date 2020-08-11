using osu.Framework.Graphics.Primitives;
using osu.Game.Rulesets.Touhosu.Edit.Blueprints.Projectiles.Components;
using osu.Game.Rulesets.Touhosu.Objects;
using osu.Game.Rulesets.Touhosu.Objects.Drawables;
using osuTK;

namespace osu.Game.Rulesets.Touhosu.Edit.Blueprints.Projectiles
{
    public class ProjectileSelectionBlueprint : TouhosuSelectionBlueprint<Projectile>
    {
        protected new DrawableProjectile DrawableObject => (DrawableProjectile)base.DrawableObject;

        protected readonly ProjectilePiece Piece;

        public ProjectileSelectionBlueprint(DrawableProjectile drawable)
            : base(drawable)
        {
            InternalChild = Piece = new ProjectilePiece();
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
