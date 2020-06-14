using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Animations;
using osu.Framework.Graphics.Textures;

namespace osu.Game.Rulesets.Touhosu.UI.Objects
{
    public class PlayerAnimation : TextureAnimation
    {
        private const double duration = 100;

        private readonly PlayerState state;

        public PlayerAnimation(PlayerState state)
        {
            this.state = state;

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            switch (state)
            {
                case PlayerState.Idle:
                    for (int i = 1; i <= 8; i++)
                        AddFrame(textures.Get($"Player/Player/idle-{i}"), duration);
                    break;

                case PlayerState.Left:
                    for (int i = 1; i <= 5; i++)
                        AddFrame(textures.Get($"Player/Player/left-{i}"), duration);
                    break;

                case PlayerState.Right:
                    for (int i = 1; i <= 5; i++)
                        AddFrame(textures.Get($"Player/Player/right-{i}"), duration);
                    break;
            }
        }
    }

    public enum PlayerState
    {
        Idle,
        Left,
        Right
    }
}
