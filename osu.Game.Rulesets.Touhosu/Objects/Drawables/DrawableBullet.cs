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
    public abstract class DrawableBullet : DrawableTouhosuHitObject
    {
        private const int hidden_distance = 70;
        private const int hidden_distance_buffer = 50;

        public bool HiddenApplied;

        protected virtual float GetBaseSize() => 25;

        protected virtual bool AffectPlayer() => false;

        protected virtual float GetWallCheckOffset() => 0;

        private Sprite texture;
        private Sprite overlay;
        protected Container Content;
        private double missTime;

        protected DrawableBullet(Bullet h)
            : base(h)
        {
            Origin = Anchor.Centre;
            Size = new Vector2(GetBaseSize() * MathExtensions.Map(h.CircleSize, 0, 10, 0.2f, 1));
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

        protected override void Update()
        {
            base.Update();

            if (HiddenApplied)
            {
                var distance = CheckDistance.Invoke(this);

                if (distance > hidden_distance + hidden_distance_buffer)
                {
                    Alpha = 1;
                    return;
                }

                if (distance < hidden_distance)
                {
                    Alpha = 0;
                    return;
                }

                Alpha = MathExtensions.Map(distance - hidden_distance, 0, hidden_distance_buffer, 0, 1);
            }
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

                if (timeOffset > GetWallCheckOffset())
                {
                    if (Position.X > TouhosuPlayfield.ACTUAL_SIZE.X + Size.X / 2f
                    || Position.X < -Size.X / 2f
                    || Position.Y > TouhosuPlayfield.BASE_SIZE.Y + Size.Y / 2f
                    || Position.Y < -Size.Y / 2f)
                        ApplyResult(r => r.Type = HitResult.Perfect);
                }
            }
        }

        public Func<DrawableBullet, bool> CheckHit;

        public Func<DrawableBullet, float> CheckDistance;

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
