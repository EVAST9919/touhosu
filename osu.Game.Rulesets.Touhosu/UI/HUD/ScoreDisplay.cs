using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Touhosu.Scoring;
using osu.Framework.Bindables;
using osu.Game.Graphics.Sprites;
using System;

namespace osu.Game.Rulesets.Touhosu.UI.HUD
{
    public class ScoreDisplay : CompositeDrawable
    {
        private readonly TouhosuScoreProcessor scoreProcessor;
        private readonly OsuSpriteText text;

        public ScoreDisplay(TouhosuScoreProcessor scoreProcessor)
        {
            this.scoreProcessor = scoreProcessor;

            AutoSizeAxes = Axes.Both;
            AddInternal(text = new OsuSpriteText());
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            scoreProcessor.TotalScore.BindValueChanged(onScoreChanged, true);
        }

        private void onScoreChanged(ValueChangedEvent<double> score)
        {
            text.Text = Math.Round(score.NewValue).ToString();
        }
    }
}
