using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Touhosu.Scoring;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osu.Framework.Graphics.Textures;
using osu.Framework.Allocation;
using osu.Framework.Extensions.IEnumerableExtensions;

namespace osu.Game.Rulesets.Touhosu.UI.HUD
{
    public class HealthDisplay : CompositeDrawable
    {
        private readonly TouhosuHealthProcessor healthProcessor;

        [Resolved]
        private TextureStore textures { get; set; }

        private readonly Sprite[] hearts = new Sprite[10];

        public HealthDisplay(TouhosuHealthProcessor healthProcessor)
        {
            this.healthProcessor = healthProcessor;

            FillFlowContainer flow;

            AutoSizeAxes = Axes.Both;
            AddInternal(flow = new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Horizontal
            });

            for (int i = 0; i < 10; i++)
                hearts[i] = new Sprite { Size = new Vector2(10) };

            hearts.ForEach(flow.Add);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            healthProcessor.Health.BindValueChanged(onHealthChanged, true);
        }

        private void onHealthChanged(ValueChangedEvent<double> health)
        {
            for (int i = 0; i < 10; i++)
            {
                if (health.NewValue == 1)
                    hearts[i].Texture = textures.Get("HUD/HP/full");
                else
                    hearts[i].Texture = getTexture(health.NewValue, i);
            }
        }

        private Texture getTexture(double value, int index)
        {
            var heartHealth = value * 10 - index;

            if (heartHealth > 1)
                return textures.Get("HUD/HP/full");

            if (heartHealth > 0.75f)
                return textures.Get("HUD/HP/75");

            if (heartHealth > 0.5f)
                return textures.Get("HUD/HP/50");

            if (heartHealth > 0.25f)
                return textures.Get("HUD/HP/25");

            return textures.Get("HUD/HP/0");
        }
    }
}
