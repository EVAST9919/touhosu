using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Touhosu.Scoring;
using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Touhosu.UI.HUD
{
    public class HealthDisplay : CompositeDrawable
    {
        private readonly OsuSpriteText text;
        private readonly TouhosuHealthProcessor healthProcessor;

        public HealthDisplay(TouhosuHealthProcessor healthProcessor)
        {
            this.healthProcessor = healthProcessor;

            AutoSizeAxes = Axes.Both;
            AddInternal(text = new OsuSpriteText());
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            healthProcessor.Health.BindValueChanged(onHealthChanged, true);
        }

        private void onHealthChanged(ValueChangedEvent<double> health)
        {
            text.Text = health.NewValue.ToString();
        }
    }
}
