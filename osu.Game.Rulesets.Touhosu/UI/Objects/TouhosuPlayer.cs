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
        private readonly Sprite focus1;
        private readonly Sprite focus2;

        public TouhosuPlayer()
        {
            RelativeSizeAxes = Axes.Both;
            AddRangeInternal(new Drawable[]
            {
                Player = new Container
                {
                    Origin = Anchor.Centre,
                    Position = new Vector2(TouhosuPlayfield.ACTUAL_SIZE.X / 2f, TouhosuPlayfield.ACTUAL_SIZE.Y - 20),
                    Children = new Drawable[]
                    {
                        focus1 = new Sprite
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Size = new Vector2(60),
                            Scale = new Vector2(0.7f),
                            Alpha = 0,
                            AlwaysPresent = true,
                        },
                        focus2 = new Sprite
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Size = new Vector2(60),
                            Scale = new Vector2(0.7f),
                            Alpha = 0,
                            AlwaysPresent = true,
                        },
                        drawablePlayer = new Sprite
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Size = new Vector2(23.25f, 33.75f),
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
            focus1.Texture = focus2.Texture = textures.Get("Player/focus");

            focus1.Spin(4000, RotationDirection.Clockwise);
            focus2.Spin(4000, RotationDirection.CounterClockwise);
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
                    onFocusPressed();
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
                    onFocusReleased();
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
                var position = Math.Clamp(Player.X + Math.Sign(horizontalDirection) * Clock.ElapsedFrameTime * base_speed * speedMultiplier, drawablePlayer.DrawWidth / 2f, TouhosuPlayfield.ACTUAL_SIZE.X - drawablePlayer.DrawWidth / 2f);

                drawablePlayer.Scale = new Vector2(Math.Abs(drawablePlayer.Scale.X) * (horizontalDirection > 0 ? 1 : -1), drawablePlayer.Scale.Y);

                if (position == Player.X)
                    return;

                Player.X = (float)position;
            }

            if (verticalDirection != 0)
            {
                var position = Math.Clamp(Player.Y + Math.Sign(verticalDirection) * Clock.ElapsedFrameTime * base_speed * speedMultiplier, drawablePlayer.DrawHeight / 2f, TouhosuPlayfield.ACTUAL_SIZE.Y - drawablePlayer.DrawHeight / 2f);

                if (position == Player.Y)
                    return;

                Player.Y = (float)position;
            }
        }

        private void onFocusPressed()
        {
            speedMultiplier = 0.5f;

            focus1.ScaleTo(1, 200, Easing.Out);
            focus1.FadeIn(200);

            focus2.ScaleTo(1, 200, Easing.Out);
            focus2.FadeIn(200);
        }

        private void onFocusReleased()
        {
            speedMultiplier = 1;

            focus1.ScaleTo(0.7f, 200, Easing.Out);
            focus1.FadeOut(200);

            focus2.ScaleTo(0.7f, 200, Easing.Out);
            focus2.FadeOut(200);
        }
    }
}
