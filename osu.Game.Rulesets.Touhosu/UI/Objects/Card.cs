using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Touhosu.UI.Objects
{
    public class Card : Sprite
    {
        public int Strength { get; set; }

        public Card()
        {
            Size = new Vector2(8, 20);
            Origin = Anchor.BottomCentre;
            Alpha = 0.5f;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            Texture = textures.Get("Player/card");
        }
    }
}
