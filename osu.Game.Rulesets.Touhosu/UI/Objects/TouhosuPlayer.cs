using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osuTK;
using System;
using osuTK.Graphics;
using osu.Framework.Input.Bindings;
using System.Collections.Generic;
using osu.Game.Rulesets.UI;
using osu.Framework.Bindables;
using osu.Game.Rulesets.Touhosu.Extensions;
using osu.Game.Rulesets.Touhosu.Replays;
using osu.Framework.Input.Events;

namespace osu.Game.Rulesets.Touhosu.UI.Objects
{
    public class TouhosuPlayer : CompositeDrawable, IKeyBindingHandler<TouhosuAction>
    {
        public static readonly float HITBOX_SIZE = 7;

        public static readonly float GRAZE_SIZE = HITBOX_SIZE * 3;

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
                    Position = new Vector2(TouhosuPlayfield.PLAYFIELD_SIZE.X / 2f, TouhosuPlayfield.PLAYFIELD_SIZE.Y - 20),
                    Children = new Drawable[]
                    {
                        animationContainer = new Container
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Size = new Vector2(23.25f, 33.75f),
                        },
                        focus = new FocusAnimation()
                    }
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            state.BindValueChanged(onStateChanged, true);
        }

        private bool isDead;

        public void Die()
        {
            isDead = true;
            onFocusReleased();
            onShootReleased();
            Player.FadeOut(500, Easing.Out);
        }

        public void PlayMissAnimation()
        {
            if (isDead)
                return;

            animationContainer.FlashColour(Color4.Red, 1000, Easing.OutQuint);
        }

        public Vector2 PlayerPosition() => Player.Position;

        public bool OnPressed(KeyBindingPressEvent<TouhosuAction> e)
        {
            if (isDead)
                return true;

            switch (e.Action)
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

        public void OnReleased(KeyBindingReleaseEvent<TouhosuAction> e)
        {
            if (isDead)
                return;

            switch (e.Action)
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

            var replayState = (GetContainingInputManager().CurrentState as RulesetInputManagerInputState<TouhosuAction>)?.LastReplayState as TouhosuFramedReplayInputHandler.TouhosuReplayState;

            if (replayState?.Position.Value != null)
            {
                Player.Position = replayState.Position.Value;
            }
            else
            {
                move(Clock.ElapsedFrameTime, horizontalDirection, verticalDirection);
            }

            if (isDead)
                return;

            updatePlayerState();
        }

        private void move(double elapsedTime, int horizontalDirection, int verticalDirection)
        {
            var movingH = horizontalDirection != 0;
            var movingV = verticalDirection != 0;

            if (!movingV && !movingH)
                return;

            // Diagonal movement
            if (movingV && movingH)
            {
                var oldX = Player.X;
                var oldY = Player.Y;
                var newX = oldX + Math.Sign(horizontalDirection) * elapsedTime * base_speed * speedMultiplier;
                var newY = oldY + Math.Sign(verticalDirection) * elapsedTime * base_speed * speedMultiplier;

                var expectedDistance = Math.Abs(newX - oldX);
                var realDistance = Vector2.Distance(new Vector2(oldX, oldY), new Vector2((float)newX, (float)newY));
                var offset = Math.Sqrt(MathExtensions.Pow(expectedDistance - realDistance) / 2);

                newX += (horizontalDirection > 0 ? -1 : 1) * offset;
                newY += (verticalDirection > 0 ? -1 : 1) * offset;

                newX = Math.Clamp(newX, animationContainer.Width / 2, TouhosuPlayfield.PLAYFIELD_SIZE.X - animationContainer.Width / 2);
                newY = Math.Clamp(newY, animationContainer.Height / 2, TouhosuPlayfield.PLAYFIELD_SIZE.Y - animationContainer.Height / 2);

                Player.Position = new Vector2((float)newX, (float)newY);
                return;
            }

            if (movingV)
            {
                var position = Math.Clamp(Player.Y + Math.Sign(verticalDirection) * elapsedTime * base_speed * speedMultiplier, animationContainer.Height / 2, TouhosuPlayfield.PLAYFIELD_SIZE.Y - animationContainer.Height / 2);
                Player.Y = (float)position;
                return;
            }

            if (movingH)
            {
                var position = Math.Clamp(Player.X + Math.Sign(horizontalDirection) * elapsedTime * base_speed * speedMultiplier, animationContainer.Width / 2, TouhosuPlayfield.PLAYFIELD_SIZE.X - animationContainer.Width / 2);
                Player.X = (float)position;
            }
        }

        public List<Card> GetCards() => cardsController.GetCards();

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
