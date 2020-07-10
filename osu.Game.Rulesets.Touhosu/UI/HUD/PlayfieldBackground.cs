using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Touhosu.Configuration;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Touhosu.UI.HUD
{
    public class PlayfieldBackground : CompositeDrawable
    {
        private readonly Bindable<double> bgDim = new Bindable<double>();

        [Resolved(canBeNull: true)]
        private TouhosuRulesetConfigManager config { get; set; }

        private readonly Box bg;

        public PlayfieldBackground()
        {
            RelativeSizeAxes = Axes.Both;
            AddInternal(bg = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Black,
                AlwaysPresent = true,
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            config?.BindWith(TouhosuRulesetSetting.PlayfieldDim, bgDim);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            bgDim.BindValueChanged(dim => onDimChanged(dim.NewValue), true);
        }

        private void onDimChanged(double newDim)
        {
            bg.Alpha = (float)newDim;
        }
    }
}
