using osu.Framework.Graphics;
using osu.Framework.Input.Bindings;
using osu.Framework.Input;
using osu.Framework.Localisation;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Touhosu.Objects;
using osu.Game.Rulesets.Touhosu.UI;
using osu.Game.Rulesets.UI;
using osu.Game.Screens.Play;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Touhosu.UI.Objects;
using osuTK;

namespace osu.Game.Rulesets.Touhosu.Mods
{
    public class TouhosuModRelax : ModRelax, IApplicableToDrawableRuleset<TouhosuHitObject>, IApplicableToPlayer
    {
        public override LocalisableString Description => "Use your mouse to dodge!";
        private DrawableRuleset drawableRuleset;

        public void ApplyToDrawableRuleset(DrawableRuleset<TouhosuHitObject> drawableRuleset)
        {
            this.drawableRuleset = drawableRuleset;
        }

        public void ApplyToPlayer(Player player)
        {
            if (!drawableRuleset.HasReplayLoaded.Value)
            {
                var touhosuPlayfield = (TouhosuPlayfield)drawableRuleset.Playfield;
                touhosuPlayfield.TouhosuArea.Add(new MouseInputHelper(touhosuPlayfield.Player));
            }
        }
    }

    public partial class MouseInputHelper : Drawable, IKeyBindingHandler<TouhosuAction>, IRequireHighFrequencyMousePosition
    {
        private readonly TouhosuPlayer player;

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => true;

        public MouseInputHelper(TouhosuPlayer player)
        {
            this.player = player;
            RelativeSizeAxes = Axes.Both;
        }

        // disable keyboard controls
        public bool OnPressed(KeyBindingPressEvent<TouhosuAction> e) => true;

        public void OnReleased(KeyBindingReleaseEvent<TouhosuAction> e)
        {
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            player.SetPosition(e.MousePosition);
            return base.OnMouseMove(e);
        }
    }
}
