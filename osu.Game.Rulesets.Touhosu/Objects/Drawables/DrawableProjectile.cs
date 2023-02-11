using JetBrains.Annotations;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osuTK;
using System;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Framework.Utils;
using osu.Game.Rulesets.Touhosu.Objects.Drawables.Pieces;
using osu.Game.Rulesets.Touhosu.Extensions;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public abstract partial class DrawableProjectile<T> : DrawableTouhosuHitObject<T>
        where T : Projectile
    {
        private const int hidden_distance = 70;
        private const int hidden_distance_buffer = 50;

        public bool IsGrazed = false;

        public readonly IBindable<Vector2> PositionBindable = new Bindable<Vector2>();

        protected abstract bool CanHitPlayer { get; }

        public Func<Vector2, bool> CheckHit;

        public Func<Vector2, bool> CheckGrazed;
        public Func<Vector2, float> DistanceToPlayer;

        public bool HiddenApplied { get; set; }

        private ProjectilePiece piece;

        protected DrawableProjectile([CanBeNull] T h = null)
            : base(h)
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Alpha = 0;
            Scale = Vector2.Zero;

            Origin = Anchor.Centre;
            Size = new Vector2(TouhosuExtensions.SPHERE_SIZE);
            AddInternal(piece = new ProjectilePiece("Sphere", true));

            AccentColour.BindValueChanged(c => piece.AccentColour = c.NewValue);
            PositionBindable.BindValueChanged(p => Position = p.NewValue);
        }

        protected override void UpdateInitialTransforms()
        {
            base.UpdateInitialTransforms();
            this.FadeInFromZero();
            this.ScaleTo(Vector2.One, HitObject.TimePreempt);
        }

        protected override void Update()
        {
            base.Update();

            if (IsHit || !HiddenApplied)
                return;

            var distance = DistanceToPlayer.Invoke(Position);

            if (distance > hidden_distance + hidden_distance_buffer)
            {
                piece.Alpha = 1;
                return;
            }

            if (distance < hidden_distance)
            {
                piece.Alpha = 0;
                return;
            }

            piece.Alpha = Interpolation.ValueAt((float)distance - hidden_distance, 0f, 1f, 0, hidden_distance_buffer);
        }

        private double missTime;

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (timeOffset < 0)
                return;

            if (!CanHitPlayer)
                return;

            if (!IsGrazed && (CheckGrazed?.Invoke(Position) ?? false))
                IsGrazed = true;

            if (CheckHit?.Invoke(Position) ?? false)
            {
                missTime = timeOffset;
                ApplyResult(h => h.Type = h.Judgement.MinResult);
                return;
            }
        }

        protected override void UpdateHitStateTransforms(ArmedState state)
        {
            base.UpdateHitStateTransforms(state);

            switch (state)
            {
                case ArmedState.Miss:
                    this.Delay(missTime).FadeOut();
                    break;
            }
        }

        protected override void OnApply()
        {
            base.OnApply();

            PositionBindable.BindTo(HitObject.PositionBindable);
            IsGrazed = false;
        }

        protected override void OnFree()
        {
            base.OnFree();

            PositionBindable.UnbindFrom(HitObject.PositionBindable);
        }


        protected override double InitialLifetimeOffset => HitObject.TimePreempt;
    }
}
