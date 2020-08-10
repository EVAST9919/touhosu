using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Game.Graphics;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Touhosu.Objects.Drawables.Pieces
{
    public class ProjectilePiece : CircularContainer, IHasAccentColour
    {
        private Color4 accentColour;

        public Color4 AccentColour
        {
            get => accentColour;
            set
            {
                accentColour = value;
                overlay.Colour = value;

                if (useGlow)
                {
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Colour = value.Opacity(0.35f),
                        Type = EdgeEffectType.Glow,
                        Radius = 5,
                    };
                }
            }
        }

        private readonly string textureName;
        private readonly bool useGlow;
        private readonly Sprite texture;
        private readonly Sprite overlay;

        public ProjectilePiece(string textureName, bool useGlow)
        {
            this.textureName = textureName;
            this.useGlow = useGlow;

            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Children = new Drawable[]
            {
                texture = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both
                },
                overlay = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both
                }
            };

            if (useGlow)
                Masking = true;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            texture.Texture = textures.Get($"Projectiles/{textureName}/texture");
            overlay.Texture = textures.Get($"Projectiles/{textureName}/overlay");
        }
    }
}
