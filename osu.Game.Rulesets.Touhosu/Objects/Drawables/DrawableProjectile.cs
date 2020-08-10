using osu.Framework.Graphics;
using osuTK;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Touhosu.UI;
using System;
using osu.Game.Rulesets.Touhosu.Objects.Drawables.Pieces;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public abstract class DrawableProjectile : DrawableTouhosuHitObject
    {
        protected virtual float BaseSize { get; } = 25;

        protected virtual bool UseGlow { get; } = true;

        protected virtual string ProjectileName { get; } = "Sphere";

        protected readonly ProjectilePiece Piece;
        private readonly bool expireOnWallHit;
        private readonly bool affectPlayer;
        private double missTime;

        protected DrawableProjectile(Projectile h)
            : base(h)
        {
            expireOnWallHit = h.ExpireOnWallHit;
            affectPlayer = h.AffectPlayer;

            Origin = Anchor.Centre;
            Size = new Vector2(BaseSize * h.SizeAdjustValue);
            Position = h.Position;
            Scale = Vector2.Zero;
            AddInternal(Piece = new ProjectilePiece(ProjectileName, UseGlow));
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AccentColour.BindValueChanged(accent => Piece.AccentColour = accent.NewValue, true);
        }

        protected override void UpdateInitialTransforms()
        {
            this.ScaleTo(Vector2.One, HitObject.TimePreempt);
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (timeOffset > 0)
            {
                if (affectPlayer)
                {
                    if (CheckHit.Invoke(this))
                    {
                        missTime = timeOffset;
                        ApplyResult(r => r.Type = HitResult.Miss);
                        return;
                    }
                }

                if (expireOnWallHit)
                {
                    if (Position.X > TouhosuPlayfield.PLAYFIELD_SIZE.X + Size.X / 2f
                    || Position.X < -Size.X / 2f
                    || Position.Y > TouhosuPlayfield.PLAYFIELD_SIZE.Y + Size.Y / 2f
                    || Position.Y < -Size.Y / 2f)
                        ApplyResult(r => r.Type = HitResult.Perfect);
                }
            }
        }

        public Func<DrawableProjectile, bool> CheckHit;

        public Func<DrawableProjectile, float> GetDistanceFromPlayer;

        protected override void UpdateStateTransforms(ArmedState state)
        {
            base.UpdateStateTransforms(state);

            switch (state)
            {
                case ArmedState.Miss:
                    this.Delay(missTime).FadeOut();
                    break;
            }
        }
    }
}
