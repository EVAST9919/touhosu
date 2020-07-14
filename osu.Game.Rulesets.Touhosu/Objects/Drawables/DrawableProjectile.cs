using osu.Framework.Graphics;
using osuTK;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Touhosu.Extensions;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Touhosu.UI;
using System;
using osu.Framework.Graphics.Effects;
using osu.Framework.Extensions.Color4Extensions;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public abstract class DrawableProjectile : DrawableTouhosuHitObject
    {
        protected virtual float BaseSize() => 25;

        protected virtual bool AffectPlayer() => true;

        protected virtual bool CheckWallCollision() => true;

        protected virtual bool UseGlow() => true;

        private Sprite texture;
        private Sprite overlay;
        protected CircularContainer Content;
        private double missTime;

        protected DrawableProjectile(Projectile h)
            : base(h)
        {
            Origin = Anchor.Centre;
            Size = new Vector2(BaseSize() * MathExtensions.Map(h.CircleSize, 0, 10, 0.2f, 1));
            Position = h.Position;
            Scale = Vector2.Zero;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            AddInternal(Content = new CircularContainer
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new Drawable[]
                {
                    texture = new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                    },
                    overlay = new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                    }
                }
            });

            if (UseGlow())
                Content.Masking = true;

            texture.Texture = textures.Get($"Projectiles/{ProjectileName()}/texture");
            overlay.Texture = textures.Get($"Projectiles/{ProjectileName()}/overlay");

            AccentColour.BindValueChanged(accent =>
            {
                overlay.Colour = accent.NewValue;

                if (UseGlow())
                {
                    Content.EdgeEffect = new EdgeEffectParameters
                    {
                        Colour = accent.NewValue.Opacity(0.5f),
                        Type = EdgeEffectType.Glow,
                        Radius = 5,
                    };
                }
            }, true);
        }

        protected virtual string ProjectileName() => "Sphere";

        protected override void UpdateInitialTransforms()
        {
            this.ScaleTo(Vector2.One, HitObject.TimePreempt);
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            if (timeOffset > 0)
            {
                if (AffectPlayer())
                {
                    if (CheckHit.Invoke(this))
                    {
                        missTime = timeOffset;
                        ApplyResult(r => r.Type = HitResult.Miss);
                        return;
                    }
                }

                if (CheckWallCollision())
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

        public Func<DrawableProjectile, float> CheckDistance;

        protected override void UpdateStateTransforms(ArmedState state)
        {
            base.UpdateStateTransforms(state);

            switch (state)
            {
                case ArmedState.Miss:
                    // Check DrawableHitCircle L#168
                    this.Delay(missTime).FadeOut();
                    break;
            }
        }
    }
}
