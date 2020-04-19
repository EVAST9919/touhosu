using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace osu.Game.Rulesets.Touhosu.UI.Objects
{
    public class Card : Sprite
    {
        public Card()
        {
            Size = new Vector2(8, 20);
            Alpha = 0.5f;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            Texture = textures.Get("Player/card");
        }
    }
}
