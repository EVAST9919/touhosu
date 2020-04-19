using osu.Framework.Graphics;
using osuTK;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Touhosu.Extensions;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables
{
    public abstract class DrawableBullet : DrawableTouhosuHitObject
    {
        protected virtual float GetBaseSize() => 40;

        private readonly Sprite texture;
        private readonly Sprite overlay;
        protected readonly Container Content;

        protected DrawableBullet(Bullet h)
            : base(h)
        {
            Origin = Anchor.Centre;
            Size = new Vector2(GetBaseSize() * MathExtensions.Map(h.CircleSize, 0, 10, 0.2f, 1));
            Position = h.Position;
            Scale = Vector2.Zero;

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
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            texture.Texture = textures.Get("Projectiles/Sphere/texture");
            overlay.Texture = textures.Get("Projectiles/Sphere/overlay");

            AccentColour.BindValueChanged(accent => overlay.Colour = accent.NewValue, true);
        }

        protected override void UpdateInitialTransforms()
        {
            this.ScaleTo(Vector2.One, HitObject.TimePreempt);
        }
    }
}
