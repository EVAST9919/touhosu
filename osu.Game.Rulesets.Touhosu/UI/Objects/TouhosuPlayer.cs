using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osuTK;
using System;
using osuTK.Graphics;
using osu.Framework.Input.Bindings;
using osu.Framework.Graphics.Shapes;
using System.Collections.Generic;
using osu.Game.Rulesets.UI;
using osu.Framework.Bindables;

namespace osu.Game.Rulesets.Touhosu.UI.Objects
{
    public class TouhosuPlayer : CompositeDrawable, IKeyBindingHandler<TouhosuAction>
    {
        private const float base_speed = 0.2f;
        private const float shoot_delay = 80;

        private float speedMultiplier = 1;

        private readonly Bindable<PlayerState> state = new Bindable<PlayerState>(PlayerState.Idle);

        private HitObjectContainer hitObjects;

        public HitObjectContainer HitObjects
        {
            get => hitObjects;
            set
            {
                hitObjects = value;
                cardsController.HitObjects = value;
            }
        }

        public override bool RemoveCompletedTransforms => false;

        private int horizontalDirection;
        private int verticalDirection;

        public readonly Container Player;
        private readonly FocusAnimation focus;
        private readonly CardsController cardsController;
        private readonly Container animationContainer;

        public TouhosuPlayer()
        {
            RelativeSizeAxes = Axes.Both;
            AddRangeInternal(new Drawable[]
            {
                cardsController = new CardsController(),
                Player = new Container
                {
                    Origin = Anchor.Centre,
                    Position = new Vector2(TouhosuPlayfield.ACTUAL_SIZE.X / 2f, TouhosuPlayfield.ACTUAL_SIZE.Y - 20),
                    Children = new Drawable[]
                    {
                        focus = new FocusAnimation(),
                        animationContainer = new Container
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

            state.BindValueChanged(onStateChanged, true);
        }

        public Vector2 PlayerPosition() => Player.Position;

        public void PlayMissAnimation() => animationContainer.FlashColour(Color4.Red, 1000, Easing.OutQuint);

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
                    onShootPressed();
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
                    onShootReleased();
                    return;
            }
        }

        protected override void Update()
        {
            base.Update();

            if (horizontalDirection != 0)
            {
                var position = Math.Clamp(Player.X + Math.Sign(horizontalDirection) * Clock.ElapsedFrameTime * base_speed * speedMultiplier, animationContainer.Width / 2, TouhosuPlayfield.ACTUAL_SIZE.X - animationContainer.Width / 2);

                if (position == Player.X)
                    return;

                Player.X = (float)position;
            }

            if (verticalDirection != 0)
            {
                var position = Math.Clamp(Player.Y + Math.Sign(verticalDirection) * Clock.ElapsedFrameTime * base_speed * speedMultiplier, animationContainer.Height / 2, TouhosuPlayfield.ACTUAL_SIZE.Y - animationContainer.Height / 2);

                if (position == Player.Y)
                    return;

                Player.Y = (float)position;
            }

            updatePlayerState();
        }

        public List<Card> GetCards() => cardsController.GetCards();

        public List<SmartCard> GetSmartCards() => cardsController.GetSmartCards();

        private bool isFocused;

        private void onFocusPressed()
        {
            isFocused = true;
            speedMultiplier = 0.5f;
            focus.Focus();
        }

        private void onFocusReleased()
        {
            isFocused = false;
            speedMultiplier = 1;
            focus.FocusLost();
        }

        private void onShootPressed()
        {
            cardsController.Shoot(PlayerPosition(), isFocused);
            Scheduler.AddDelayed(onShootPressed, shoot_delay);
        }

        private void onShootReleased()
        {
            Scheduler.CancelDelayedTasks();
        }

        private void updatePlayerState()
        {
            if (horizontalDirection == 1)
            {
                state.Value = PlayerState.Right;
                return;
            }

            if (horizontalDirection == -1)
            {
                state.Value = PlayerState.Left;
                return;
            }

            state.Value = PlayerState.Idle;
        }

        private void onStateChanged(ValueChangedEvent<PlayerState> s)
        {
            animationContainer.Child = new PlayerAnimation(s.NewValue);
        }
    }
}
