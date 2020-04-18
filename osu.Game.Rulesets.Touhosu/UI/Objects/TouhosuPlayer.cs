using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osuTK;
using System;
using osuTK.Graphics;
using osu.Framework.Input.Bindings;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Shapes;

namespace osu.Game.Rulesets.Touhosu.UI.Objects
{
    public class TouhosuPlayer : CompositeDrawable, IKeyBindingHandler<TouhosuAction>
    {
        private const float base_speed = 0.2f;
        private float speedMultiplier = 1;

        [Resolved]
        private TextureStore textures { get; set; }

        public override bool RemoveCompletedTransforms => false;

        private int horizontalDirection;
        private int verticalDirection;

        public readonly Container Player;
        private readonly Sprite drawablePlayer;

        public TouhosuPlayer()
        {
            RelativeSizeAxes = Axes.Both;
            AddRangeInternal(new Drawable[]
            {
                Player = new Container
                {
                    Origin = Anchor.Centre,
                    Position = new Vector2(TouhosuPlayfield.ACTUAL_SIZE.X / 2, TouhosuPlayfield.ACTUAL_SIZE.Y - 20),
                    Size = new Vector2(15, 22),
                    Children = new Drawable[]
                    {
                        drawablePlayer = new Sprite
                        {
                            RelativeSizeAxes = Axes.Both
                        },
                        new Circle
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Size = new Vector2(3),
                            Colour = Color4.Red,
                        },
                        new Circle
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Size = new Vector2(2),
                        }
                    }
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            drawablePlayer.Texture = textures.Get("player");
        }

        public Vector2 PlayerPosition() => Player.Position;

        public void PlayMissAnimation() => drawablePlayer.FlashColour(Color4.Red, 1000, Easing.OutQuint);

        public bool OnPressed(TouhosuAction action)
        {
            switch (action)
            {
                case TouhosuAction.MoveLeft:
                    horizontalDirection--;
                    return true;

                case TouhosuAction.MoveRight:
                    horizontalDirection++;
                    return true;

                case TouhosuAction.MoveUp:
                    verticalDirection--;
                    return true;

                case TouhosuAction.MoveDown:
                    verticalDirection++;
                    return true;

                case TouhosuAction.Focus:
                    speedMultiplier = 0.5f;
                    return true;

                case TouhosuAction.Shoot:
                    return true;
            }

            return false;
        }

        public void OnReleased(TouhosuAction action)
        {
            switch (action)
            {
                case TouhosuAction.MoveLeft:
                    horizontalDirection++;
                    return;

                case TouhosuAction.MoveRight:
                    horizontalDirection--;
                    return;

                case TouhosuAction.MoveUp:
                    verticalDirection++;
                    return;

                case TouhosuAction.MoveDown:
                    verticalDirection--;
                    return;

                case TouhosuAction.Focus:
                    speedMultiplier = 1;
                    return;

                case TouhosuAction.Shoot:
                    return;
            }
        }

        protected override void Update()
        {
            base.Update();

            if (horizontalDirection != 0)
            {
                var position = Math.Clamp(Player.X + Math.Sign(horizontalDirection) * Clock.ElapsedFrameTime * base_speed * speedMultiplier, 0, TouhosuPlayfield.ACTUAL_SIZE.X);

                Player.Scale = new Vector2(Math.Abs(Scale.X) * (horizontalDirection > 0 ? 1 : -1), Player.Scale.Y);

                if (position == Player.X)
                    return;

                Player.X = (float)position;
            }

            if (verticalDirection != 0)
            {
                var position = Math.Clamp(Player.Y + Math.Sign(verticalDirection) * Clock.ElapsedFrameTime * base_speed * speedMultiplier, 0, TouhosuPlayfield.ACTUAL_SIZE.Y);

                if (position == Player.Y)
                    return;

                Player.Y = (float)position;
            }
        }
    }
}
