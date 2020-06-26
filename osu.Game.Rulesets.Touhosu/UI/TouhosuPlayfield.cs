using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Touhosu.UI.Objects;
using osu.Game.Rulesets.UI;
using osuTK;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;
using osu.Game.Rulesets.Touhosu.UI.HUD;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Touhosu.Objects.Drawables;
using osu.Game.Rulesets.Touhosu.Extensions;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Touhosu.UI
{
    public class TouhosuPlayfield : Playfield
    {
        public static readonly Vector2 BASE_SIZE = new Vector2(512, 384);
        public static readonly Vector2 ACTUAL_SIZE = new Vector2(307, 384);
        public static readonly float X_SCALE_MULTIPLIER = 0.6f;

        private readonly TouhosuRuleset ruleset;
        private TouhosuPlayer player;

        public TouhosuPlayfield(TouhosuRuleset ruleset)
        {
            this.ruleset = ruleset;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            Masking = true;
            BorderThickness = 1.5f;
            MaskingSmoothness = 1;
            BorderColour = Color4.White;

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0,
                    AlwaysPresent = true,
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(X_SCALE_MULTIPLIER, 1),
                    Masking = true,
                    Children = new Drawable[]
                    {
                        player = new TouhosuPlayer(),
                        HitObjectContainer
                    }
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(1 - X_SCALE_MULTIPLIER, 1),
                    RelativePositionAxes = Axes.Both,
                    X = X_SCALE_MULTIPLIER,
                    Children = new Drawable[]
                    {
                        new Sprite
                        {
                            RelativeSizeAxes = Axes.Both,
                            Texture = textures.Get("Frame")
                        },
                        new FillFlowContainer
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(0, 5),
                            Children = new Drawable[]
                            {
                                new HealthDisplay(ruleset.HealthProcessor),
                                new ScoreDisplay(ruleset.ScoreProcessor)
                            }
                        }
                    }
                }
            };

            player.HitObjects = HitObjectContainer;
        }

        public bool CheckHit(DrawableBullet obj)
        {
            var radius = obj.Size.X / 2;
            var distance = MathExtensions.Distance(player.PlayerPosition(), obj.Position);
            var isHit = distance < radius + 2;

            if (isHit)
                player.PlayMissAnimation();

            return isHit;
        }

        public float CheckDistance(DrawableBullet obj) => (float)MathExtensions.Distance(player.PlayerPosition(), obj.Position);

        public float GetPlayerAngle(DrawableHomingBullet obj) => MathExtensions.GetPlayerAngle(player, obj.Position);

        public override void Add(DrawableHitObject h)
        {
            base.Add(h);

            if (h is DrawableBullet bullet)
            {
                bullet.CheckHit += CheckHit;
                bullet.CheckDistance += CheckDistance;
            }

            if (h is DrawableHomingBullet homingBullet)
            {
                homingBullet.PlayerAngle = GetPlayerAngle;
            }
        }

        private bool failInvoked;

        protected override void Update()
        {
            base.Update();

            if (!ruleset.HealthProcessor.HasFailed)
                return;

            if (failInvoked)
                return;

            onFail();
            failInvoked = true;
        }

        private void onFail()
        {
            player.Die();
        }
    }
}
