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

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public abstract class DrawableProjectile : DrawableTouhosuHitObject
    {
        protected virtual float BaseSize() => 25;

        protected virtual bool AffectPlayer() => true;

        protected virtual bool CheckWallCollision() => true;

        private Sprite texture;
        private Sprite overlay;
        protected Container Content;
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
            AddInternal(Content = new Container
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

            texture.Texture = textures.Get($"Projectiles/{ProjectileName()}/texture");
            overlay.Texture = textures.Get($"Projectiles/{ProjectileName()}/overlay");

            AccentColour.BindValueChanged(accent => overlay.Colour = accent.NewValue, true);
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
